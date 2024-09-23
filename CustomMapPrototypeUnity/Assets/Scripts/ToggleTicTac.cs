using UnityEngine;

public class ToggleTicTac : MonoBehaviour {
    public MeshRenderer cube;
    public Material enabledMat;
    public Material disabledMat;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
    private const string saveName = "TicTac";

    private bool near = false;
    private bool hasInit = false;
    private bool on = false;

    private GameObject climber;

    private void Init() {
        climber = GameObject.Find("Climber");
        if (climber == null)
            return;

        on = PersistentSaveObject.upgradesObtained.Contains(saveName);
        SetMaterial();

        hasInit = true;
    }

    private void Update() {
        if (!hasInit)
            Init();

        if (Input.GetButtonDown("Interact") && near) {
            on = !on;

            climber.GetComponent<TicTac2>().enabled = on;

            if (on)
                PersistentSaveObject.upgradesObtained.Add(saveName);
            else
                PersistentSaveObject.upgradesObtained.Remove(saveName);

            SetMaterial();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Climber")
            near = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "Climber")
            near = false;
    }

    private void SetMaterial() {
        cube.material = on ? enabledMat : disabledMat;
    }
#endif
}
