using BlownAway.Character;
using UnityEngine;

public class BouncyBalloon : SphereTrigger
{
    private new void Awake()
    {
        base.Awake();
        OnEnterTrigger += MakePlayerBounce;
    }

    void MakePlayerBounce()
    {
        if (!_lastOtherCollider.transform.parent.parent.TryGetComponent(out CharacterManager character)) return;

        // do something
        Debug.Log("*BOING*");
    }
}
