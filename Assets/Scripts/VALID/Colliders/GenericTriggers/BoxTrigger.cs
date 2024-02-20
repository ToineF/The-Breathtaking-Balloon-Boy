using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
public class BoxTrigger : GenericTrigger
{
    protected override void DrawGizmos(Color boxColor, Color wireColor)
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();

        Gizmos.color = boxColor;
        Gizmos.DrawCube(transform.position, _collider.bounds.size);
        Gizmos.color = wireColor;
        Gizmos.DrawWireCube(transform.position, _collider.bounds.size);
    }
}
