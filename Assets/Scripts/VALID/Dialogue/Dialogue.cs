using UnityEngine;

namespace BlownAway.Cutscenes
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public string[] Texts => _texts;
        public DialogueCharacterData CharacterData => characterData;

        [SerializeField] private string[] _texts;
        [SerializeField] private DialogueCharacterData characterData;
    }
}