using System;
using Game.Core;
using Game.UI.Component;
using Game.UI.Panel;
using Game.UI.Panel.Task;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Widget
{
    public class TaskIconWidget : WidgetBase, IListWidget, IPointerClickHandler
    {
        public CustomImage icon_img;
        private int id;

        public void Refresh(ListData data)
        {
            if (data is not TaskListData taskListData)
            {
                return;
            }
            
            icon_img.SetIcon(new AtlasSpriteID
            {
                atlas = AtlasEnum.Task,
                resName = $"task_{taskListData.id}"
            });

            id = taskListData.id;

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UIManager.Instance.OpenPanel<TaskDetailPanel>(new TaskDetailPanelOption
            {
                taskID = id,
            });
        }
    }
}