using UnityEngine;
using System;
using AntoineFoucault.Utilities;


namespace BlownAway.Children
{
    public class ChildTrigger : BoxTrigger
    {
        [Header("Child")]
        [SerializeField] private ChildAnimator _animator;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += FindChild;
        }

        private void FindChild()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            collider.Manager.ChildrenManager.AddChild();
            _animator.Found();
        }
    }
}