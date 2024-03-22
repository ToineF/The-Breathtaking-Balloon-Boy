using UnityEngine;

namespace BlownAway.Cutscenes
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
    public class DialogueCharacterData : ScriptableObject
    {
        public Color DialogueBoxColor;
        public Color DialogueTextColor;
        public AudioClip[] SoundsTalk;
        public int TalkFrequency;
    }
}