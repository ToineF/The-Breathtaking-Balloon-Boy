using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterCutsceneState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("CUTSCENE");
            manager.CameraManager.IsMovable = false;
            manager.UIManager.ShowUI(false);

            manager.MovementManager.SnapToGround(manager);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.CameraManager.IsMovable = true;
            manager.CameraManager.ReactivateCamera();
            manager.UIManager.ShowUI(true);
        }

        public override void UpdateState(CharacterManager manager)
        {
            if (manager.Inputs.NextDialoguePressed)
                manager.CutsceneManager.DialogueManager.GoToNextText();
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
