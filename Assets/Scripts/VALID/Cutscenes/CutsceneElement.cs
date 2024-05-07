using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace BlownAway.Cutscenes
{
    [Serializable]
    public class CutsceneElement { }

    public class CutsceneDialogue : CutsceneElement
    {
        [field: SerializeField] public Dialogue Dialogue { get; private set; }
        [field: SerializeField] public bool CloseWindowOnDialogueEnd { get; private set; } = true;
    }

    public class CutsceneCamera : CutsceneElement
    {
        [field: SerializeField] public Cinemachine.CinemachineVirtualCamera TargetTransform { get; private set; }

    }
    public class CutsceneWaitForTime : CutsceneElement
    {
        [field: SerializeField] public float WaitTime { get; private set; }
    }
    public class CutsceneMoveObject : CutsceneElement
    {
        [field: SerializeField] public GameObject ObjectToMove { get; private set; }
        [field: SerializeField] public Transform TargetPosition { get; private set; }
        [field: SerializeField] public bool SetParent { get; private set; } = false;
    }
    public class CutsceneInvokeFunction : CutsceneElement
    {
        [field: SerializeField] public UnityEvent Event { get; private set; }
    }

    public class CutsceneTimeline : CutsceneElement
    {
        [field: SerializeField] public PlayableDirector Director { get; private set; }
    }

    [Serializable]
    public class CutsceneElementWrapper
    {
        [SerializeReference] public List<CutsceneElement> List;
    }
}