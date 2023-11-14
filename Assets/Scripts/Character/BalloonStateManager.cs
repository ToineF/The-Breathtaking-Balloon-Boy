using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BalloonStateManager : MonoBehaviour
{
    public PlayerInputs Inputs;
    public CharacterControllerTest CharaController;

    private BalloonBaseState _currentState;
    public BalloonFlower BalloonFlower = new BalloonFlower();
    public BalloonHammer BalloonHammer = new BalloonHammer();


    [Header("Hammer")]
    public GameObject HammerModelisation;
    public Animator Animator;
    public string AnimatorHammerTrigger;
    public GameObject HammerFXGroundPrefab;
    public HammerCollision HammerSideCollider;
    public float HammerAirPercentageUsed;
    public bool IsHammerFalling = false;
    [Space(25)]
    public float HammerGroundForce;
    public float HammerGroundJumpTime;
    [Range(0, 1)] public float HammerGroundAccel;
    [Range(0, 1)] public float HammerGroundDecel;
    [Space(25)]
    public float HammerAirFallForce;
    [Range(0, 1)] public float HammerAirFallAccel;
    [Space(25)]
    public float HammerAirJumpForce;
    public float HammerAirJumpTime;
    [Range(0, 1)] public float HammerAirJumpAccel;
    [Range(0, 1)] public float HammerAirJumpDecel;
    [Space(25)]
    public float HammerGravityAccel;
    [ReadOnly] public float HammerWindForce;
    public float HammerWindForceMultiplier = 1;

    [Header("Flower")]
    public GameObject FlowerModelisation;
    public GameObject FlowerJumpFXPrefab;
    public float FlowerAirPercentageUsed;
    public float FlowerJumpForce;
    public float FlowerJumpTime;
    [Range(0, 1)] public float FlowerJumpAccel;
    [Range(0, 1)] public float FlowerJumpDecel;
    public float FlowerGlideForce;
    [Range(0, 1)] public float FlowerGlideAccel;
    [Space(25)]
    [ReadOnly] public int FlowerJumps;
    public int FlowerMaxJumps = 1;
    // force with wind

    [Header("UI")]
    public GameObject UIStateHammer;
    public GameObject UIStateFlower;

    private void Awake()
    {
        Inputs = new PlayerInputs();
        Inputs.Enable();
        CharaController = GetComponent<CharacterControllerTest>();
    }

    private void Start()
    {
        DisableAllObjects();
        _currentState = BalloonHammer;
        _currentState.StartState(this);
        Inputs.Player.Action.performed += _currentState.OnActionPressed;
        Inputs.Player.SecondaryAction.performed += _currentState.OnSecondaryActionPressed;
        Inputs.Player.ChangeBalloon.performed += ChangeState;

        FlowerJumps = FlowerMaxJumps;
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SwitchState(BalloonBaseState state)
    {
        DisableAllObjects();

        Inputs.Player.Action.performed -= _currentState.OnActionPressed;
        Inputs.Player.SecondaryAction.performed -= _currentState.OnSecondaryActionPressed;

        _currentState = state;
        _currentState.StartState(this);
        Inputs.Player.Action.performed += _currentState.OnActionPressed;
        Inputs.Player.SecondaryAction.performed += _currentState.OnSecondaryActionPressed;

    }

    private void ChangeState(InputAction.CallbackContext context)
    {
        var changeVector = context.ReadValue<Vector2>();

        if (changeVector == Vector2.down)
            SwitchState(BalloonHammer);

        if (changeVector == Vector2.up)
            SwitchState(BalloonFlower);

    }

    private void DisableAllObjects()
    {
        HammerModelisation.SetActive(false);
        FlowerModelisation.SetActive(false);
        UIStateHammer.SetActive(false);
        UIStateFlower.SetActive(false);
    }

    public BalloonBaseState GetState()
    {
        return _currentState;
    }
}
