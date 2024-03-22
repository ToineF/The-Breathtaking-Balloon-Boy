using UnityEngine;

namespace BlownAway.Cutscenes
{
    [CreateAssetMenu(fileName ="New Text Effect", menuName ="Dialogue/Text Effect")]
    public class TextEffectData : ScriptableObject
    {
        [SerializeField] public TextEffectRole Role;
        [SerializeField] public string Key;
        [SerializeField] public TextEffect TextEffect;
        [SerializeField] public Vector3Displacement MathDisplacement;
    }

    public enum TextEffectRole
    {
        HIDDEN = 0, // text not visible
        BASE = 1, // text visible, for main part
        SPECIAL = 2, // text visible, for smaller parts
    }
}