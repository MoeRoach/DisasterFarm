// File create date:2022/7/4
using RoachLite.UIComponent;
using UnityEngine;
using UnityEngine.UI;

// Created By Yu.Liu
namespace RoachLite.Basic {
	/// <summary>
	/// 基础面板组件抽象基类
	/// </summary>
	[RequireComponent(typeof(UiAnimator))]
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(GraphicRaycaster))]
	public abstract class BasePanel : BaseWidget {

		public bool startShow; // 是否初始显示
		public bool hideRootObject; // 物体本身是否跟随激活状态
		public bool hideRaycast; // 射线检测是否跟随激活状态

		protected Canvas canvas;
		protected GraphicRaycaster raycaster;
		protected UiAnimator uiAnimator;

		public bool IsAnimate => uiAnimator != null && uiAnimator.IsAnimate;
		public bool IsVisible => uiAnimator != null && uiAnimator.IsVisible;

		protected override void PreLoad() {
			base.PreLoad();
			canvas = GetComponent<Canvas>();
			raycaster = GetComponent<GraphicRaycaster>();
			uiAnimator = GetComponent<UiAnimator>();
			uiAnimator.BeforeShow += OnBeforeShow;
			uiAnimator.OnShown += OnAfterShow;
			uiAnimator.BeforeHide += OnBeforeHide;
			uiAnimator.OnHiden += OnAfterHide;
		}

		protected override void OnLazyLoad() {
			base.OnLazyLoad();
			if (!startShow) {
				Hide(true);
			}
		}
		
		public virtual void SetupSortingLayer(string ln) {
			canvas.sortingLayerName = ln;
		}

		public virtual void SetupSortingOrder(int order) {
			canvas.sortingOrder = order;
		}
		
		public virtual void Show(bool instantFlag = false) {
			if (uiAnimator.IsVisible || uiAnimator.IsAnimate) return;
			if (hideRootObject) {
				gameObject.SetActive(true);
			} else {
				if (!uiAnimator.hasShowAnimation && hideRaycast) {
					canvasGroup.interactable = true;
					canvasGroup.blocksRaycasts = true;
				}
			}

			if (instantFlag) {
				uiAnimator.SetupVisible(true);
				if (!hideRaycast) return;
				canvasGroup.interactable = true;
				canvasGroup.blocksRaycasts = true;
			} else {
				uiAnimator.RequestShowAnimation();
			}
		}

		public virtual void Hide(bool instantFlag = false) {
			if (!uiAnimator.IsVisible || uiAnimator.IsAnimate) return;
			if (instantFlag) {
				uiAnimator.SetupVisible(false);
				if (!hideRaycast) return;
				canvasGroup.interactable = false;
				canvasGroup.blocksRaycasts = false;
			} else {
				uiAnimator.RequestHideAnimation();
			}
		}

		protected virtual void OnBeforeShow() {
			if (!hideRaycast) return;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		}

		protected virtual void OnAfterShow() {
			NotifyUpdate();
		}
		
		protected virtual void OnBeforeHide() { }

		protected virtual void OnAfterHide() {
			if (hideRootObject) {
				gameObject.SetActive(false);
				return;
			}
			if (!hideRaycast) return;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}
	}
}