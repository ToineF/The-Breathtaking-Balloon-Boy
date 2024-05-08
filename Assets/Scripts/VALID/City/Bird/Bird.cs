using UnityEngine;
using DG.Tweening;
using System;

namespace BlownAway.City
{
    public class Bird : SphereTrigger
    {
        public State CurrentState { get; private set; }
        public enum State
        {
            IDLE = 0,
            FLY = 1,
        }

        [Header("Bird Animator")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _startFlying;

        [Header("Bird Settings")]
        [SerializeField] private Vector3 _targetDirection;
        [SerializeField] private float _directionRandomness;
        [SerializeField] private float _flySpeed;


        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += StartFlying;
        }

        public void StartFlying()
        {
            CurrentState = State.FLY;
            _animator.SetTrigger(_startFlying);

            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * _directionRandomness;
            Vector3 targetDirection = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y) + _targetDirection;
            gameObject.transform.DOMove(targetDirection, _flySpeed).OnComplete(EndFly);
        }

        private void EndFly()
        {
            Destroy(gameObject);
        }


    }
}