using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BalloonStateManager : MonoBehaviour
{
    public PlayerInputs Inputs;
    public CharacterControllerTest CharaController;

    private BalloonBaseState _currentState;
    public BalloonNone BalloonNone = new BalloonNone();
    public BalloonFlower BalloonFlower = new BalloonFlower();
    public BalloonHammer BalloonHammer = new BalloonHammer();


    [Header("Hammer")]
    public Animator Animator;
    public string AnimatorHammerTrigger;
    public GameObject HammerFXGroundPrefab;
    public HammerCollision HammerSideCollider;
    public float HammerAirPercentageUsed;
    public bool IsHammerFalling = false;
    [Space(25)]
    public float HammerGroundForce;
    public float HammerGroundJumpTime;
    [Range(0,1)] public float HammerGroundAccel;
    [Range(0,1)] public float HammerGroundDecel;
    [Space(25)]
    public float HammerAirFallForce;
    [Range(0, 1)] public float HammerAirFallAccel;
    [Space(25)]
    public float HammerAirJumpForce;
    public float HammerAirJumpTime;
    [Range(0, 1)] public float HammerAirJumpAccel;
    [Range(0, 1)] public float HammerAirJumpDecel;

    private void Awake()
    {
        Inputs = new PlayerInputs();
        Inputs.Enable();
        CharaController = GetComponent<CharacterControllerTest>();
    }

    private void Start()
    {
        _currentState = BalloonHammer;
        _currentState.StartState(this);
        Inputs.Player.Action.performed += _currentState.OnActionPressed;
        Inputs.Player.SecondaryAction.performed += _currentState.OnSecondaryActionPressed;
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SwitchState(BalloonBaseState state)
    {
        Inputs.Player.Action.performed -= _currentState.OnActionPressed;

        _currentState = state;
        _currentState.StartState(this);
        Inputs.Player.Action.performed += _currentState.OnActionPressed;
        Inputs.Player.SecondaryAction.performed -= _currentState.OnSecondaryActionPressed;
    }
}
