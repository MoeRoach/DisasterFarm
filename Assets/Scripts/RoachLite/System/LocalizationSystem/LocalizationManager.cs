// File create date:2020/10/27
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using RoachLite.Basic;

// Created By Yu.Liu
namespace RoachLite.LocalizationSystem {
    /// <summary>
    /// 多语言管理器
    /// </summary>
    public class LocalizationManager : MonoSingleton<LocalizationManager> {

        public static int UPDATE_COUNT_PER_FRAME = 120;
        public const string LOC_TAG_DEFAULT = "Loc_CN";

        protected string activeLocalizationTag = LOC_TAG_DEFAULT;
        protected Dictionary<string, HashSet<LocalizationText>> manageTexts = new Dictionary<string, HashSet<LocalizationText>>();
        protected Dictionary<string, LocalizationData> manageDatas = new Dictionary<string, LocalizationData>();
        protected Dictionary<string, string> localizationBindings = new Dictionary<string, string>();
        protected bool isUpdating;

        public IEnumerable<string> LocalizationTags => manageDatas.Keys;

        public virtual void SetupLocalization(string loc) {
            activeLocalizationTag = loc;
        }

        public virtual void LoadLocalizationData(string jsonContent) {
            manageDatas = JsonConvert.DeserializeObject<Dictionary<string, LocalizationData>>(jsonContent);
        }

        public virtual void PutLocalizationData(string loc, LocalizationData data) {
            manageDatas[loc] = data;
        }

        public virtual LocalizationData GetLocalizationData(string loc) {
            return manageDatas.TryGetElement(loc);
        }

        public virtual void DeleteLocalizationData(string loc) {
            manageDatas.Remove(loc);
        }

        public virtual void RegisterText(LocalizationText text) {
            if (!manageTexts.ContainsKey(text.textIdentifier)) {
                manageTexts[text.textIdentifier] = new HashSet<LocalizationText>();
            }
            manageTexts[text.textIdentifier].Add(text);
        }

        public virtual void UnregisterText(LocalizationText text) {
            manageTexts.TryGetElement(text.textIdentifier)?.Remove(text);
        }

        public virtual void BindLocalizationText(string id, string tt) {
            localizationBindings[id] = tt;
        }

        public virtual void UnbindLocalizationText(string id) {
            localizationBindings.Remove(id);
        }

        public virtual void SetTextByTag(string id, string tt) {
            var textSet = manageTexts.TryGetElement(id);
            if (textSet == null) return;
            var data = manageDatas.TryGetElement(activeLocalizationTag);
            if (data == null) return;
            var content = data.locContent.TryGetElement(tt);
            foreach (var text in textSet) {
                text.text = content ?? tt;
            }
        }

        public virtual void SetText(string id, string content) {
            var textSet = manageTexts.TryGetElement(id);
            if (textSet == null) return;
            foreach (var text in textSet) {
                text.text = content;
            }
        }

        protected virtual IEnumerator UpdateLocalizationText() {
            var counter = 0;
            isUpdating = true;
            yield return null;
            foreach (var id in localizationBindings.Keys) {
                var textSet = manageTexts.TryGetElement(id);
                if (textSet == null) continue;
                var tt = localizationBindings[id];
                var data = manageDatas.TryGetElement(activeLocalizationTag);
                if (data == null) continue;
                var content = data.locContent.TryGetElement(tt);
                foreach (var text in textSet) {
                    text.text = content ?? tt;
                    counter++;
                    if (counter >= UPDATE_COUNT_PER_FRAME) {
                        yield return null;
                    }
                }
            }
            isUpdating = false;
        }

        public virtual void NotifyLocalizationUpdate() {
            if (!isUpdating) {
                StartCoroutine(UpdateLocalizationText());
            }
        }
    }

    public class LocalizationData {

        public string locTag;
        public Dictionary<string, string> locContent;

        public LocalizationData(string loc) {
            locTag = loc;
            locContent = new Dictionary<string, string>();
        }
    }
}
