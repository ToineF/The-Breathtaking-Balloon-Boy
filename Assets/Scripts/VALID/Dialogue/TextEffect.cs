using System;
using UnityEngine;

namespace BlownAway.Cutscenes
{
    [Serializable]
    public struct TextEffect
    {
        public enum LimitRangeByTypeWriter
        {
            FALSE = 0,
            MIN = 1,
            MAX = 2,
        }

        public int MinRange;
        public int MaxRange;
        public Color Color;
        public Func<Vector3, Vector3> GetText;
        public LimitRangeByTypeWriter TypeWriterRange;

        public TextEffect(Func<Vector3, Vector3> getText, Color color, int minRange = 0, int maxRange = -1, LimitRangeByTypeWriter typewriterRange = LimitRangeByTypeWriter.FALSE)
        {
            MinRange = minRange;
            MaxRange = maxRange;
            Color = color;
            GetText = getText;
            TypeWriterRange = typewriterRange;
        }

    }

    [Serializable]
    public struct DisplacementParams
    {
        public enum FunctionUsed
        {
            COS = 0,
            SIN = 1,
            TAN = 2,
            NONE = 3,
        }

        public FunctionUsed Function;
        public float TimeAmount;
        public Vector3 AddedOriginAmount;
        public float GlobalMultiplier;

        public DisplacementParams(FunctionUsed function, float time, Vector3 originAdd, float globalMultiply)
        {
            Function = function;
            TimeAmount = time;
            AddedOriginAmount = originAdd;
            GlobalMultiplier = globalMultiply;
        }

        public float GetTotalValue(Vector3 origin)
        {
            float addedOrigin = origin.x * AddedOriginAmount.x + origin.y * AddedOriginAmount.y + origin.z * AddedOriginAmount.z;

            float value = Time.time * TimeAmount;
            value += addedOrigin;

            if (Function == FunctionUsed.COS) value = Mathf.Cos(value);
            else if (Function == FunctionUsed.SIN) value = Mathf.Sin(value);
            else if (Function == FunctionUsed.TAN) value = Mathf.Tan(value);

            value *= GlobalMultiplier;

            return value;
        }
    }

    [Serializable]
    public struct Vector3Displacement
    {
        public DisplacementParams x;
        public DisplacementParams y;
        public DisplacementParams z;

        public Vector3Displacement(DisplacementParams x, DisplacementParams y, DisplacementParams z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3 GetTotalFunction(Vector3 origin)
        {
            return new Vector3(x.GetTotalValue(origin), y.GetTotalValue(origin), z.GetTotalValue(origin));
        }
    }
}
