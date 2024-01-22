using UnityEngine;
using UnityEngine.UI;
// Created By Yu.Liu
namespace RoachLite.LocalizationSystem {
    [RequireComponent(typeof(Text))]
    public class LocalizationText : MonoBehaviour {

        public string textIdentifier = "Default";
        protected Text contentText;

        public string text {
            get => contentText.text;
            set => contentText.text = value;
        }

        private void Awake() {
            contentText = GetComponent<Text>();
        }

        private void Start() {
            LocalizationManager.Instance.RegisterText(this);
        }

        private void OnDestroy() {
            LocalizationManager.Instance.UnregisterText(this);
        }
    }
}
