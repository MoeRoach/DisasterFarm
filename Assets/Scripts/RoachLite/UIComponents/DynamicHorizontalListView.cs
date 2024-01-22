// File create date:2021/4/4
using RoachLite.Basic;
using UnityEngine;
using UnityEngine.UI;
// Created By Yu.Liu
namespace RoachLite.UIComponent {
    /// <summary>
    /// 水平动态列表组件
    /// </summary>
    public class DynamicHorizontalListView : BaseDynamicListView {

        public float spacing = 0f;

        protected float itemSize; // 子项尺寸，用于计算当前索引

        protected override void InitializeContainer() {
            var posRef = new Vector2(0, 1);
            contentRoot.anchorMin = Vector2.zero;
            contentRoot.anchorMax = posRef;
            contentRoot.pivot = posRef;
            // 子项缓存挂载点
            var cacheRootObj = new GameObject("ObjectCacheRoot");
            cacheRoot = cacheRootObj.transform;
            cacheRoot.SetParent(contentRoot.parent);
            cacheRoot.localPosition = Vector3.zero;
            cacheRoot.localScale = Vector3.one;
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
            var grp = contentRoot.gameObject.AddComponent<HorizontalLayoutGroup>();
            grp.padding = paddingOffset;
            grp.spacing = spacing;
            grp.childForceExpandHeight = true;
            grp.childControlHeight = true;
            grp.childControlWidth = true;
            var fitter = contentRoot.gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        protected override int GetCurrentPosition() {
            var rowHeight = itemSize + spacing;
            if (itemSize > 0f) {
                return (int)((Mathf.Abs(contentRoot.localPosition.x) - paddingOffset.left) / rowHeight);
            }
            return 0;
        }
    }
}
