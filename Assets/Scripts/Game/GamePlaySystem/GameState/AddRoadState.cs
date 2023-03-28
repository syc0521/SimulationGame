using System.Collections.Generic;
using System.Linq;
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

        private Queue<(int2[], int2)> _roadRedoQueue;
        private Queue<GameObject[]> _roadRedoObjQueue;

        private GameObject _startObj, _tempStartObj;

        public override void OnEnter(params object[] list)
        {
            _roadStack = new(5);
            _roadObjStack = new(5);
            _roadRedoQueue = new(5);
            _roadRedoObjQueue = new(5);
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildingManagedSystem>().SetGridVisible(true);
            EventCenter.AddListener<TouchEvent>(PlaceRoad);
        }

        public override void OnLeave(params object[] list)
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildingManagedSystem>().SetGridVisible(false);
            EventCenter.RemoveListener<TouchEvent>(PlaceRoad);
            Object.Destroy(_startObj);
            _startObj = null;
            _roadStack.Clear();
            _roadObjStack.Clear();
            _roadRedoQueue.Clear();
            _roadRedoObjQueue.Clear();
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
                _roadRedoQueue.Enqueue(roadData);
                var roadObj = _roadObjStack.Pop();
                foreach (var obj in roadObj)
                {
                    obj.SetActive(false);
                }
                _roadRedoObjQueue.Enqueue(roadObj);
            }
            else if (_count == 0)
            {
                _count--;
                _tempStartObj = _startObj;
                _startObj = null;
            }
        }

        public void RedoRoad()
        {
            
        }

        private void CreateRoad(int2 pos, int2 lastPos)
        {
            var road = GetSingleRoad(pos, lastPos);

            if (_roadRedoQueue.Count > 0)
            {
                _roadRedoQueue.Clear();
                foreach (var obj in _roadRedoObjQueue.SelectMany(redoObjs => redoObjs))
                {
                    Object.Destroy(obj);
                }
                _roadRedoObjQueue.Clear();
            }
            
            _roadStack.Push((road, _lastPos));
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

        private int2[] GetSingleRoad(int2 pos, int2 lastPos)
        {
            var deltaPos = math.abs(pos - lastPos);
            var length = deltaPos[0] + deltaPos[1];
            var road = new List<int2>(length);

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

            road.RemoveAll(item => item.Equals(lastPos));
            return road.ToArray();
        }

        private void Swap(ref int a, ref int b)
        {
            (a, b) = (b, a);
        }
    }
}