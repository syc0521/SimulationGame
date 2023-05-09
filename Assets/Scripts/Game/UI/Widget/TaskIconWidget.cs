using System;
using Game.Core;
using Game.Data;
using Game.UI.Component;
using Game.UI.Panel;
using Game.UI.Panel.Task;
using Game.UI.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Widget
{
    public class TaskIconWidget : WidgetBase, IListWidget, IPointerClickHandler
    {
        public CustomImage icon_img;
        public Image finish_img;
        private int id;

        public override void OnDestroyed()
        {
            icon_img.OnDestroyed();
            base.OnDestroyed();
        }

        public void Refresh(ListData data)
        {
            if (data is not TaskListData taskListData)
            {
                return;
            }
            
            icon_img.SetIcon(IconUtility.GetTaskIcon(taskListData.id));
            finish_img.gameObject.SetActive(taskListData.state is TaskState.Finished);
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