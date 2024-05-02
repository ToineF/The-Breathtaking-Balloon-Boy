using BlownAway.Character;
using UnityEngine;

public class CharacterCollider : CharacterSubComponent
{
    [field: SerializeField] public Rigidbody Rigidbody { get; set; }
    [field: SerializeField] public Collider Collider { get; set; }
}
