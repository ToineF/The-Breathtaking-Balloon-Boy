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
        public ColorSquare Colors;
        public LimitRangeByTypeWriter TypeWriterRange;
        public float CharacterApparitionTime;

        [Header("Typewriter Params")]
        public float ScaleTime;
        public Vector3AnimationCurve ScaleAnimations;

        public TextEffect(ColorSquare colors, float apparitionTime, float scaleTime, Vector3AnimationCurve scaleAnimations, int minRange = 0, int maxRange = -1, LimitRangeByTypeWriter typewriterRange = LimitRangeByTypeWriter.FALSE)
        {
            MinRange = minRange;
            MaxRange = maxRange;
            Colors = colors;
            TypeWriterRange = typewriterRange;
            CharacterApparitionTime = apparitionTime;
            ScaleTime = scaleTime;
            ScaleAnimations = scaleAnimations;
        }

        public Vector3 GetCurrentScale(float timer)
        {
            float percentile = timer / ScaleTime;
            float x = ScaleAnimations.x.Evaluate(percentile);
            float y = ScaleAnimations.y.Evaluate(percentile);
            float z = ScaleAnimations.z.Evaluate(percentile);
            Vector3 targetScale = new Vector3(x, y, z);
            return targetScale;
        }

        public Vector3 ReturnScaledPosition(Vector3 position, Vector3 scale, Vector3 center)
        {
            Vector3 direction = (position - center);
            return Vector3.Scale(direction, scale);
        }

        public Vector3 ReturnAddedScaledPosition(Vector3 position, Vector3 scale, Vector3 center)
        {
            Vector3 direction = (position - center);
            return Vector3.Scale(direction, scale - Vector3.one);
        }

    }

    [Serializable]
    public struct DisplacementParams
    {
        public enum MathFunction
        {
            COS = 0,
            SIN = 1,
            TAN = 2,
            NONE = 3,
        }

        [Header("Base Params")]
        public MathFunction Function;
        public float TimeAmount;
        public Vector3 AddedOriginAmount;
        public Vector3MinMaxRange AddedRandomAmount;
        public float GlobalMultiplier;

        public DisplacementParams(MathFunction function, float time, Vector3 originAdd, float globalMultiply, Vector3MinMaxRange randomAdd)
        {
            Function = function;
            TimeAmount = time;
            AddedOriginAmount = originAdd;
            AddedRandomAmount = randomAdd;
            GlobalMultiplier = globalMultiply;
        }

        public float GetTotalValue(Vector3 origin)
        {
            float addedOrigin = origin.x * AddedOriginAmount.x + origin.y * AddedOriginAmount.y + origin.z * AddedOriginAmount.z;
            Vector3 randomRange = AddedRandomAmount.GetRandomRange();
            float addedRandom = randomRange.x + randomRange.y + randomRange.z;

            float value = Time.time * TimeAmount;
            value += addedOrigin;
            value += addedRandom;
            
            if (Function == MathFunction.COS) value = Mathf.Cos(value);
            else if (Function == MathFunction.SIN) value = Mathf.Sin(value);
            else if (Function == MathFunction.TAN) value = Mathf.Tan(value);

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

    [Serializable]
    public struct Vector3MinMaxRange
    {
        public Vector3 Min;
        public Vector3 Max;

        public Vector3 GetRandomRange()
        {
            return new Vector3(UnityEngine.Random.Range(Min.x, Max.x), UnityEngine.Random.Range(Min.y, Max.y), UnityEngine.Random.Range(Min.z, Max.z));
        }
    }

    [Serializable]
    public struct ColorSquare
    {
        public Color TopLeft;
        public Color TopRight;
        public Color BottomLeft;
        public Color BottomRight;

        public Color this[int i]
        {
            get {
                if (i == 1) return TopLeft;
                else if (i == 2) return TopRight;
                else if (i == 3) return BottomRight;
                else return BottomLeft;
            }
        }
    }

    [Serializable]
    public struct Vector3AnimationCurve
    {
        public AnimationCurve x;
        public AnimationCurve y;
        public AnimationCurve z;
    }
}