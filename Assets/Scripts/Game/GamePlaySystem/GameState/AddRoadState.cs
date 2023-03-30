using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.BurstUtil;
using Game.GamePlaySystem.StateMachine;
using Game.Input;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.GamePlaySystem.GameState
{
    public class AddRoadState : StateBase
    {
        private int _count = -1;
        private int2 _lastPos = new(0, 0);
        private Stack<(int2[], int2)> _roadStack;
        private Stack<GameObject[]> _roadObjStack;

        private Stack<(int2[], int2)> _roadRedoStack;
        private Stack<GameObject[]> _roadRedoObjStack;

        private GameObject _startObj, _tempStartObj;

        public override void OnEnter(params object[] list)
        {
            _roadStack = new(5);
            _roadObjStack = new(5);
            _roadRedoStack = new(5);
            _roadRedoObjStack = new(5);
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildingManagedSystem>().SetGridVisible(true);
            EventCenter.AddListener<TouchEvent>(PlaceRoad);
        }

        public override void OnLeave(params object[] list)
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildingManagedSystem>().SetGridVisible(false);
            EventCenter.RemoveListener<TouchEvent>(PlaceRoad);
            if ((bool)list[0])
            {
                ConstructRoad();
            }

            if (_startObj != null)
            {
                Object.Destroy(_startObj);
            }
            if (_tempStartObj != null)
            {
                Object.Destroy(_tempStartObj);
            }
            
            foreach (var obj in _roadObjStack.SelectMany(redoObjs => redoObjs))
            {
                Object.Destroy(obj);
            }
            foreach (var obj in _roadRedoObjStack.SelectMany(redoObjs => redoObjs))
            {
                Object.Destroy(obj);
            }
            _startObj = null;
            _roadStack.Clear();
            _roadObjStack.Clear();
            _roadRedoStack.Clear();
            _roadRedoObjStack.Clear();
        }

        private void PlaceRoad(TouchEvent evt)
        {
            var collisionWorld = BuildingManager.Instance.GetCollisionWorld();
            var raycastInput = BuildingManager.Instance.GetRaycastInput(new float3(evt.pos, 0));
            
            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entity = hit.Entity;
                if (entityManager.HasComponent<BuildingPlane>(entity))
                {
                    var spawnPos = BuildingUtils.GetGridPos(hit.Position);
                    if (_count > 0 && spawnPos.Equals(_lastPos)) return; // 已建造道路且上次点击的位置和此次相同

                    _count++;
                    if (_count > 0)
                    {
                        CreateRoad(spawnPos, _lastPos);
                    }
                    else if (_count == 0)
                    {
                        var prefab = ConfigTable.Instance.GetBuilding(4);
                        _startObj = Object.Instantiate(prefab, new Vector3(spawnPos[0], 0, spawnPos[1]), Quaternion.identity);
                        MaterialUtil.SetTransparency(_startObj);
                    }
                    EventCenter.DispatchEvent(new RoadConstructEvent());
                    _lastPos = spawnPos;
                }
            }
        }

        public void UndoRoad()
        {
            if (_count > 0)
            {
                _count--;
                var roadData = _roadStack.Pop();
                _roadRedoStack.Push(roadData);
                _lastPos = roadData.Item2;
                var roadObj = _roadObjStack.Pop();
                foreach (var obj in roadObj)
                {
                    obj.SetActive(false);
                }
                _roadRedoObjStack.Push(roadObj);
                EventCenter.DispatchEvent(new RoadConstructEvent());
            }
            else if (_count == 0)
            {
                _count--;
                _tempStartObj = _startObj;
                _startObj.SetActive(false);
                _startObj = null;
                EventCenter.DispatchEvent(new RoadConstructEvent());
            }
        }

        public void RedoRoad()
        {
            if (_count == -1 && _tempStartObj != null)
            {
                _count++;
                _startObj = _tempStartObj;
                _startObj.SetActive(true);
                _tempStartObj = null;
                EventCenter.DispatchEvent(new RoadConstructEvent());
            }
            else if (_count >= 0 && _roadRedoStack.Count > 0)
            {
                _count++;
                var roadData = _roadRedoStack.Pop();
                _roadStack.Push(roadData);
                _lastPos = roadData.Item2;
                var roadObj = _roadRedoObjStack.Pop();
                foreach (var obj in roadObj)
                {
                    obj.SetActive(true);
                }
                _roadObjStack.Push(roadObj);
                EventCenter.DispatchEvent(new RoadConstructEvent());
            }
        }

        /// <summary>
        /// ECS构建道路（带去重）
        /// </summary>
        public void ConstructRoad()
        {
            var roadData = _roadStack.SelectMany(items => items.Item1).ToHashSet();
            foreach (var road in roadData)
            {
                var id = BuildingManager.Instance.GetID();
                BuildingManager.Instance.SetBuildingData(id, new BuildingData
                {
                    level = 1,
                    position = new Vector2(road[0], road[1]),
                    rotation = 0,
                    type = 4,
                });
                var pos = new float3(road[0], 0, road[1]);
                BuildingManager.Instance.SetGridData(pos, 0, 4, 4);
                BuildingManager.Instance.Build(pos, 4, id);
            }
            Managers.Get<ISaveDataManager>().SaveData();
        }

        private void CreateRoad(int2 pos, int2 lastPos)
        {
            var road = GetSingleRoadData(pos, lastPos);

            if (_roadRedoStack.Count > 0)
            {
                _roadRedoStack.Clear();
                foreach (var obj in _roadRedoObjStack.SelectMany(redoObjs => redoObjs))
                {
                    Object.Destroy(obj);
                }
                _roadRedoObjStack.Clear();
            }
            
            _roadStack.Push((road, _lastPos));
            _tempStartObj = null;
            var roadObjs = new GameObject[road.Length];
            for (int i = 0; i < road.Length; i++)
            {
                var prefab = ConfigTable.Instance.GetBuilding(4);
                var block = road[i];
                roadObjs[i] = Object.Instantiate(prefab, new Vector3(block[0], 0, block[1]), Quaternion.identity);
                MaterialUtil.SetTransparency(roadObjs[i]);
            }
            _roadObjStack.Push(roadObjs);
        }

        private int2[] GetSingleRoadData(int2 pos, int2 lastPos)
        {
            var deltaPos = math.abs(pos - lastPos);
            var length = deltaPos[0] + deltaPos[1];
            var road = new HashSet<int2>(length);

            var startX = lastPos[0];
            var endX = pos[0];
            if (endX < startX)
            {
                Swap(ref startX, ref endX);
            }

            for (int i = startX; i <= endX; i++) //x轴
            {
                if (deltaPos[0] == 0 && i == pos[0])
                {
                    continue;
                }
                road.Add(new int2(i, lastPos[1]));
            }
            
            var startY = lastPos[1];
            var endY = pos[1];
            if (endY < startY)
            {
                Swap(ref startY, ref endY);
            }
            
            for (int i = startY; i <= endY; i++) //z轴
            {
                if (deltaPos[1] == 0 && i == pos[1])
                {
                    continue;
                }
                road.Add(new int2(pos[0], i));
            }
            
            road.RemoveWhere(item => item.Equals(lastPos));
            return road.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Swap(ref int a, ref int b)
        {
            (a, b) = (b, a);
        }

        public bool CanUndo()
        {
            return _roadStack.Count > 0;
        }

        public bool CanRedo()
        {
            return _roadRedoStack.Count > 0;
        }
    }
}