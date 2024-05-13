using UnityEngine;
using DG.Tweening;

namespace BlownAway.City
{
    public class Bird : SphereTrigger
    {
        public State CurrentState { get; private set; }
        public bool TriggerOtherBirds { get => _triggerOtherBirds; set => _triggerOtherBirds = value; }

        public enum State
        {
            IDLE = 0,
            FLY = 1,
        }

        [Header("Bird Animator")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _startFlying;
        [SerializeField] private string _startPecking;
        [SerializeField] private float _peckingRandomness;

        [Header("Fly Settings")]
        [SerializeField] private Vector3 _targetDirection;
        [SerializeField] private float _directionRandomness;
        [SerializeField] private float _flySpeed;
        [SerializeField] private bool _triggerOtherBirds;


        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += StartFlying;
        }

        public void StartFlying()
        {
            if (CurrentState == State.FLY) return;

            CurrentState = State.FLY;
            _animator.SetTrigger(_startFlying);

            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * _directionRandomness;
            Vector3 targetDirection = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y) + _targetDirection;
            gameObject.transform.DOMove(targetDirection, _flySpeed).OnComplete(EndFly);


            transform.LookAt(new Vector3(targetDirection.x, 0, targetDirection.z));
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 90, 0);

            SetNeighboursBirdsFree();
        }

        private void SetNeighboursBirdsFree()
        {
            if (!_triggerOtherBirds) return;

            Debug.Log(_sphereCollider);
            if (_sphereCollider == null) _sphereCollider = GetComponent<SphereCollider>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, _sphereCollider.radius);
            Debug.Log(colliders);

            foreach (var item in colliders)
            {
                if (item.TryGetComponent(out Bird bird))
                {
                    if (bird.CurrentState != State.FLY)
                        bird.StartFlying();
                }
            }
        }

        private void EndFly()
        {
            Destroy(gameObject);
        }


    }
}