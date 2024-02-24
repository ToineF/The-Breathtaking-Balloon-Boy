using System.Collections;
using UnityEngine;

namespace BlownAway.GPE
{
    public class AirRefreshManager : MonoBehaviour
    {
        [SerializeField] private AirRefreshBonus AirRefreshPrefab;
        [SerializeField] private float _timeBeforeRespawn;

        private void Start()
        {
            SpawnNewAirRefreshBonus();
        }

        private void SpawnNewAirRefreshBonus()
        {
            AirRefreshBonus bonus = Instantiate(AirRefreshPrefab, transform.position, Quaternion.identity, transform);
            bonus.Manager = this;
        }

        public void StartWaitForRespawn()
        {
            StartCoroutine(WaitForRespawn());
        }

        private IEnumerator WaitForRespawn()
        {
            yield return new WaitForSeconds(_timeBeforeRespawn);

            SpawnNewAirRefreshBonus();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}