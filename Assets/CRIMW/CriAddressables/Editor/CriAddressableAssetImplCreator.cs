/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ADDRESSABLES_INTEGRATION
 * @{
 */

#if CRI_USE_ADDRESSABLES

using UnityEngine;
using UnityEditor.AddressableAssets;
using UnityEditor;
using System.IO;
using System.Linq;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	[CriDisplayName("Addressables (Remote)")]
	public class CriAddressableAssetImplCreator : ICriAssetImplCreator
	{
		public string Description =>
@"This DeployType is deprecateed. Please use ""Addressables"" instead.
Available for assets that are delivered via the Addressables system.
When the assets are loaded, their respective data files are downloaded from a remote location.";

		public ICriAssetImpl CreateAssetImpl(AssetImportContext ctx)
		{
			if(AddressableAssetSettingsDefaultObject.Settings == null)
				throw new System.Exception($"[CRIWARE] AddressableAssetSettingsDefaultObject.Settings is null.\nCreate Addresasbles Settings and reimport the CRI Asset ({ctx.assetPath})");
			var anchor = CriRemoteAddressableGroupGenerator.CreateAnchor(ctx.assetPath);
			var fileName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(anchor.anchor)).ToLowerInvariant();
			var impl = new CriAddressableAssetImpl(fileName, anchor.anchor, AssetDatabase.AssetPathToGUID(ctx.assetPath), anchor.bundleName);
			return impl;
		}
	}

#if ADDRESSABLES_1_15_1_OR_NEWER
	[CriDisplayName("Addressables")]
	public class CriCustomAddressableAssetImplCreator : ICriAssetImplCreator
	{
		[SerializeField]
		CriAddressablesPathPair customPathPair = null;

		public string Description =>
@"Available for assets that are delivered via the Addressables system.
When the assets are loaded, their respective data files are provided from the loadpath.";

		public ICriAssetImpl CreateAssetImpl(AssetImportContext ctx)
		{
			if (AddressableAssetSettingsDefaultObject.Settings == null)
				throw new System.Exception($"[CRIWARE] AddressableAssetSettingsDefaultObject.Settings is null.\nCreate Addresasbles Settings and reimport the CRI Asset ({ctx.assetPath})");
			if (customPathPair == null)
				customPathPair = CriAddressablesSetting.Instance.Remote;
			ctx.DependsOnSourceAsset(AssetDatabase.GetAssetPath(customPathPair));
			var anchor = CriAddressableGroupGenerator.CreateAnchor(ctx.assetPath, customPathPair.CreateGroup());
			var fileName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(anchor.anchor)).ToLowerInvariant();
			var impl = new CriAddressableAssetImpl(fileName, anchor.anchor, AssetDatabase.AssetPathToGUID(ctx.assetPath), anchor.bundleName);
			return impl;
		}
	}
#endif

	[CustomPropertyDrawer(typeof(CriCustomAddressableAssetImplCreator))]
	public class CriAddressableAssetImplCreatorDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// make setting instance exist.
			var remote = CriAddressablesSetting.Instance.Remote;
			var local = CriAddressablesSetting.Instance.Local;

			var pathPairProp = property.FindPropertyRelative("customPathPair");
			var groups = AssetDatabase.FindAssets($"t:{nameof(CriAddressablesPathPair)}")
				.SelectMany(guid => AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(guid)))
				.Where(obj => obj is CriAddressablesPathPair)
				.Where(obj => (CriAddressablesPathPair)obj)
				.ToList();
			var currentIndex = groups.IndexOf(pathPairProp.objectReferenceValue as CriAddressablesPathPair);
			var newIndex = EditorGUI.Popup(position, "Group", currentIndex, groups.Select(g => g.name).ToArray());
			if (currentIndex != newIndex)
			{
				pathPairProp.objectReferenceValue = groups[newIndex];
			}
		}
	}
}

#endif

/** @} */
