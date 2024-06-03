using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BlownAway.Character
{
    //[RequireComponent(typeof(DecalProjector))]
    public class CharacterShadow : MonoBehaviour
    {
        [SerializeField] private CharacterManager _manager;
        [SerializeField] private bool _hideOnGrounded;

        private DecalProjector _projector;

        private void Awake()
        {
            if (_hideOnGrounded) Destroy(this);
            _projector = GetComponent<DecalProjector>();
        }

        private void Update()
        {
            _projector.enabled = !_manager.MovementManager.IsGrounded;
        }



        //[SerializeField] private float _maxDistance;
        //[SerializeField] private LayerMask _plateformLayer;



        //private void Start()
        //{
        //    //Debug.Log(_projector.drawDistance);
        //    //Debug.Log(_projector.size.x);
        //    //Debug.Log(_projector.size.y);
        //    //Debug.Log(_projector.uvScale); // 1 1
        //    //Debug.Log(_projector.uvBias); // 0 0
        //}

        //[SerializeField] private Vector2 uvBias;
        //[SerializeField] private Vector2 uvScale;

        //void Update()
        //{
        //    bool raycast = Physics.Raycast(_projector.transform.position, Vector2.down, out RaycastHit hit, _maxDistance, _plateformLayer);
        //    if (raycast == false) return;

        //    float distance = _projector.transform.position.y - hit.transform.position.y;
        //    //Debug.Log(distance);
        //    //Debug.DrawLine(new Vector3(_projector.transform.position.x, _projector.transform.position.y), new Vector3(_projector.transform.position.x, hit.transform.position.y), Color.red);
        //    //_projector.
        //}
    }
}