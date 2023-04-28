using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Core;
using Game.Data;
using Game.GamePlaySystem.GM;
using Game.UI.Component;
using Game.UI.Widget;
using UnityEngine;

namespace Game.UI.Panel.GM
{
    public class GMCategoryListData : ListData
    {
        public string name;
        public int index;
        public Action<string, int> callback;
    }

    public class GMInfoListData : ListData
    {
        public string name;
        public int index;
    }
    
    public class GMPanel : UIPanel
    {
        public GMPanel_Nodes nodes;
        private List<string> _gmCategory, _gmName;
        private int _gmIndex, _gmCategoryIndex;
        private ICommand _currentCommand;
        private Type _currentType;
        private List<FieldInfo> _fieldInfos;
        
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.close_btn.onClick.AddListener(CloseSelf);
            nodes.run_btn.onClick.AddListener(Run);
            _gmCategory = GMManager.Instance.GetGMCategory();
        }

        public override void OnShown()
        {
            base.OnShown();
            InitGMCategory();
        }

        public override void OnDestroyed()
        {
            nodes.run_btn.onClick.RemoveListener(Run);
            nodes.close_btn.onClick.RemoveListener(CloseSelf);
            base.OnDestroyed();
        }

        private void InitGMCategory()
        {
            for (var i = 0; i < _gmCategory.Count; i++)
            {
                nodes.type_list.AddItem(new GMCategoryListData
                {
                    name = _gmCategory[i],
                    index = i,
                    callback = GMCategoryCallbackHandler
                });
            }

            Refresh(_gmCategory[0]);
        }

        private void GMCategoryCallbackHandler(string gmName, int index)
        {
            _gmCategoryIndex = index;
            Refresh(gmName);
        }

        private void Refresh(string gmName)
        {
            nodes.name_list.Clear();
            _gmName = GMManager.Instance.GetGMNames(gmName);
            for (var i = 0; i < _gmName.Count; i++)
            {
                nodes.name_list.AddItem(new GMCategoryListData
                {
                    name = _gmName[i],
                    index = i,
                    callback = ShowGMFeature
                });
            }
        }

        private void ShowGMFeature(string gmName, int index)
        {
            _currentType = GMManager.Instance.GetGMType(_gmCategory[_gmCategoryIndex], index);
            nodes.info_list.Clear();

            _fieldInfos = new();
            for (var i = 0; i < _currentType.GetFields().Length; i++)
            {
                var item = _currentType.GetFields()[i];
                if (item.FieldType == typeof(string) || item.FieldType == typeof(int) || item.FieldType == typeof(float))
                {
                    _fieldInfos.Add(item);
                    nodes.info_list.AddItem(new GMInfoListData
                    {
                        name = item.Name,
                        index = i,
                    });
                }
            }
        }

        private void Run()
        {
            var constructorInfo = _currentType.GetConstructor(new Type[] {});
            var obj = constructorInfo?.Invoke(new object[] { });
            for (var i = 0; i < _currentType.GetFields().Length; i++)
            {
                var item = _currentType.GetFields()[i];
                var listObj = nodes.info_list.GetListObject(i) as GMInfoWidget;
                if (listObj == null)
                {
                    continue;
                }
                
                if (item.FieldType == typeof(string))
                {
                    item.SetValue(obj, listObj.GetData());
                }
                else if (item.FieldType == typeof(int))
                {
                    item.SetValue(obj, int.Parse(listObj.GetData()));
                }
                else if (item.FieldType == typeof(float))
                {
                    item.SetValue(obj, float.Parse(listObj.GetData()));
                }
            }
            var command = obj as ICommand;
            command?.Run();
            Managers.Get<ISaveDataManager>().SaveData();
        }
    }
}