using UnityEngine;
using FMODUnity;
using AntoineFoucault.Utilities;

namespace BlownAway
{
    public class MenuAudioSettings : MonoBehaviour
    {
        [SerializeField] private EventReference[] _onSelectSound;
        [SerializeField] private EventReference[] _onCancelSound;
        [SerializeField] private EventReference[] _onConfirmSound;
        public void PlayOnSelectSound()
        {
            FMODAudioManager.Instance?.PlayClip(_onSelectSound.GetRandomItem());
        }

        public void PlayCancelSound()
        {
            FMODAudioManager.Instance?.PlayClip(_onCancelSound.GetRandomItem());
        }

        public void PlayConfirmSound()
        {
            FMODAudioManager.Instance?.PlayClip(_onConfirmSound.GetRandomItem());
        }
    }
}