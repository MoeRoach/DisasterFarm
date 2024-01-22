// File create date:2021/4/4
using RoachLite.Basic;
using UnityEngine;
using UnityEngine.UI;
// Created By Yu.Liu
namespace RoachLite.UIComponent {
    /// <summary>
    /// 竖直动态列表组件
    /// </summary>
    public class DynamicVerticalListView : BaseDynamicListView {

        public float spacing = 0f;

        protected float itemSize; // 子项尺寸，用于计算当前索引

        protected override void InitializeContainer() {
            var posRef = new Vector2(0, 1);
            contentRoot.anchorMin = posRef;
            contentRoot.anchorMax = Vector2.one;
            contentRoot.pivot = posRef;
            // 子项缓存挂载点
            var cacheRootObj = new GameObject("ObjectCacheRoot");
            cacheRoot = cacheRootObj.transform;
            cacheRoot.SetParent(contentRoot.parent);
            cacheRoot.localPosition = Vector3.zero;
            cacheRoot.localScale = Vector3.one;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            var grp = contentRoot.gameObject.AddComponent<VerticalLayoutGroup>();
            grp.padding = paddingOffset;
            grp.spacing = spacing;
            grp.childForceExpandWidth = true;
            grp.childControlHeight = true;
            grp.childControlWidth = true;
            var fitter = contentRoot.gameObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        protected override int GetCurrentPosition() {
            var rowHeight = itemSize + spacing;
            if (itemSize > 0f) {
                return (int)((Mathf.Abs(contentRoot.localPosition.y) - paddingOffset.top) / rowHeight);
            }
            return 0;
        }
    }
}
