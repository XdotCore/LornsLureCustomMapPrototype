using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour {

    private void Start() {
        SceneManager.LoadSceneAsync("TheWallV3", LoadSceneMode.Additive).completed += aop => {
            Scene c0 = SceneManager.GetSceneByName("TheWallV3");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("CustomMap1"));
            GameObject[] roots = c0.GetRootGameObjects();

            foreach (GameObject root in roots) {
                switch (root.name) {
                    case "ClimbingSystemPrefab": {
                        for (int i = 0; i < root.transform.childCount; i++) {
                            GameObject child = root.transform.GetChild(i).gameObject;
                            switch (child.name) {
                                case "Climber": {
                                    child.transform.position = transform.position;
                                } break;
                            }
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
                        Destroy(root);
                        break;
                }
            }
        };
    }
}
