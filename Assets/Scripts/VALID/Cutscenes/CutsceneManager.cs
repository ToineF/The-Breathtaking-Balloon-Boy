using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlownAway.Cutscenes
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private CutsceneCameraManager _cameraManager;

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
                WaitForTime(waitForTime);
            }
        }

        private void ReadDialogue(CutsceneDialogue dialogue)
        {
            _dialogueManager.DialogueUI.alpha = 1;
            _dialogueManager.SetNewDialogue(dialogue.Dialogue);
            _dialogueManager.OnDialogueEnd += EndDialogue;
        }

        private void EndDialogue()
        {
            _dialogueManager.DialogueUI.alpha = 0;
            _dialogueManager.OnDialogueEnd -= EndDialogue;
            GoToNextSequenceElement();
        }

        private void StartCameraTravelling(CutsceneCamera camera)
        {
            //_dialogueManager.OnDialogueEnd += EndCameraTravelling;

        }

        private void EndCameraTravelling()
        {
            GoToNextSequenceElement();
        }

        private void WaitForTime(CutsceneWaitForTime waitForTime)
        {
            //_dialogueManager.OnDialogueEnd += EndWaitForTime;

        }

        private void EndWaitForTime()
        {
            GoToNextSequenceElement();
        }
    }
}