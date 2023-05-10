using Game.Data;
using Game.GamePlaySystem.Task;
using Game.UI.Component;
using Game.UI.Panel.Task;
using TMPro;

namespace Game.UI.Widget
{
    public class DailyTaskWidget : WidgetBase, IListWidget
    {
        public TextMeshProUGUI title_txt, detail_txt;
        public FrameComponent state_frame;
        public CustomButton accepted_btn;
        private int _taskId;

        public override void OnCreated()
        {
            base.OnCreated();
            accepted_btn.onClick.AddListener(AcceptTask);
        }

        public override void OnDestroyed()
        {
            accepted_btn.onClick.RemoveListener(AcceptTask);
            base.OnDestroyed();
        }

        public void Refresh(ListData data)
        {
            if (data is DailyTaskListData dailyTaskListData)
            {
                title_txt.text = dailyTaskListData.name;
                detail_txt.text = dailyTaskListData.desc;
                _taskId = dailyTaskListData.taskId;
                var state = dailyTaskListData.state;
                var frame = state switch
                {
                    TaskState.Accepted => 2,
                    TaskState.Finished => 3,
                    _ => 1
                };
                state_frame.SetFrame(frame);
            }
        }

        private void AcceptTask()
        {
            TaskManager.Instance.ActivateTask(_taskId);
            TaskManager.Instance.RefreshTask();
            state_frame.SetFrame(2);
        }
    }
}