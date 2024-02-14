using System.Collections;
using UnityEngine;
using BlownAway.Character.Air.Data;

namespace BlownAway.Character.Air
{
    public class CharacterAirManager : MonoBehaviour
    {
        public CharacterManager Manager { get; set; }

        public float CurrentAir { get => _currentAir; private set => _currentAir = Mathf.Clamp(value, 0, 1); }
        public bool AirIsFull { get => _currentAir >= 1;}
        public bool AirIsEmpty { get => _currentAir <= 0.00001f;}


        [field:SerializeField] public CharacterAirData AirData { get; private set; }

        private float _currentAir;

        private Coroutine _refillAirCoroutine;

        public void AddAir(float airAddSpeed)
        {
            CurrentAir += airAddSpeed * Time.deltaTime;
        }

        public void ReduceAir(float airReductionSpeed)
        {
            CurrentAir -= airReductionSpeed * Time.deltaTime;
        }

        public void EmptyAir()
        {
            CurrentAir = 0;
        }

        public void RefreshAir()
        {
            CurrentAir = 1;
        }

        public void AddAirUntilFullIfEmpty(CharacterManager manager, float airAddSpeed, float delay)
        {
            if (!manager.AirManager.AirIsEmpty) return;

            manager.Inputs.ResetLastPropulsionInputDirection();

            _refillAirCoroutine = StartCoroutine(AddAirUntilFull(airAddSpeed, delay));
        }

        public void StopAddingAir()
        {
            if (_refillAirCoroutine != null)
                StopCoroutine(_refillAirCoroutine);
        }

        private IEnumerator AddAirUntilFull(float airAddSpeed, float delay)
        {
            yield return new WaitForSeconds(delay);

            while (!AirIsFull) {
                AddAir(airAddSpeed);
                yield return new WaitForEndOfFrame();
            }

            yield break;
        }
    }
}