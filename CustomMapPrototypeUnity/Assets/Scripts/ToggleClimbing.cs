﻿using UnityEngine;

public class ToggleClimbing : MonoBehaviour {
    public MeshRenderer cube;
    public Material enabledMat;
    public Material disabledMat;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
    private const string saveName = "ClimbingAbility";

    private bool near = false;
    private bool hasInit = false;
    private bool on = false;

    private GameObject climber;
    private GameObject leftPickWalking;
    private GameObject rightPickWalking;

    private void Init() {
        climber = GameObject.Find("Climber");
        if (climber == null)
            return;

        leftPickWalking = climber.GetComponent<HideUIToggle>().leftPickWalking.gameObject;
        rightPickWalking = climber.GetComponent<HideUIToggle>().rightPickWalking.gameObject;

        on = PersistentSaveObject.upgradesObtained.Contains(saveName);
        SetMaterial();

        hasInit = true;
    }

    private void Update() {
        if (!hasInit)
            Init();

        if (Input.GetButtonDown("Interact") && near) {
            on = !on;

            climber.GetComponent<ClimbingAbilityV2>().abilityActive = on;

            leftPickWalking.SetActive(on);
            rightPickWalking.SetActive(on);

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
