using UnityEngine;

namespace BlownAway.Hitbox.Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Gizmo Settings")]
        [SerializeField] private float _radius;
        [SerializeField] private Color _color = Color.yellow;
        [SerializeField] private Color _wireColor = Color.black;

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
            Gizmos.color = _wireColor;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}