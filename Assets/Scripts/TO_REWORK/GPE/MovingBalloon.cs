using UnityEngine;
using DG.Tweening;

namespace BlownAway.GPE {
    public class MovingBalloon : MonoBehaviour
    {
        [SerializeField] [Tooltip("A object to move")] private GameObject _movingObject;
        [SerializeField] [Tooltip("A list of positions to travel to")] private Transform[] _positions;
        [SerializeField] [Tooltip("The time it takes to travel from one point to another in seconds")] private float _timeByTravel;
        [SerializeField] [Tooltip("Should the travel time take the distance into account")] private bool _speedByDistance;
        [SerializeField] [Tooltip("The multiplication of the distance to create a travel time")] private float _timeByDistanceMultiplier = 1;
        [SerializeField] [Tooltip("The ease between two points")] private Ease _ease;
        private int _index = 0;

        private void Start()
        {
            _movingObject.transform.position = _positions[_index].position;
            MoveToNextPoint();
        }

        private void MoveToNextPoint()
        {
            _index = (_index + 1) % _positions.Length;

            var speed = _speedByDistance ? Vector3.Distance(_positions[_index].position, transform.position) * _timeByDistanceMultiplier : _timeByTravel;
            _movingObject.transform.DOMove(_positions[_index].position, speed).SetEase(_ease).OnComplete(MoveToNextPoint);
        }
    }
}
