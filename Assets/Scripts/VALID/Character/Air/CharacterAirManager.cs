using UnityEngine;

namespace BlownAway.Character.Air
{
    public class CharacterAirManager : MonoBehaviour
    {
        public float CurrentAir { get => _currentAir; private set => Mathf.Clamp(_currentAir, 0, 1); }

        // Variable (Put in a ScriptableObject)
        [SerializeField] private float _airReductionSpeed;
        [SerializeField] private float _airDashReductionSpeed;
        [SerializeField] private float _airRefillSpeed;
        [SerializeField] private float _airRefillStart;

        private float _currentAir;
        private float _currentFillingAir;
        private float _airRefillTime;

        public void AddAir(float airAddSpeed)
        {
            CurrentAir += airAddSpeed;
        }

        public void ReduceAir(float airReductionSpeed)
        {
            CurrentAir -= airReductionSpeed;
        }

        public void EmptyhAir()
        {
            CurrentAir = 0;
        }

        public void RefreshAir()
        {
            CurrentAir = 1;
        }
    }
}