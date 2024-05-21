using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterCutsceneState : CharacterBaseState
    {
        public enum CutsceneState
        {
            BEFORE = 0,
            DURING = 1,
        }

        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("CUTSCENE");
            manager.UIManager.ShowUI(false);

            manager.MovementManager.CurrentCutsceneState = CutsceneState.BEFORE;

            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.BaseData);

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

            manager.CutsceneManager.SetSkipCutscene(manager.Inputs.SkipCutscene);
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.CheckIfGrounded(manager, false, false);
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
