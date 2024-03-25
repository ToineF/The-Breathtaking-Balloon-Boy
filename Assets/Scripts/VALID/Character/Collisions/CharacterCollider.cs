using BlownAway.Character;
using UnityEngine;

public class CharacterCollider : CharacterSubComponent
{
    [field: SerializeField] public Rigidbody Rigidbody { get; set; }
    [field: SerializeField] public Collider Collider { get; set; }
    [field: SerializeField] public GameObject LowerStepRaycast { get; set; }
    [field: SerializeField] public GameObject UpperStepRaycast { get; set; }

    protected override void StartScript(CharacterManager manager)
    {
        UpperStepRaycast.transform.localPosition += Vector3.up * manager.Data.GroundDetectionData.StepHeight;
    }
}
