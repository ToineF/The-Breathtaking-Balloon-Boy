using UnityEngine;

namespace BlownAway.Cutscenes.Data
{
    [CreateAssetMenu(fileName = "CutsceneData", menuName = "CharacterData/Cutscene")]
    public class CharacterCutsceneData : ScriptableObject
    {
        [field: SerializeField, Tooltip("The time the user must hold the key to skip a cutscene")] public float CutsceneSkipTime { get; private set; } = 2f;
        [field: SerializeField, Tooltip("The minimum time the user has to wait before pass to the next dialogue")] public float DialogueEndWaitTime { get; private set; }
    }
}