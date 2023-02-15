using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Widget
{
    public class TabBar : WidgetBase
    {
        private List<TabWidget> _widgets;
        public GameObject cell;

        public void SetData(List<string> tabName, Action<int> clickHandler)
        {
            for (int i = 0; i < tabName.Count; i++)
            {
                var obj = Instantiate(cell, transform);
                var widget = obj.GetComponent<TabWidget>();
                widget.SetText(tabName[i]);
                widget.SetClickHandler(i, clickHandler);
                widget.SetToggleGroup(GetComponent<ToggleGroup>());
            }
        }
    }
}