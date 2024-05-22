using FMODUnity;
using UnityEngine;

namespace BlownAway.Cutscenes
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
    public class DialogueCharacterData : ScriptableObject
    {
        public string Name;
        public Color DialogueBoxColor;
        public Color DialogueTextColor;
        public EventReference[] SoundsTalk;
        public int TalkFrequency;
    }
}