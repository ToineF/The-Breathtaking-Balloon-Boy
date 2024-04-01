using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlownAway.Cutscenes
{
    public class Cutscene : MonoBehaviour
    {
        [field:BF_SubclassList.SubclassList(typeof(CutsceneElement)), SerializeField] public CutsceneElementWrapper SequenceElements { get; private set; }

    }
}
