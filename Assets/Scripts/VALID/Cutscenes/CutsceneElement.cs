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
    }

    public class CutsceneCamera : CutsceneElement
    {
        [field: SerializeField] public Transform TargetTransform { get; private set; }
        [field:SerializeField] public float TravellingTime { get; private set; }
        [field:SerializeField] public DG.Tweening.Ease TravellingEase { get; private set; }

}
    public class CutsceneWaitForTime : CutsceneElement
    {
        [field:SerializeField] public float WaitTIme { get; private set; }
}

    [Serializable]
    public class CutsceneElementWrapper
    {
        [SerializeReference] public List<CutsceneElement> List;
    }
}