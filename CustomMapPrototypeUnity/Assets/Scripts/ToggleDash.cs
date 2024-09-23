using UnityEngine;

public class ToggleDash : MonoBehaviour {
    public MeshRenderer cube;
    public Material enabledMat;
    public Material disabledMat;

#if !(UNITY_EDITOR || UNITY_STANDALONE) 
    private bool near = false;
    private bool hasInit = false;
    private bool on = false;

    private GameObject climber;

    private void Init() {
        climber = GameObject.Find("Climber");
        if (climber == null)
            return;

        hasInit = true;
    }

    private void Update() {
        if (!hasInit)
            Init();

        if (Input.GetButtonDown("Interact") && near) {
            on = !on;

            climber.GetComponent<Dash>().enabled = on;
            climber.GetComponent<Dash>().abilityActive = on;

            if (on)
                PersistentSaveObject.upgradesObtained.Add("Dash");
            else
                PersistentSaveObject.upgradesObtained.Remove("Dash");

            cube.material = on ? enabledMat : disabledMat;
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
#endif
}
