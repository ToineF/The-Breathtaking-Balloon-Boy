#if UNITY_EDITOR
using BF_SubclassList;
using UnityEngine;

namespace BF_SubclassList_Example
{
    public class BF_Example_SO : ScriptableObject
    {
        [SubclassList(typeof(BF_ExampleSuperclass))] public BF_ExampleSuperclassContainer myValues;
    }

}
#endif
