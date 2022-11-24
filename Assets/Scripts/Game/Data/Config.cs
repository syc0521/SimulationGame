﻿using System.Collections.Generic;
using Game.Core;
using Game.Data.ScriptableObject;
using UnityEngine;

namespace Game.Data
{
    public class Config : Singleton<Config>
    {
        [SerializeField] private PanelDef panelDef;
        [SerializeField] private Transform uiRoot;
        [SerializeField] private BuildingCollection buildings;

        public GameObject GetPanel(PanelEnum type) => panelDef.panels[type];

        public Transform GetUIRoot() => uiRoot;

        public List<GameObject> GetBuildings() => buildings.buildings;
    }
}