using BlownAway.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BalloonBoyController : MonoBehaviour
{
    [SerializeField] private int _maxJumps;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpTime;
    [Range(0, 1)][SerializeField] private float _jumpAccel;
    [Range(0, 1)][SerializeField] private float _jumpDecel;
    [SerializeField] private float _glideForce;
    [Range(0, 1)] [SerializeField] private float _glideAccel;
    [SerializeField] private GameObject _jumpFXPrefab;

    private int _jumps;

    private PlayerInputs _inputs;

    private void Awake()
    {
        _inputs = new PlayerInputs();
        _jumps = _maxJumps;
    }

    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.TakePicture.performed += BalloonBump;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        _inputs.Player.TakePicture.performed -= BalloonBump;
    }

    private void BalloonBump(InputAction.CallbackContext context)
    {
        if (_jumps <= 0) return;

        CharacterControllerTest.Instance.OnGroundEnter += PlayerEnterGround;
        if (!CharacterControllerTest.Instance.IsGrounded) // also is force of wind is null
            _jumps--;


        Vector3 hit = Vector3.up * _jumpForce;
        float startLerp = _jumpAccel;
        float endLerp = _jumpDecel;
        float time = _jumpTime;
        CharacterControllerTest.Instance.SetForceForTime(hit, time, startLerp, endLerp);

        Vector3 glideForce = Vector3.up * _glideForce;
        float glideAccel = _glideAccel;
        //CharacterControllerTest.Instance.AddForce(glideForce, glideAccel);

        Collider collider = CharacterControllerTest.Instance.GetComponent<Collider>();
        GameObject.Instantiate(_jumpFXPrefab, collider.bounds.center - collider.bounds.extents.y * Vector3.up, _jumpFXPrefab.transform.rotation);
    }

    private void PlayerEnterGround()
    {
        Debug.Log("h");
        _jumps = _maxJumps;
        CharacterControllerTest.Instance.OnGroundEnter -= PlayerEnterGround;
        CharacterControllerTest.Instance.SetForce(Vector3.zero, 1);
    }
}
