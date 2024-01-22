using System.Collections;
using System.Collections.Generic;
using RoachLite.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RoachLite.UIComponent {
    /// <summary>
    /// 拖曳功能响应区
    /// </summary>
    public class DragableArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        protected IDragable dragable;

        public virtual void SetupDragable(IDragable id) {
            dragable = id;
        }
        
        public void OnBeginDrag(PointerEventData eventData) {
            dragable?.OnDragBegin(eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            dragable?.OnDragUpdate(eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            dragable?.OnDragFinish(eventData);
        }
    }
}