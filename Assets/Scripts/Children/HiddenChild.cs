using BlownAway.Player;
using UnityEngine;

namespace BlownAway.GPE
{
    [RequireComponent(typeof(Collider))]
    public class HiddenChild : MonoBehaviour
    {
        private bool _taken;

        private void OnTriggerEnter(Collider other)
        {
            if (_taken) return;
            if (!other.TryGetComponent(out ChildrenManager character)) return;

            _taken = true;
            character.AddChild();
            Destroy(gameObject);
        }
    }
}