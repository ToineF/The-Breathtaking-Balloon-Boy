using BlownAway.Character;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BlownAway.Cutscenes
{
    public class CutsceneManager : CharacterSubComponent
    {
        public DialogueManager DialogueManager { get => _dialogueManager; set => _dialogueManager = value; }

        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private CutsceneCameraManager _cameraManager;
        [SerializeField] private CutsceneWaitForTimeManager _waitForTimeManager;

        private CharacterManager _character;

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

        public void StartNewSequence(Cutscene cutscene, CharacterManager character)
        {
            _currentInteractionSequence = cutscene;

            if (_currentInteractionSequence.StopPlayerMovements)
            {
                character.States.SwitchState(character.States.CutsceneState);
                character.CameraManager.IsMovable = false;
            }
            _character = character;

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
            _character.States.SwitchState(_character.States.FallingState);
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

            CutsceneMoveObject moveObject = interactionElement as CutsceneMoveObject;
            if (moveObject != null)
            {
                StartMoveObject(moveObject);
            }

            CutsceneInvokeFunction function = interactionElement as CutsceneInvokeFunction;
            if (function != null)
            {
                StartInvokeMethod(function);
            }

            CutsceneTimeline timeline = interactionElement as CutsceneTimeline;
            if (timeline != null)
            {
                StartTimeline(timeline);
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

        private void StartMoveObject(CutsceneMoveObject moveObject)
        {
            moveObject.ObjectToMove.transform.position = moveObject.TargetPosition.position;
            if (moveObject.SetParent) moveObject.ObjectToMove.transform.SetParent(moveObject.TargetPosition.transform);
            GoToNextSequenceElement();
        }

        private void StartInvokeMethod(CutsceneInvokeFunction function)
        {
            function.Event?.Invoke();
            GoToNextSequenceElement();
        }

        private void StartTimeline(CutsceneTimeline timeline)
        {
            timeline.Director.Play();
            timeline.Director.stopped += EndTimeline;
            Debug.Log("start timeline");
        }
        private void EndTimeline(PlayableDirector director)
        {
            Debug.Log("end timeline");
            director.stopped -= EndTimeline;
            _dialogueManager.OnDialogueEnd -= EndDialogue;
            GoToNextSequenceElement();
        }
    }
}