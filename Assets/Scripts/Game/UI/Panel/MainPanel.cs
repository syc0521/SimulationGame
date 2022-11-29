using System;
using Game.Core;
using Game.Data.Event;
using TMPro;

namespace Game.UI.Panel
{
    public class MainPanel : UIPanel
    {
        public TextMeshProUGUI text;
        
        public override void OnCreated()
        {
            EventCenter.AddListener<DataChangedEvent>(RefreshUI);
        }

        public void RefreshUI(DataChangedEvent evt)
        {
            text.text = evt.gameData.people.ToString();
        }
    }
}