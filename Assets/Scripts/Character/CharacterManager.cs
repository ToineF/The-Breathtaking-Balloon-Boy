using UnityEngine;

namespace Character
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; set; }
        

        // Idle Data
        /// /////////////////////////////////////////////////////////// PUT IN A SCRIPTABLE OBJECT 
        [field:SerializeField, Tooltip("The walking speed the character starts moving at")] public float BaseWalkSpeed { get; set; }
        public Vector3 MoveInputDirection { get; set; }

    }
}