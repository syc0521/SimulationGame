using DG.Tweening;
using Game.Data.Event;

namespace Game.UI.Panel
{
    public partial class MainPanel
    {
        private void ChangeHUDStatus(ShowHUDEvent evt)
        {
            ChangeHUDStatus(evt.HUDType);
        }

        private void ChangeHUDStatus(HUDType type)
        {
            switch (type)
            {
                case HUDType.All:
                    if (!nodes.operator_go.activeInHierarchy && nodes.task_go.activeInHierarchy)
                    {
                        PlayOperatorInAnim();
                    }
                    else if (!nodes.task_go.activeInHierarchy)
                    {
                        PlayStatusInAnim();
                    }
                    
                    nodes.operator_go.SetActive(true);
                    nodes.task_go.SetActive(true);
                    nodes.status_go.SetActive(true);
                    nodes.pause_go.SetActive(true);
                    break;
                case HUDType.Build:
                    nodes.operator_go.SetActive(false);
                    nodes.task_go.SetActive(false);
                    nodes.status_go.SetActive(false);
                    break;
                case HUDType.Detail:
                    PlayOperatorOutAnim();
                    break;
            }
        }

        private void PlayOperatorInAnim()
        {
            var length = _animation["MainPanel_OperatorIn"].clip.length;
            _animation["MainPanel_OperatorIn"].clip.SampleAnimation(gameObject, 0f);
            _animation.Play("MainPanel_OperatorIn");
            DOTween.Sequence().AppendInterval(length).AppendCallback(() =>
            {
                nodes.operator_go.SetActive(true);
            });
        }
        
        private void PlayOperatorOutAnim()
        {
            var length = _animation["MainPanel_OperatorOut"].clip.length;
            _animation["MainPanel_OperatorOut"].clip.SampleAnimation(gameObject, 0f);
            _animation.Play("MainPanel_OperatorOut");
            DOTween.Sequence().AppendInterval(length).AppendCallback(() =>
            {
                nodes.operator_go.SetActive(false);
            });
        }

        private void PlayStatusInAnim()
        {
            _animation["MainPanel_StatusIn"].clip.SampleAnimation(gameObject, 0f);
            _animation.Play("MainPanel_StatusIn");
        }

        private void PlayIntroAnim()
        {
            _animation["MainPanel_Intro"].clip.SampleAnimation(gameObject, 0f);
            _animation.Play("MainPanel_Intro");
        }
    }
}