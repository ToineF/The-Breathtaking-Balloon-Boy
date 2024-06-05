using UnityEngine;

public class NameCollider : BoxTrigger
{
    [SerializeField] private Animation _animation;

    private new void Awake()
    {
        base.Awake();
        OnEnterTrigger += StartApparition;
    }


    private void StartApparition()
    {
        if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;
        _animation.Play();
    }
}
