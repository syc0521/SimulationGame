using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Data.Event.Common;
using Game.GamePlaySystem.Loading;
using Game.UI.UISystem;
using UnityEngine;

namespace Game.UI.Panel.Loading
{
    public class LoadingPanel : UIPanel
    {
        public LoadingPanel_Nodes nodes;
        private Coroutine _tipCoroutine;
        private List<string> _loadingTips;
        private int _currentIndex = -1;
        private bool _showTips = true;
        
        public override void OnCreated()
        {
            base.OnCreated();
            EventCenter.AddListener<LoadingEvent>(ShowLoadingProgress);
            EventCenter.AddListener<LoadSceneFinishedEvent>(StopShowTips);
        }

        public override void OnShown()
        {
            base.OnShown();
            _loadingTips = CommonSystem.Instance.GetLoadingTips();
            _tipCoroutine = StartCoroutine(ShowLoadingTips());
            LoadGame();
        }

        public override void OnDestroyed()
        {
            if (_tipCoroutine != null)
            {
                StopCoroutine(_tipCoroutine);
            }
            
            EventCenter.RemoveListener<LoadingEvent>(ShowLoadingProgress);
            EventCenter.RemoveListener<LoadSceneFinishedEvent>(StopShowTips);
            base.OnDestroyed();
        }

        private void LoadGame()
        {
            LoadingManager.Instance.StartLoadingGame();
        }

        private void ShowLoadingProgress(LoadingEvent evt)
        {
            nodes.slider.SetValue(evt.progress);
            nodes.slider.SetDetailText(evt.text);
        }

        private IEnumerator ShowLoadingTips()
        {
            while (_showTips)
            {
                _currentIndex = (_currentIndex + 1) % 4;
                nodes.tip_txt.text = _loadingTips[_currentIndex];
                yield return new WaitForSeconds(4.0f);
            }
        }

        private void StopShowTips(LoadSceneFinishedEvent evt)
        {
            _showTips = false;
        }
    }
}