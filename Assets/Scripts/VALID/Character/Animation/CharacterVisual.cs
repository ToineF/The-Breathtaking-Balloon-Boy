using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    [SerializeField] private Transform PlayerRigidbody;

    private void Update()
    {
        transform.position = PlayerRigidbody.position;
    }
}
