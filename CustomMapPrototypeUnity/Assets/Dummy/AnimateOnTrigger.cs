using UnityEngine;

public class AnimateOnTrigger : MonoBehaviour {

    public Animator an;
    public AudioSource aso;
    public string anim;
    public bool makeChild;
    public bool pressInteract;
    public bool activated;
    public bool skipSoundTriggered;
    public bool pressAnyKey;
    public bool allowFreefall;
    public bool holdToSkipOnly;
    public float pressTime;
    public float holdTime = 2f;
    public bool resetTrigger;
    public float resetTime = 0.15f;
    public GameObject spinner;
    public GameObject skipMessage;

}
