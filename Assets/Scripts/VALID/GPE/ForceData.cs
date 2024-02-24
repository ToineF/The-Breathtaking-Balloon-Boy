using System;
using UnityEngine;

namespace BlownAway.GPE
{
    [Serializable]
    public class ForceData
    {
        public ForceData(Vector3 targetForce, float forceLerp)
        {
            CurrentForce = Vector3.zero;
            TargetForce = targetForce;
            ForceLerp = forceLerp;
        }
        public Vector3 CurrentForce;
        public Vector3 TargetForce;
        [Range(0, 1)] public float ForceLerp;

    }
}