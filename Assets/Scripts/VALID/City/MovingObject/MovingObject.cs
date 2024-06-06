using UnityEngine;
using DG.Tweening;
using AntoineFoucault.Utilities;
using System.Collections;

namespace BlownAway.City
{
    public class MovingObject : MonoBehaviour
    {
        [SerializeField, Tooltip("The object to move")] private GameObject _movingObject;
        [SerializeField, Tooltip("A list of positions to travel to")] private Transform[] _positions;
        [SerializeField, Tooltip("The time it takes to travel from one point to another in seconds")] private float _timeByTravel;
        [SerializeField, Tooltip("Should the travel time take the distance into account")] private bool _speedByDistance;
        [SerializeField, Tooltip("The multiplication of the distance to create a travel time")] private float _timeByDistanceMultiplier = 1;
        [SerializeField, Tooltip("The ease between two points")] private Ease _ease;
        [SerializeField, Tooltip("Does it move on start")] private bool _moveOnStart;
        [SerializeField, Tooltip("Does the position list loops")] private bool _loopPositions;
        [SerializeField, Tooltip("Does the position list plays only one at a time")] private bool _playOnePosition;
        [SerializeField, Tooltip("Does it move again if collider is re-entered")] private bool _canBeReactivated;
        [SerializeField, Tooltip("The time between collisions in seconds if it take be reactivated")] private float _timeBetweenPlayerCollisions;
        [SerializeField, Tooltip("The wait time between positions")] private float _waitTimeBetweenPositions;

        private int _index = 0;
        private float _timerMovements = 0;
        private float _timerCollisions = 0;
        private float _currentSpeed = 1;
        private bool _isMoving = false;
        private bool _canMove = true;
        private bool _isWaiting = false;

        private Vector3 _movingPosition;
        private Vector3 _currentVelocity;
        private void Start()
        {
            _movingPosition = _movingObject.transform.position;
            if (!_moveOnStart) return;
            StartMoving();
        }

        public void StartMoving()
        {
            if (_isMoving || !_canMove) return;

            if (!_playOnePosition) _index = 0;
            _isMoving = true;
            _movingObject.transform.position = _positions[_index].position;
            MoveToNextPoint();
        }

        private void MoveToNextPoint()
        {
            if (!_loopPositions && _index == _positions.Length - 1 && !_canBeReactivated)
            {
                StopMoving();
                return;
            }

            int previousIndex = _index;
            _index = (_index + 1) % _positions.Length;
            _currentSpeed = _speedByDistance ? Vector3.Distance(_positions[previousIndex].position, _positions[_index].position) * _timeByDistanceMultiplier : _timeByTravel;
            _timerMovements = 0;
        }

        private void StopMoving()
        {
            _isMoving = false;
            _canMove = false;
            _timerCollisions = _timeBetweenPlayerCollisions;
        }

        private void Update()
        {
            if (_isMoving)
            {
                if (_canMove)
                {
                    _timerMovements += Time.deltaTime;
                    //_movingObject.transform.position = Vector3.SmoothDamp(_movingObject.transform.position, _movingPosition, ref _currentVelocity, .1f);
                }
            }
            else
            {
                _timerCollisions -= Time.deltaTime;
                if (_timerCollisions < 0 && _canBeReactivated)
                {
                    _canMove = true;
                }
            }



        }

        private void FixedUpdate()
        {
            if (!_isMoving) return;
            if (_isWaiting) return;

            float elapsedPercentage = _timerMovements / _currentSpeed;
            elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
            _movingObject.transform.position = Vector3.Lerp(_positions[(_index - 1).Modulo(_positions.Length)].position, _positions[_index].position, elapsedPercentage);

            if (elapsedPercentage >= 1)
            {
                if (_playOnePosition)
                    StopMoving();
                else
                    StartCoroutine(WaitForMoveToNextPoint());
            }
        }

        private IEnumerator WaitForMoveToNextPoint()
        {
            _isWaiting = true;
            yield return new WaitForSeconds(_waitTimeBetweenPositions);
            _isWaiting = false;
            MoveToNextPoint();
        }

        public void ResetPosition()
        {
            _index = 0;
            _timerMovements = 0;
            _timerCollisions = 0;
            _currentSpeed = 1;
            _isMoving = false;
            _canMove = true;
            _isWaiting = false;
            _movingObject.transform.position = _positions[_index].position;
        }
    }
}
