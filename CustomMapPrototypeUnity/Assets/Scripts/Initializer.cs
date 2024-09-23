using UnityEngine;
using System.Reflection;


#if !(UNITY_EDITOR || UNITY_STANDALONE)
using CustomMapPrototype;
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
        GameObject checkpoints = levelMechanics.Find("Checkpoints").gameObject;
        GameObject secretCrystals = levelMechanics.Find("SecretCrystalPlaceholders").gameObject;
        GameObject wayPoints = levelMechanics.Find("WayPointPlaceholders").gameObject;

        AsyncOperation loadSceneOp = SceneManager.LoadSceneAsync(SceneToCopy, LoadSceneMode.Additive);
        loadSceneOp.completed += op => {
            Scene copyScene = SceneManager.GetSceneByName(SceneToCopy);
            Scene customMap1 = SceneManager.GetSceneByName(ThisScene);
            SceneManager.SetActiveScene(customMap1);
            GameObject[] roots = copyScene.GetRootGameObjects();

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
                                case "Checkpoints": {
                                    foreach (Transform oldCheckpoint in child.transform)
                                        Destroy(oldCheckpoint.gameObject);

                                    while (checkpoints.transform.childCount > 0) {
                                        Transform newCheckpoint = checkpoints.transform.GetChild(0);
                                        newCheckpoint.SetParent(child.transform, true);
                                    }

                                    Destroy(checkpoints);
                                } break;
                                case "SecretCrystals": {
                                    GameObject templateCrystal = child.transform.GetChild(0).gameObject;
                                    templateCrystal.name = "Template Crystal";

                                    bool skipFirst = true;
                                    foreach (Transform oldCrystal in child.transform) {
                                        if (skipFirst) {
                                            skipFirst = false;
                                            continue;
                                        }
                                        Destroy(oldCrystal.gameObject);
                                    }

                                    foreach (Transform crystal in secretCrystals.transform) {
                                        Transform newCrystal = Instantiate(templateCrystal, child.transform).transform;
                                        newCrystal.position = crystal.position;
                                        newCrystal.gameObject.name = crystal.gameObject.name;
                                        newCrystal.gameObject.SetActive(crystal.gameObject.activeSelf);
                                    }

                                    Destroy(templateCrystal);
                                    Destroy(secretCrystals);
                                } break;
                                case "WayPointHints": {
                                    GameObject templateWayPoint = child.transform.GetChild(0).gameObject;
                                    templateWayPoint.name = "Template WayPoint";

                                    bool skipFirst = true;
                                    foreach (Transform oldWayPoint in child.transform) {
                                        if (skipFirst) {
                                            skipFirst = false;
                                            continue;
                                        }
                                        Destroy(oldWayPoint.gameObject);
                                    }

                                    foreach (Transform wayPoint in wayPoints.transform) {
                                        Transform newWayPoint = Instantiate(templateWayPoint, child.transform).transform;
                                        newWayPoint.position = wayPoint.position;
                                        newWayPoint.gameObject.name = wayPoint.gameObject.name;
                                        newWayPoint.gameObject.SetActive(wayPoint.gameObject.activeSelf);
                                    }

                                    Destroy(templateWayPoint);
                                    Destroy(wayPoints);
                                } break;
                                case "NG+ Enable":
                                case "EndCube":
                                case "Deprecated Checkpoints":
                                    Destroy(child);
                                    break;
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
