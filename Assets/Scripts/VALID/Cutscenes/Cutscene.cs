using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlownAway.Cutscenes
{
    public class Cutscene : MonoBehaviour
    {
        [field:BF_SubclassList.SubclassList(typeof(CutsceneElement)), SerializeField] public CutsceneElementWrapper SequenceElements { get; private set; }
        [field:SerializeField] public bool StopPlayerMovements { get; private set; }
        [field:SerializeField] public List<UnityEvent> OnSkipEvents { get; private set; }

    }
}
