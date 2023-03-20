/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CriWare.Assets
{
    public abstract class CriAssetSettingsBase<T> : CriEditorSettingBase where T : CriAssetSettingsBase<T>
    {
        readonly static string SettingsDirPath = "Assets/CriData/Settings";

        static T _instance = null;
        public static T Instance
		{
			get
			{
                if(_instance == null)
				{
                    var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                    if (guids.Length >= 2)
                        Debug.LogWarning("[CRIWARE] There is multiple instances of " + typeof(T).Name);
                    if (guids.Length <= 0)
					{
						if (!System.IO.Directory.Exists(SettingsDirPath))
							System.IO.Directory.CreateDirectory(SettingsDirPath);
                        _instance = CreateInstance<T>();
                        AssetDatabase.CreateAsset(_instance, System.IO.Path.Combine(SettingsDirPath, typeof(T).Name + ".asset"));
					}
					else
					{
                        _instance = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
					}
				}
                return _instance;
			}
        }
    }
}

/** @} */
