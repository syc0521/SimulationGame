using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Start
{
    public class LogoPanel : UIPanel
    {
        public Image logo_img;
        public TextMeshProUGUI desc_txt;
        
        public override void OnShown()
        {
            base.OnShown();
            logo_img.color = new Color(1, 1, 1, 0);
            StartCoroutine(ShowLogo());
        }

        private IEnumerator ShowLogo()
        {
            yield return new WaitForSeconds(1.2f);
            DOTween.ToAlpha(() => desc_txt.color, x => desc_txt.color = x, 1, 1.5f);
            DOTween.ToAlpha(() => logo_img.color, x => logo_img.color = x, 1, 1.5f);
            yield return new WaitForSeconds(2f);
            DOTween.ToAlpha(() => logo_img.color, x => logo_img.color = x, 0, 0.8f);
            DOTween.ToAlpha(() => desc_txt.color, x => desc_txt.color = x, 0, 0.8f);
            yield return new WaitForSeconds(1.5f);
            CloseSelf();
            UIManager.Instance.OpenPanel<StartPanel>();
        }
    }
}