using UnityEngine;


namespace BlownAway.Character
{
    public class CharacterSubComponent : MonoBehaviour
    {
        public CharacterManager Manager { get; private set; }

        public void InitScript(CharacterManager manager)
        {
            Manager = manager;
            StartScript(manager);
        }

        protected virtual void StartScript(CharacterManager manager)
        {
        }
    }
}