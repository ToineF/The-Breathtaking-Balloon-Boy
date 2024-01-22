using UnityEngine;


namespace Character.States
{

    public class CharacterIdleState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("Hello World");

        }

        public override void ExitState(CharacterStatesManager manager)
        {
        }

        public override void UpdateState(CharacterStatesManager manager)
        {
        }
    }

}