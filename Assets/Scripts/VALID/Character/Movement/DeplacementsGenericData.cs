using System;
using UnityEngine;

namespace BlownAway.Character.Movements.Data {

    [Serializable]
    public class DeplacementsGenericData
    {
        [field: SerializeField, Tooltip("The maximum propulsion speed the character aims to moves at")] public float TargetValue { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BasePropulsionSpeed) after pressing the inputs")] public float Time { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BasePropulsionSpeed) after pressing the inputs")] public AnimationCurve Curve { get; set; }
        public float BaseValue { get; set; }
        public Action<float> BaseValueUpdate { get; set; }


        public DeplacementsGenericData(float target, float time, AnimationCurve curve)
        {
            this.TargetValue = target;
            this.Time = time;
            this.Curve = curve;
        }

        public DeplacementsGenericData(float target, float time, AnimationCurve curve, ref float value)
        {
            this.TargetValue = target;
            this.Time = time;
            this.Curve = curve;
            this.BaseValue = value;
            this.BaseValueUpdate = ((result) => this.BaseValue = result);
        }
    }
}