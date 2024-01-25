using BlownAway.Player;
using UnityEngine;

namespace BlownAway.GPE
{
    [RequireComponent(typeof(Collider))]
    public class HiddenChild : MonoBehaviour
    {
        [Header("Sounds")]
        [SerializeField] private AudioClip _childFound;

        private bool _taken;

        private void OnTriggerEnter(Collider other)
        {
            if (_taken) return;
            if (!other.TryGetComponent(out ChildrenManager character)) return;

            _taken = true;
            character.AddChild();

            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClip(_childFound);

            Destroy(gameObject);
        }
    }
}