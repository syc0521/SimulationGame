using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Component
{
    public class ListData
    {

    }

    public class ListComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject template;

        private List<GameObject> _objects;
        private List<ListData> _listData;
        private Transform content;

        public void Init()
        {
            content = transform.childCount > 0 ? transform.GetChild(0).GetChild(0) : transform;
            _objects = new();
            _listData = new();
        }

        public void AddItem(ListData data)
        {
            if (template == null)
            {
                Debug.LogError("列表没有生成模板！！");
            }
            var obj = Instantiate(template, content);
            obj.GetComponent<IListWidget>().Refresh(data);
            _objects.Add(obj);
            _listData.Add(data);
        }

        public void DeleteItem(int index)
        {
            Destroy(_objects[index]);
            _listData.Remove(_listData[index]);
        }

        public ListData GetData(int index)
        {
            if (index < 0 || index >= _listData.Count)
            {
                return null;
            }
            return _listData[index];
        }

        public void SetData(int index, ListData data)
        {
            if (index < 0 || index >= _listData.Count)
            {
                return;
            }

            _listData[index] = data;
            _objects[index].GetComponent<IListWidget>().Refresh(data);
        }

        public IListWidget GetListObject(int index)
        {
            if (index < 0 || index >= _listData.Count)
            {
                return null;
            }

            return _objects[index].GetComponent<IListWidget>();
        }

        public void Clear()
        {
            _listData?.Clear();
            _objects?.ForEach(Destroy);
        }
    }
}