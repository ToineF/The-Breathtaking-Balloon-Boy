using UnityEngine;

namespace BlownAway.Character.Air
{
    public class CharacterAirManager : MonoBehaviour
    {
        public float CurrentAir { get => _currentAir; private set => _currentAir = Mathf.Clamp(value, 0, 1); }

        // Variable (Put in a ScriptableObject)
        [field:SerializeField, Tooltip("The speed at which the air reduces while floating")] public float FloatingAirReductionSpeed { get; private set; }
        [field:SerializeField, Tooltip("The speed at which the air reduces while propulsing")] public float PropulsionAirReductionSpeed {get; private set;}
        [field:SerializeField] public float AirRefillSpeed {get; private set;}
        [field:SerializeField] public float AirRefillStart {get; private set;}

        private float _currentAir;

        private float _currentFillingAir;
        private float _airRefillTime;

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
    }
}