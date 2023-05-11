using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Start
{
    public class LogoPanel : UIPanel
    {
        public Image logo_img;
        public override void OnShown()
        {
            base.OnShown();
            logo_img.color = new Color(1, 1, 1, 0);
            StartCoroutine(ShowLogo());
        }

        private IEnumerator ShowLogo()
        {
            yield return new WaitForSeconds(0.5f);
            DOTween.ToAlpha(() => logo_img.color, x => logo_img.color = x, 1, 0.8f);
            yield return new WaitForSeconds(2f);
            DOTween.ToAlpha(() => logo_img.color, x => logo_img.color = x, 0, 0.8f);
            yield return new WaitForSeconds(1.5f);
            CloseSelf();
            UIManager.Instance.OpenPanel<StartPanel>();
        }
    }
}