using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

namespace CriWare.Assets
{
	partial class CriAtomAcbAssetImporter
	{
		[StructLayout(LayoutKind.Auto)]
		partial struct AcbAssetInfo
		{
			public uint decryptionKeyForEditor;
		}
	}

	internal class CriAtomAssetsEditorDecryption
	{
		[InitializeOnLoadMethod]
		static void RegisterLoadedEvent()
		{
			CriAtomAssetsPreviewPlayer.Instance.OnLoaded += (path, handle) => {
				var importer = AssetImporter.GetAtPath(path) as CriAtomAcbAssetImporter;
				handle.Decrypt(importer.assetInfo.decryptionKeyForEditor, 0);
			};
		}
	}
}
