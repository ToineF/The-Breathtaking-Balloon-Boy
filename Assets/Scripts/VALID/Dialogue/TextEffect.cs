using System;
using UnityEngine;

namespace BlownAway.Cutscenes
{
    [Serializable]
    struct TextEffect
    {
        public TextEffect(Func<Vector3, Vector3> getText, Color color, Vector3 scale, int minRange = 0, int maxRange = -1, LimitRangeByTypeWriter typewriterRange = LimitRangeByTypeWriter.FALSE)
        {
            MinRange = minRange;
            MaxRange = maxRange;
            Color = color;
            Scale = scale;
            GetText = getText;
            TypeWriterRange = typewriterRange;
        }

        public int MinRange;
        public int MaxRange;
        public Color Color;
        public Vector3 Scale;
        public Func<Vector3, Vector3> GetText;
        public LimitRangeByTypeWriter TypeWriterRange;
        public enum LimitRangeByTypeWriter
        {
            FALSE = 0,
            MIN = 1,
            MAX = 2,
        }
    }
}
