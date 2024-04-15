using BlownAway.Player;
using UnityEngine;

namespace BlownAway.Character.Air
{
    public class AirUI : RadialUI
    {
        [Header("Reference")]
        [SerializeField] private CharacterAirManager AirManager;

        void Update()
        {
            FillAmount = AirManager.CurrentAir;
        }
    }
}