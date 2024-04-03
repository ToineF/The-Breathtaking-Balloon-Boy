using BlownAway.Character;
using UnityEngine;

namespace BlownAway.Cutscenes
{
    public class CutsceneManager : CharacterSubComponent
    {
        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private CutsceneCameraManager _cameraManager;
        [SerializeField] private CutsceneWaitForTimeManager _waitForTimeManager;

        public int CurrentSequenceIndex
        {
            get => _currentSequenceIndex; private set
            {
                if (_currentInteractionSequence == null) return;

                _currentSequenceIndex = Mathf.Clamp(value, 0, _currentInteractionSequence.SequenceElements.List.Count - 1);

                if (value < 0 || value > _currentInteractionSequence.SequenceElements.List.Count - 1) EndSequence();
                else ReadSequenceElement();
            }
        }

        private Cutscene _currentInteractionSequence;
        private int _currentSequenceIndex;

        public void StartNewSequence(Cutscene cutscene)
        {
            //if (MainGame.Instance.Player)
            //    MainGame.Instance.Player.CanMove = false;

            _currentInteractionSequence = cutscene;
            CurrentSequenceIndex = 0;
        }

        private void GoToSequenceAt(int index)
        {
            CurrentSequenceIndex = index;
        }

        public void GoToNextSequenceElement()
        {
            GoToSequenceAt(CurrentSequenceIndex + 1);
        }

        private void EndSequence()
        {
            //if (MainGame.Instance.Player)
            //    MainGame.Instance.Player.CanMove = true;
        }

        private void ReadSequenceElement()
        {
            CutsceneElement interactionElement = _currentInteractionSequence.SequenceElements.List[CurrentSequenceIndex];

            CutsceneDialogue dialogue = interactionElement as CutsceneDialogue;
            if (dialogue != null)
            {
                ReadDialogue(dialogue);
            }

            CutsceneCamera camera = interactionElement as CutsceneCamera;
            if (camera != null)
            {
                StartCameraTravelling(camera);
            }

            CutsceneWaitForTime waitForTime = interactionElement as CutsceneWaitForTime;
            if (waitForTime != null)
            {
                StartWaitForTime(waitForTime);
            }
        }

        private void ReadDialogue(CutsceneDialogue dialogue)
        {
            _dialogueManager.DialogueUI.alpha = 1;
            _dialogueManager.SetNewDialogue(dialogue.Dialogue);
            _dialogueManager.OnDialogueEnd += EndDialogue;
            if (dialogue.CloseWindowOnDialogueEnd) _dialogueManager.OnDialogueEnd += HideDialogueWindow;
        }

        private void HideDialogueWindow()
        {
            _dialogueManager.DialogueUI.alpha = 0;
        }

        private void EndDialogue()
        {
            _dialogueManager.OnDialogueEnd -= EndDialogue;
            GoToNextSequenceElement();
        }

        private void StartCameraTravelling(CutsceneCamera camera)
        {
            _cameraManager.SetNewCamera(camera);

            GoToNextSequenceElement();
        }

        private void StartWaitForTime(CutsceneWaitForTime waitForTime)
        {
            _waitForTimeManager.StartTimer(waitForTime.WaitTime);
            _waitForTimeManager.OnTimerEnd += EndWaitForTime;

        }

        private void EndWaitForTime()
        {
            _waitForTimeManager.OnTimerEnd -= EndWaitForTime;
            GoToNextSequenceElement();
        }
    }
}