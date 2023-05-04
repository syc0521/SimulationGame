using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Data.Event.Common;
using Game.Data.TableData;
using Game.GamePlaySystem.Loading;
using Game.LevelAndEntity.ResLoader;
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
        private LoadingPictureData _picData;
        private float _fadeValue = 0f;
        private static readonly int Value = Shader.PropertyToID("_Value");

        public override void OnCreated()
        {
            base.OnCreated();
            EventCenter.AddListener<LoadingEvent>(ShowLoadingProgress);
            EventCenter.AddListener<LoadSceneFinishedEvent>(StopShowTips);
            _picData = LoadingManager.Instance.GetLoadingPic();
        }

        public override void OnShown()
        {
            base.OnShown();
            _loadingTips = CommonSystem.Instance.GetLoadingTips();
            _tipCoroutine = StartCoroutine(ShowLoadingTips());
            nodes.image.SetPicture($"loading{_picData.Picid:D2}");
            nodes.picture_txt.text = _picData.Name;
            StartCoroutine(FadePicture());
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
            nodes.image.material.SetFloat(Value, 0f);
            Managers.Get<IResLoader>().UnloadRes(ResEnum.Picture, $"loading{_picData.Picid:D2}");
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

        private IEnumerator FadePicture()
        {
            yield return new WaitForSeconds(0.4f);
            while (_fadeValue < 1.0f)
            {
                nodes.image.material.SetFloat(Value, _fadeValue);
                _fadeValue += Time.deltaTime / 8.0f;
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
    }
}