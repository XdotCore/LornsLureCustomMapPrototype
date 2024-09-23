using HarmonyLib;
using MelonLoader;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[assembly: MelonInfo(typeof(CustomMapPrototype.Mod), "Custom Map Prototype", "1.0.0", "X.Core")]
[assembly: MelonGame("Rubeki Games", "LornsLure")]

namespace CustomMapPrototype {
    [HarmonyPatch]
    public class Mod : MelonMod {
        public static MelonLogger.Instance Logger { get; private set; }

        private Menu ModdedLevelSelect { get; set; }
        private const string CustomMap1Id = "CustomMap1";
        private AssetBundle CustomMap1Bundle;

        public override void OnInitializeMelon() {
            Logger = LoggerInstance;

            using (Stream sceneBundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomMapPrototype.Unity.AssetBundles.custommap1")) {
                AssetBundle.LoadFromStream(sceneBundleStream);
            }

            using (Stream assetBundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomMapPrototype.Unity.AssetBundles.custommap1assets")) {
                CustomMap1Bundle = AssetBundle.LoadFromStream(assetBundleStream);
            }
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName) {
            switch (sceneName) {
                case "Menu":
                    AddModButton();
                    AddCustomLevelSelect();
                    break;
            }
        }

        [HarmonyPatch(typeof(CollectibleMeta), "Start")]
        [HarmonyPostfix]
        private static void InitCollectibles() {
            CollectibleMeta.maxCrystals[CustomMap1Id] = 1;
            CollectibleMeta.maxScans[CustomMap1Id] = 0;
            CollectibleMeta.noCrystalLevels.Add(CustomMap1Id);
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
            modBtn.onClick.AddListener(() => ZUIManager.Instance.OpenMenu(ModdedLevelSelect));

            Text modBtnText = modBtnGameObject.GetComponentInChildren<Text>();
            modBtnText.text = "Mods";

            // localization isn't needed for this demo
            Object.Destroy(modBtnGameObject.GetComponentInChildren<LocalizeStringEvent>());
        }

        private void AddCustomLevelSelect() {
            // prereqs
            GameObject lsm2 = ZUIManager.Instance.AllMenus[2].gameObject;

            // creation
            GameObject lsmModded = Object.Instantiate(lsm2, lsm2.transform.parent);
            lsmModded.name = "Level Select Menu Modded";
            lsmModded.transform.SetSiblingIndex(lsm2.transform.GetSiblingIndex() + 1);

            GameObject records = null;
            TextMeshProUGUI levelTitle = null;
            LoadLevelPerformanceStats stats = null;

            for (int i = 0; i < lsmModded.transform.childCount; i++) {
                GameObject child = lsmModded.transform.GetChild(i).gameObject;
                switch (child.name) {
                    case "Title": {
                        GameObject title = child;
                        title.GetComponent<TextMeshProUGUI>().text = "CUSTOM MAPS";

                        Object.Destroy(title.GetComponent<LocalizeStringEvent>());
                        Object.Destroy(title.GetComponent<StringUpdateListenerConfiguerer>());
                    }
                    break;
                    case "Records Panel": {
                        records = child;
                        levelTitle = records.GetComponentInChildren<TextMeshProUGUI>();
                        levelTitle.text = "Custom Map 1";
                        stats = records.GetComponent<LoadLevelPerformanceStats>();
                    }
                    break;
                    case "TheWallV3": {
                        GameObject firstMap = child;
                        firstMap.name = CustomMap1Id;

                        Button btn = firstMap.GetComponent<Button>();
                        btn.interactable = true;
                        btn.onClick = new Button.ButtonClickedEvent();
                        btn.onClick.AddListener(() => {
                            ZUIManager.Instance.OpenPopup("New Game or Continue Popup");
                            stats.lsm.OnClickLevel(CustomMap1Id);
                        });

                        Image img = firstMap.GetComponent<Image>();
                        img.color = Color.white;

                        firstMap.transform.Find("Crystal Locked").gameObject.SetActive(false);
                        firstMap.transform.Find("Crystal Unlocked").gameObject.SetActive(false);

                        EventTrigger hoverTrigger = firstMap.GetComponents<EventTrigger>().Last();
                        hoverTrigger.triggers.Clear();

                        EventTrigger.Entry enterEntry = new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter };
                        enterEntry.callback.AddListener(_ => btn.Select());
                        hoverTrigger.triggers.Add(enterEntry);

                        EventTrigger.Entry selectEntry = new EventTrigger.Entry() { eventID = EventTriggerType.Select };
                        selectEntry.callback.AddListener(_ => {
                            levelTitle.text = "Custom Map 1";
                            stats.DisplayLevelStats(CustomMap1Id);
                        });
                        hoverTrigger.triggers.Add(selectEntry);

                        GameObject graphic = firstMap.transform.GetChild(0).gameObject;
                        graphic.name = $"{CustomMap1Id}Graphic";

                        graphic.GetComponent<Image>().sprite = CustomMap1Bundle.LoadAsset<Sprite>(CustomMap1Id);
                    }
                    break;
                    case "Levels Panel":
                    case "Back Button":
                        break;
                    default:
                        child.SetActive(false);
                        break;
                }
            }

            // add to ZUI
            ModdedLevelSelect = lsmModded.GetComponent<Menu>();
            ZUIManager.Instance.AllMenus.Add(ModdedLevelSelect);
        }
    }
}
