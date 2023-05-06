using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.Audio;
using Game.Core;
using Game.Data.Event.Common;
using Game.GamePlaySystem.Loading;
using Game.UI.Panel.Loading;
using UnityEngine;

namespace Game.UI.Panel.Start
{
    public class StartPanel : UIPanel
    {
        public StartPanel_Nodes nodes;
        private static readonly int FaceDilate = Shader.PropertyToID("_FaceDilate");
        private TweenerCore<float, float, FloatOptions> _anim;

        public override void OnCreated()
        {
            base.OnCreated();
            nodes.start_btn.onClick.AddListener(StartGame);
            EventCenter.AddListener<LoadSceneFinishedEvent>(Close);
        }

        public override void OnShown()
        {
            base.OnShown();
            _anim = nodes.tip_text.materialForRendering.DOFloat(0.2f, FaceDilate, 1.2f).SetLoops(-1, LoopType.Yoyo);
        }

        public override void OnDestroyed()
        {
            base.OnDestroyed();
            nodes.start_btn.onClick.RemoveListener(StartGame);
            _anim.Pause();
            nodes.tip_text.materialForRendering.SetFloat(FaceDilate, 0f);
            EventCenter.AddListener<LoadSceneFinishedEvent>(Close);
        }

        private void StartGame()
        {
            Managers.Get<IAudioManager>().PlaySFX(SFXType.Button_Effect);
            UIManager.Instance.OpenPanel<LoadingPanel>();
        }

        private void Close(LoadSceneFinishedEvent evt)
        {
            CloseSelf();
        }
    }
}