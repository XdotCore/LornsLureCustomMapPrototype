using UnityEngine;

public class ToggleGrapple : MonoBehaviour {
    public MeshRenderer cube;
    public Material enabledMat;
    public Material disabledMat;

#if !(UNITY_EDITOR || UNITY_STANDALONE) 
    private bool near = false;
    private bool hasInit = false;
    private bool on = false;

    private GameObject climber;
    private GameObject grappleAttachmentRunning;

    private void Init() {
        climber = GameObject.Find("Climber");
        if (climber == null)
            return;

        grappleAttachmentRunning = climber.GetComponent<HideUIToggle>().leftPickWalking.transform.GetChild(0).gameObject;

        hasInit = true;
    }

    private void Update() {
        if (!hasInit)
            Init();

        if (Input.GetButtonDown("Interact") && near) {
            on = !on;

            climber.GetComponent<GrappleHook>().abilityActive = true;
            climber.GetComponent<GrappleHook>().StopGrapple();

            grappleAttachmentRunning.SetActive(on);

            if (on)
                PersistentSaveObject.upgradesObtained.Add("Grapple");
            else
                PersistentSaveObject.upgradesObtained.Remove("Grapple");

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
