using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
public class BoxTrigger : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnEnterTrigger { get; private set; }
    [field: SerializeField] public UnityEvent OnExitTrigger { get; private set; }

    [Header("Settings")]
    [SerializeField] private bool _oneShot = false;
    [SerializeField] private bool _isTrigger = true;

    [Header("Filters")]
    [SerializeField] private GameObject[] _gameObjectsToIgnore;
    [SerializeField] private LayerMask _layersToDetect = -1;

    [Header("Gizmo Settings")]
    [SerializeField] private bool _displayGizmos = false;
    [SerializeField] private bool _showOnlyWhileSelected = true;
    [SerializeField] private Color _gizmoColor = Color.green;
    [SerializeField] private Color _gizmoSelectedColor = Color.red;
    [SerializeField] private Color _gizmoWireColor = Color.black;
    [SerializeField] private Color _gizmoSelectedWireColor = Color.white;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = _isTrigger;
    }

    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TriggerEnter(collision.collider);
    }

    private void TriggerEnter(Collider other)
    {
        if (!IsCollisionValid(other)) return;

        OnEnterTrigger?.Invoke();

        if (_oneShot) Destroy(gameObject);
    }


    private void OnTriggerExit(Collider other)
    {
        TriggerExit(other);
    }

    private void OnCollisionExit(Collision collision)
    {
        TriggerExit(collision.collider);
    }

    private void TriggerExit(Collider other)
    {
        if (!IsCollisionValid(other)) return;

        OnExitTrigger?.Invoke();
    }

    private bool IsCollisionValid(Collider other)
    {
        // GameObject Check
        if (_gameObjectsToIgnore.Length > 0)
        {
            foreach (GameObject go in _gameObjectsToIgnore)
            {
                if (go == other.gameObject) return false;
            }
        }

        // Layer Check
        if (!IsInLayerMask(other.gameObject.layer, _layersToDetect)) return false;

        return true;
    }
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (!_displayGizmos) return;
        if (!_showOnlyWhileSelected) return;

        DrawGizmos(_gizmoColor, _gizmoWireColor);
    }

    private void OnDrawGizmosSelected()
    {
        if (!_displayGizmos) return;

        DrawGizmos(_gizmoSelectedColor, _gizmoSelectedWireColor);
    }
    private void DrawGizmos(Color boxColor, Color wireColor)
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();

        Gizmos.color = boxColor;
        Gizmos.DrawCube(transform.position, _collider.bounds.size);
        Gizmos.color = wireColor;
        Gizmos.DrawWireCube(transform.position, _collider.bounds.size);
    }
    #endregion
}
