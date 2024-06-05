using System;
using UnityEngine;

public class ShopTrigger : BoxTrigger
{
    [SerializeField] private CanvasGroup _ui;

    private new void Awake()
    {
        base.Awake();
        OnEnterTrigger += StartApparition;
    }

    private void StartApparition()
    {
        if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;


    }
}
