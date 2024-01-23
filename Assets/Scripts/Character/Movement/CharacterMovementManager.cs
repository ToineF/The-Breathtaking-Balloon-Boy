using BlownAway.Character;
using BlownAway.Character.States;
using System;
using UnityEngine;

public class CharacterMovementManager : MonoBehaviour
{
    public Action OnGroundEnter;
    public Action OnGroundExit;

    // Idle Data
    /// /////////////////////////////////////////////////////////// PUT IN A SCRIPTABLE OBJECT
    [field: SerializeField, Tooltip("The walking speed the character starts moving at")] public float BaseWalkSpeed { get; set; }
    [field: SerializeField, Tooltip("The lateral speed the character moves at while falling")] public float FallDeplacementSpeed { get; set; }
    public Vector3 CurrentVelocity { get; set; }

    [Header("Gravity")]
    [ReadOnly] public float CurrentGravity;
    public float BaseGravity;
    public float MaxGravity;
    public float FloatingGravity;
    public float GravityIncreaseByFrame;

    [Header("Ground Check")]
    public float GroundCheckDistance;
    public float GroundDetectionSphereRadius;
    public LayerMask GroundLayer;
    public RaycastHit[] GroundHitResults { get; set; }

    [Header("Forces")]
    [ReadOnly] public Vector3 Force;
    [ReadOnly] public Vector3 CurrentForce;
    [ReadOnly] public bool IsGrounded;
    [HideInInspector] public RaycastHit LastGround;

    private void Start()
    {
        GroundHitResults = new RaycastHit[2];
        CurrentGravity = BaseGravity;
    }

    public void CheckIfGrounded(CharacterStatesManager manager)
    {
        var lastGrounded = IsGrounded;
        IsGrounded = Physics.SphereCastNonAlloc(CharacterManager.Instance.CharacterTransform.position, GroundDetectionSphereRadius, Vector3.down, GroundHitResults, GroundCheckDistance, GroundLayer) > 0;
        if (lastGrounded != IsGrounded)
        {
            if (IsGrounded) // On Ground Enter
            {
                LastGround = GroundHitResults[0];
                OnGroundEnter?.Invoke();
                CurrentGravity = BaseGravity;
                manager.SwitchState(manager.IdleState);
            }
            else // On Ground Leave
            {
                OnGroundExit?.Invoke();
                manager.SwitchState(manager.FallingState); // HERE DISCOSSIATE FALLING FROM PROPULSION
            }
        }
    }
    public void MoveAtSpeed(float moveSpeed)
    {
        Vector3 moveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)) * CharacterManager.Instance.Inputs.MoveInputDirection.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * CharacterManager.Instance.Inputs.MoveInputDirection.x).normalized;
        moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
        //SetAnimation(moveDirection);
        CurrentVelocity += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void ApplyVelocity()
    {
        CharacterManager.Instance.CharacterRigidbody.velocity = CurrentVelocity;
    }

    public void ResetVelocity()
    {
        CurrentVelocity = Vector3.zero;
    }

}
