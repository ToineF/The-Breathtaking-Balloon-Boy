using AntoineFoucault.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BlownAway.City
{
    public class BirdGroup : MonoBehaviour
    {
        [SerializeField] private bool _allBirdsFlyTogether;
        [SerializeField] private Bird _birdPrefab;
        [SerializeField] private Vector2Int _birdsCountMinMax;
        [SerializeField] private float _spawnRandomness;
        [SerializeField] private Vector2 _birdsScaleMinMax;

        private List<Bird> _birds;

        public void SpawnBirds()
        {
            Clear();

            int birdsCount = Random.Range(_birdsCountMinMax.x, _birdsCountMinMax.y);
            for (int i = 0; i < birdsCount; i++)
            {
                Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * _spawnRandomness;
                Vector3 spawnPoint = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
                Vector3 rotation = new Vector3(0, Random.Range(0, 360), 0);
                Bird newBird = Instantiate(_birdPrefab, spawnPoint, Quaternion.Euler(rotation), transform);
                float randomScale = UnityEngine.Random.Range(_birdsScaleMinMax.x, _birdsScaleMinMax.y);
                newBird.transform.localScale = Vector3.one * randomScale;


                _birds.Add(newBird);
            }
        }

        public void Clear()
        {
            if (_birds == null) return;
            foreach (var item in _birds)
            {
                DestroyImmediate(item);
            }
            _birds.Clear();
            transform.ClearImmediate();
        }


        private void SetAllBirdsFree()
        {
            if (!_allBirdsFlyTogether) return;

            foreach (var bird in _birds)
            {
                if (bird.CurrentState == Bird.State.FLY) continue;

                bird.StartFlying();
            }
        }
    }
}