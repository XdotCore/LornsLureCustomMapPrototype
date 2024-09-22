using MelonLoader;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(CustomMapPrototype.Mod), "Custom Map Prototype", "1.0.0", "X.Core")]
[assembly: MelonGame("Rubeki Games", "LornsLure")]

namespace CustomMapPrototype {
    public class Mod : MelonMod {
        static MelonLogger.Instance Logger { get; set; }

        public override void OnInitializeMelon() {
            Logger = LoggerInstance;
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName) {
            switch (sceneName) {
                case "Menu":
                    AddModButton();
                    break;
            }
        }

        private void AddModButton() {
            // prereqs
            GameObject quitBtn = ZUIManager.Instance.AllMenus[0].AnimatedElements.Last().gameObject;

            // creation
            GameObject modBtnGameObject = Object.Instantiate(quitBtn, quitBtn.transform.parent);
            modBtnGameObject.name = "Mod Button";
            RectTransform modBtnTransform = modBtnGameObject.transform as RectTransform;
            modBtnTransform.anchorMin -= new Vector2(0, .08f);
            modBtnTransform.anchorMax -= new Vector2(0, .08f);

            Button modBtn = modBtnGameObject.GetComponent<Button>();
            modBtn.onClick = new Button.ButtonClickedEvent();
            modBtn.onClick.AddListener(OnModButtonClicked);

            Text modBtnText = modBtnGameObject.GetComponentInChildren<Text>();
            modBtnText.text = "Mods";

            // localization isn't needed for this demo
            Object.Destroy(modBtnGameObject.GetComponentInChildren<LocalizeStringEvent>());
        }

        private void OnModButtonClicked() {

        }
    }
}
