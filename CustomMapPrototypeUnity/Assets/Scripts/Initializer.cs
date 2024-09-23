using UnityEngine;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
using UnityEngine.SceneManagement;
#endif

public class Initializer : MonoBehaviour {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
    private const string SceneToCopy = "TheWallV3";
    private const string ThisScene = "CustomMap1";

    private void Start() {
        Transform climberStart = transform.Find("ClimberStart");
        Transform cameraRotation = climberStart.Find("CameraRotation");

        Transform levelMechanics = transform.Find("LevelMechanics");
        Transform endingTrigger = levelMechanics.Find("EndingTrigger");

        SceneManager.LoadSceneAsync(SceneToCopy, LoadSceneMode.Additive).completed += aop => {
            Scene c0 = SceneManager.GetSceneByName(SceneToCopy);
            Scene customMap1 = SceneManager.GetSceneByName(ThisScene);
            SceneManager.SetActiveScene(customMap1);
            GameObject[] roots = c0.GetRootGameObjects();

            foreach (GameObject root in roots) {
                switch (root.name) {
                    case "ClimbingSystemPrefab": {
                        for (int i = 0; i < root.transform.childCount; i++) {
                            GameObject child = root.transform.GetChild(i).gameObject;
                            switch (child.name) {
                                case "Climber": {
                                    child.transform.position = climberStart.position;
                                    child.transform.rotation = climberStart.rotation;
                                    child.GetComponent<UIToggleControls>().walkingCam.transform.localRotation = cameraRotation.localRotation;
                                } break;
                            }
                        }
                    } break;
                    case "Map": {
                        for (int i = 0; i < root.transform.childCount; i++) {
                            GameObject child = root.transform.GetChild(i).gameObject;
                            Destroy(child);
                        }
                    } break;
                    case "Level Mechanics": {
                        for (int i = 0; i < root.transform.childCount; i++) {
                            GameObject child = root.transform.GetChild(i).gameObject;
                            switch (child.name) {
                                case "NG+ Enable": {
                                    Destroy(child);
                                } break;
                                case "EndingTrigger": {
                                    Animator blackoutAn = child.GetComponent<AnimateOnTrigger>().an;
                                    endingTrigger.GetComponent<AnimateOnTrigger>().an = blackoutAn;

                                    StatsScreenEnding sse = blackoutAn.GetComponent<StatsScreenEnding>();
                                    sse.cutsceneNext = false;
                                    sse.sceneAfterNext = null;
                                    sse.nextScene = "Menu";
                                    sse.CRNextScene = null;
                                    sse.runModeNextScene = null;

                                    Destroy(child);
                                } break;
                                case "EndCube": {
                                    Destroy(child);
                                } break;
                                case "Deprecated Checkpoints": {
                                    Destroy(child);
                                } break;
                                case "Checkpoints": {
                                    foreach (Transform checkpoint in child.transform)
                                        Destroy(checkpoint.gameObject);
                                } break;
                                case "AllCutscenes": {
                                    foreach (Transform cutscene in child.transform)
                                        Destroy(cutscene);
                                } break;
                                case "SecretCrystals": {
                                    foreach (Transform crystal in child.transform)
                                        Destroy(crystal.gameObject);
                                } break;
                            }
                        }

                        // copy over custom level mechanics
                        for (int i = 0; i < levelMechanics.childCount; i++) {
                            Transform child = levelMechanics.GetChild(i);
                            child.parent = root.transform;
                        }
                    } break;
                    case "Gameplay": {
                        for (int i = 0; i < root.transform.childCount; i++) {
                            GameObject child = root.transform.GetChild(i).gameObject;
                            switch (child.name) {
                                case "NeedThisToDisableTerminal":
                                    break;
                                default:
                                    Destroy(child);
                                    break;
                            }
                        }
                    } break;
                    case "CancelNarrationSystem":
                    case "NarrationSystem":
                    case "Global Scene Objects":
                    case "ScanningSystem":
                    case "CheckpointStore":
                    case "EventSystem":
                    case "ScreenshotCompanion":
                        break;
                    default:
                        // default don't move to custom scene
                        continue;
                }

                SceneManager.MoveGameObjectToScene(root, customMap1);
            }

            SceneManager.UnloadSceneAsync(SceneToCopy);
        };
    }
#endif
}
