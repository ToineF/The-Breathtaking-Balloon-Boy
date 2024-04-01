using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlownAway.Cutscenes
{
    [Serializable]
    public class CutsceneElement { }

    public class CutsceneDialogue : CutsceneElement
    {
        [field:SerializeField] public Dialogue Dialogue { get; private set; }
        [field: SerializeField] public bool CloseWindowOnDialogueEnd { get; private set; } = true;
    }

    public class CutsceneCamera : CutsceneElement
    {
        [field: SerializeField] public Cinemachine.CinemachineVirtualCamera TargetTransform { get; private set; }

}
    public class CutsceneWaitForTime : CutsceneElement
    {
        [field:SerializeField] public float WaitTime { get; private set; }
}

    [Serializable]
    public class CutsceneElementWrapper
    {
        [SerializeReference] public List<CutsceneElement> List;
    }
}