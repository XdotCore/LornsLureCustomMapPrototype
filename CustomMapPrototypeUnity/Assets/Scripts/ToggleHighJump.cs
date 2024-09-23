using UnityEngine;

public class ToggleHighJump : MonoBehaviour {
    public MeshRenderer cube;
    public Material enabledMat;
    public Material disabledMat;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
    private const string saveName = "JumpForce";

    private bool near = false;
    private bool hasInit = false;
    private bool on = false;

    private GameObject climber;

    private float bfpscExtraJumpTimeDefault;
    private float bfpscExtraJumpPowerDefault;
    private float bfpscSpeedDefault;
    private float bfpscForwardSpeedDefault;
    private float bfpscBackwardSpeedDefault;
    private float bfpscStrafeSpeedDefault;
    private float bfpscMovementMaxLateralSpeedDefault;
    private float bfpscMovementMaxRiseSpeedDefault;
    private float jumpOffWallVerticalImpulseDefault;
    private float jumpOffWallHorizontalImpulseDefault;
    private float criticalVelocityDefault;
    private float jumpForceForwardDefault;
    private float jumpForceNormalDefault;
    private float jumpForceWallDefault;
    private float jumpForceUpDefault;
    private float grappleMaxLateralSpeedDefault;
    private float grappleDefaultSpeedDefault;

    private void Init() {
        climber = GameObject.Find("Climber");
        if (climber == null)
            return;

        bfpscExtraJumpTimeDefault           = climber.GetComponent<ClimbingAbilityV2>().bfpsc.extraJumpTime;
        bfpscExtraJumpPowerDefault          = climber.GetComponent<ClimbingAbilityV2>().bfpsc.extraJumpPower;
        bfpscSpeedDefault                   = climber.GetComponent<ClimbingAbilityV2>().bfpsc.speed;
        bfpscForwardSpeedDefault            = climber.GetComponent<ClimbingAbilityV2>().bfpsc.forwardSpeed;
        bfpscBackwardSpeedDefault           = climber.GetComponent<ClimbingAbilityV2>().bfpsc.backwardSpeed;
        bfpscStrafeSpeedDefault             = climber.GetComponent<ClimbingAbilityV2>().bfpsc.strafeSpeed;
        bfpscMovementMaxLateralSpeedDefault = climber.GetComponent<ClimbingAbilityV2>().bfpsc.movement.maxLateralSpeed;
        bfpscMovementMaxRiseSpeedDefault    = climber.GetComponent<ClimbingAbilityV2>().bfpsc.movement.maxRiseSpeed;
        jumpOffWallVerticalImpulseDefault   = climber.GetComponent<ClimbingAbilityV2>().jumpOffWallVerticalImpulse;
        jumpOffWallHorizontalImpulseDefault = climber.GetComponent<ClimbingAbilityV2>().jumpOffWallHorizontalImpulse;
        criticalVelocityDefault = climber.GetComponent<FallDeath>().criticalVelocity;
        jumpForceForwardDefault = climber.GetComponent<TicTac2>().jumpForceForward;
        jumpForceNormalDefault  = climber.GetComponent<TicTac2>().jumpForceNormal;
        jumpForceWallDefault    = climber.GetComponent<TicTac2>().jumpForceWall;
        jumpForceUpDefault      = climber.GetComponent<TicTac2>().jumpForceUp;
        grappleMaxLateralSpeedDefault = climber.GetComponent<GrappleHook>().maxLateralSpeed;
        grappleDefaultSpeedDefault    = climber.GetComponent<GrappleHook>().defaultSpeed;

        on = PersistentSaveObject.upgradesObtained.Contains(saveName);
        SetMaterial();

        hasInit = true;
    }

    private void Update() {
        if (!hasInit)
            Init();

        if (Input.GetButtonDown("Interact") && near) {
            on = !on;

            if (on) {
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.extraJumpTime = 1f;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.extraJumpPower = 25f;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.speed = 15f;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.forwardSpeed = 15f;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.backwardSpeed = 15f;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.strafeSpeed = 15f;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.movement.maxLateralSpeed = 20f;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.movement.maxRiseSpeed = 20f;
                climber.GetComponent<ClimbingAbilityV2>().jumpOffWallVerticalImpulse = 200f;
                climber.GetComponent<ClimbingAbilityV2>().jumpOffWallHorizontalImpulse = 500f;
                climber.GetComponent<FallDeath>().criticalVelocity = -39.5f;
                climber.GetComponent<TicTac2>().jumpForceForward = 150f;
                climber.GetComponent<TicTac2>().jumpForceNormal = 300f;
                climber.GetComponent<TicTac2>().jumpForceWall = 400f;
                climber.GetComponent<TicTac2>().jumpForceUp = 300f;
                climber.GetComponent<GrappleHook>().maxLateralSpeed = 20f;
                climber.GetComponent<GrappleHook>().defaultSpeed = 15f;

                PersistentSaveObject.upgradesObtained.Add(saveName);
            } else {
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.extraJumpTime = bfpscExtraJumpTimeDefault;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.extraJumpPower = bfpscExtraJumpPowerDefault;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.speed = bfpscSpeedDefault;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.forwardSpeed = bfpscForwardSpeedDefault;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.backwardSpeed = bfpscBackwardSpeedDefault;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.strafeSpeed = bfpscStrafeSpeedDefault;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.movement.maxLateralSpeed = bfpscMovementMaxLateralSpeedDefault;
                climber.GetComponent<ClimbingAbilityV2>().bfpsc.movement.maxRiseSpeed = bfpscMovementMaxRiseSpeedDefault;
                climber.GetComponent<ClimbingAbilityV2>().jumpOffWallVerticalImpulse = jumpOffWallVerticalImpulseDefault;
                climber.GetComponent<ClimbingAbilityV2>().jumpOffWallHorizontalImpulse = jumpOffWallHorizontalImpulseDefault;
                climber.GetComponent<FallDeath>().criticalVelocity = criticalVelocityDefault;
                climber.GetComponent<TicTac2>().jumpForceForward = jumpForceForwardDefault;
                climber.GetComponent<TicTac2>().jumpForceNormal = jumpForceNormalDefault;
                climber.GetComponent<TicTac2>().jumpForceWall = jumpForceWallDefault;
                climber.GetComponent<TicTac2>().jumpForceUp = jumpForceUpDefault;
                climber.GetComponent<GrappleHook>().maxLateralSpeed = grappleMaxLateralSpeedDefault;
                climber.GetComponent<GrappleHook>().defaultSpeed = grappleDefaultSpeedDefault;

                PersistentSaveObject.upgradesObtained.Remove(saveName);
            }

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
