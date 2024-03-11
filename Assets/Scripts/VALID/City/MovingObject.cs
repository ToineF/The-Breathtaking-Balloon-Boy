using UnityEngine;
using DG.Tweening;
using AntoineFoucault.Utilities;
using System.Collections;

namespace BlownAway.City
{
    public class MovingObject : MonoBehaviour
    {
        [SerializeField] [Tooltip("A object to move")] private GameObject _movingObject;
        [SerializeField] [Tooltip("A list of positions to travel to")] private Transform[] _positions;
        [SerializeField] [Tooltip("The time it takes to travel from one point to another in seconds")] private float _timeByTravel;
        [SerializeField] [Tooltip("Should the travel time take the distance into account")] private bool _speedByDistance;
        [SerializeField] [Tooltip("The multiplication of the distance to create a travel time")] private float _timeByDistanceMultiplier = 1;
        [SerializeField] [Tooltip("The ease between two points")] private Ease _ease;
        [SerializeField] [Tooltip("Does it move on start")] private bool _moveOnStart;
        [SerializeField] [Tooltip("Does the position list loops")] private bool _loopPositions;

        private int _index = 0;
        private float _timer = 0;
        private float _currentSpeed = 1;
        private bool _hasStarted = false;

        private void Start()
        {
            if (!_moveOnStart) return;
            StartMoving();
        }

        public void StartMoving()
        {
            _hasStarted = true;
            _movingObject.transform.position = _positions[_index].position;
            MoveToNextPoint();
        }

        private void MoveToNextPoint()
        {
            if (!_loopPositions && _index == _positions.Length-1) return;

            int previousIndex = _index;
            _index = (_index + 1) % _positions.Length;
            _currentSpeed = _speedByDistance ? Vector3.Distance(_positions[previousIndex].position, _positions[_index].position) * _timeByDistanceMultiplier : _timeByTravel;
            //_currentSpeed = _speedByDistance ? Vector3.Distance(_positions[_index].position, transform.position) * _timeByDistanceMultiplier : _timeByTravel;
            _timer = 0;
            //_movingObject.transform.DOMove(_positions[_index].position, speed).SetEase(_ease).OnComplete(MoveToNextPoint);
        }

        private void FixedUpdate()
        {
            if (!_hasStarted) return;

            _timer += Time.deltaTime;

            float elapsedPercentage = _timer / _currentSpeed;
            elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
            _movingObject.transform.position = Vector3.Lerp(_positions[(_index-1).Modulo(_positions.Length)].position, _positions[_index].position, elapsedPercentage);
            
            if (elapsedPercentage >= 1)
            {
                MoveToNextPoint();
            }
        }
    }
}
