using BlownAway.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PictureController : MonoBehaviour
{
    [SerializeField] private GameObject _picturePlanePrefab;
    [SerializeField] private Animator _pictureAnimator;
    [SerializeField] private string _pictureAnimationName;

    private PlayerInputs _inputs;

    private void Awake()
    {
        _inputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player_Archive.TakePicture.performed += TakePicture;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        _inputs.Player_Archive.TakePicture.performed -= TakePicture;
    }

    private void TakePicture(InputAction.CallbackContext context)
    {
        if (!CharacterControllerTest.Instance.CanMove) return;
        _pictureAnimator.SetTrigger(_pictureAnimationName);
        Instantiate(_picturePlanePrefab, transform.position, Quaternion.identity);
    }
}
