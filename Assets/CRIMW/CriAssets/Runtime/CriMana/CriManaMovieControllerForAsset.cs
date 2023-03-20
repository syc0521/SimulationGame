/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_COMPONENT
 * @{
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriWare.Assets
{
	public interface ICriManaMovieMaterialTarget
	{
		Material Material { get; set; }
		bool IsActive { get; set; }
		bool uiRenderMode { get; }
	}

	[System.Serializable]
	public struct ManaMovieMaterialRendererTarget : ICriManaMovieMaterialTarget
	{
		[SerializeField]
		Renderer renderer;

		public ManaMovieMaterialRendererTarget(Renderer renderer) =>
			this.renderer = renderer;

		public Material Material { get => renderer.material; set => renderer.material = value; }
		public bool IsActive { get => renderer.enabled; set => renderer.enabled = value; }
		public bool uiRenderMode => false;
	}

	[System.Serializable]
	public struct ManaMovieMaterialGraphicTarget : ICriManaMovieMaterialTarget
	{
		[SerializeField]
		UnityEngine.UI.Graphic graphic;

		public ManaMovieMaterialGraphicTarget(UnityEngine.UI.Graphic graphic) =>
			this.graphic = graphic;

		public Material Material { get => graphic.material; set => graphic.material = value; }
		public bool IsActive { get => graphic.enabled; set => graphic.enabled = value; }
		public bool uiRenderMode => true;
	}

	/**
	 * <summary>Movie playback component for CRI Assets</summary>
	 * <remarks>
	 * <para header='Description'>This component is used to play USM assets in a scene.<br/>
	 * The movie is played back as a Renderer or Graphic material specified by this component.</para>
	 * </remarks>
	 */
	public class CriManaMovieControllerForAsset : CriManaMovieMaterialBase
	{
		#region Overrides

		protected override bool initializeWithAdvancedAudio => true;
		protected override bool initializeWithAmbisonics => true;

		protected override uint FilePathLength => (uint)(usmAsset?.FilePath?.Length ?? 0);
		protected override void SetDataToPlayer()
		{
			if (usmAsset != null)
				player.SetAsset(usmAsset);
			player.maxFrameDrop = (int)maxFrameDrop;
			player.uiRenderMode = Target.uiRenderMode;
		}

		#endregion

		[SerializeField]
		internal CriManaUsmAsset usmAsset = null;

		[SerializeReference]
		internal ICriManaMovieMaterialTarget target;

		#region Properties
		/**
		 * <summary>The target to which the MovieMaterial will be set.</summary>
		 * <remarks>
		 * <para header='Description'>ICriManaMovieMaterialTarget to which to set the MovieMaterial.<br/></para>
		 * </remarks>
		 */
		public ICriManaMovieMaterialTarget Target { get => target; set => target = value; }


		/**
		 * <summary>Whether to display the original Material when movie frames are not available.</summary>
		 * <remarks>
		 * <para header='Description'>Whether to display the original material when a movie frame is not available.<br/>
		 * true : display the original material when a movie frame is not available.<br/>
		 * false : disable the rendering of the target when a movie frame is not available.<br/></para>
		 * </remarks>
		 */
		public bool useOriginalMaterial;
		#endregion


		#region Internal Variables
		private Material originalMaterial;
		#endregion


		protected override void Awake()
		{
			base.Awake();
		}

		public override void CriInternalUpdate()
		{
			base.CriInternalUpdate();

			// If there is a target connected but current GameObject is not a Renderer,
			// we check target activation an then update movie material if active.
			if (renderMode == RenderMode.OnVisibility)
			{
				if (HaveRendererOwner == false && target != null && target.IsActive)
				{
					player.OnWillRenderObject(this);
				}
			}

			if (player?.atomEx3DsourceForAmbisonics != null)
			{
				if (transform.hasChanged)
				{
					player.atomEx3DsourceForAmbisonics.SetOrientation(transform.forward, transform.up);
					player.atomEx3DsourceForAmbisonics.Update();
					transform.hasChanged = false;
				}
			}
		}


		public override bool RenderTargetManualSetup()
		{
			if (target == null)
			{
				Debug.LogError("[CRIWARE] Missing render target for the Mana Controller component: Please add a renderer to the GameObject or specify the target manually.");
				return false;
			}
			originalMaterial = target.Material;
			if (!useOriginalMaterial)
			{
				target.IsActive = false;
			}
			return true;
		}


		public override void RenderTargetManualFinalize()
		{
			if (target != null)
			{
				target.Material = originalMaterial;
				if (!useOriginalMaterial)
				{
					target.IsActive = false;
				}
			}
			originalMaterial = null;
		}

		protected override void OnMaterialAvailableChanged()
		{
			if (target == null)
			{
				return;
			}

			if (isMaterialAvailable)
			{
				target.Material = material;
				target.IsActive = true;
			}
			else
			{
				target.Material = originalMaterial;
				if (!useOriginalMaterial)
				{
					target.IsActive = false;
				}
			}
		}

#if UNITY_EDITOR
		void Reset()
		{
			var renderer = GetComponent<Renderer>();
			if (renderer != null)
			{
				Target = new ManaMovieMaterialRendererTarget(renderer);
				return;
			}

			var graphic = GetComponent<UnityEngine.UI.Graphic>();
			if (graphic != null)
			{
				Target = new ManaMovieMaterialGraphicTarget(graphic);
				return;
			}
		}
#endif
	}
}

/** @} */
