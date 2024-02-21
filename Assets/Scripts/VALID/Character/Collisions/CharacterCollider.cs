using BlownAway.Character;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    public CharacterManager Manager { get; set; }

    [field: SerializeField] public Rigidbody Rigidbody { get; set; }
    [field: SerializeField] public Collider Collider { get; set; }
}
