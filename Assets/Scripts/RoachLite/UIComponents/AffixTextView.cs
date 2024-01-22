using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RoachLite.UIComponent {
    /// <summary>
    /// 附带前后缀的Text辅助组件
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class AffixTextView : MonoBehaviour {

        public bool hasPrefix;
        public string prefixText = "";
        public bool hasSuffix;
        public string suffixText = "";

        private Text _contentText;

        private void Start() {
            _contentText = GetComponent<Text>();
        }

        public void SetText(string content) {
            StartCoroutine(UpdateText(content));
        }

        private IEnumerator UpdateText(string str) {
            yield return null;
            var result = "";
            if (hasPrefix) {
                result += prefixText;
            }
            result += str;
            if (hasSuffix) {
                result += suffixText;
            }
            _contentText.text = result;
        }
    }
}
