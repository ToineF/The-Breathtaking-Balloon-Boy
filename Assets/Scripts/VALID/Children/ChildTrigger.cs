using UnityEngine;
using BlownAway.Cutscenes;

namespace BlownAway.Children
{
    public class ChildTrigger : BoxTrigger
    {
        [Header("Child")]
        [SerializeField] private ChildAnimator _animator;
        [SerializeField] private Cutscene _cutscene;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += FindChild;
        }

        private void FindChild()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            collider.Manager.ChildrenManager.AddChild();
            if (_cutscene != null) collider.Manager.CutsceneManager.StartNewSequence(_cutscene);
            _animator.Found();
        }
    }
}