using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
[RequireComponent(typeof(SphereCollider))]
public class SphereTrigger : GenericTrigger
{
    private SphereCollider _sphereCollider;

    private new void Awake()
    {
        base.Awake();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    protected override void DrawGizmos(Color boxColor, Color wireColor)
    {
        if (_sphereCollider == null) _sphereCollider = GetComponent<SphereCollider>();

        float scale = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z);

        Gizmos.color = boxColor;
        Gizmos.DrawSphere(transform.position, scale * _sphereCollider.radius);
        Gizmos.color = wireColor;
        Gizmos.DrawWireSphere(transform.position, scale * _sphereCollider.radius);
    }
}
