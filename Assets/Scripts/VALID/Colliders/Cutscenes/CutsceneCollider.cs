using UnityEngine;

namespace BlownAway.Cutscenes
{
    public class CutsceneCollider : BoxTrigger
    {
        [SerializeField] private Cutscene _cutscene;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += StartCutscene;
        }

        private void StartCutscene()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            if (_cutscene != null)
            {
                collider.Manager.CutsceneManager.StartNewSequence(_cutscene, collider.Manager);
            }
        }
    }
}