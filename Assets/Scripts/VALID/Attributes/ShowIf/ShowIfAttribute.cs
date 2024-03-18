using System;
using UnityEngine;

namespace BlownAway.Attributes.ShowIf
{

    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string FieldName;
        public readonly int FieldValue;

        public ShowIfAttribute() { }

        public ShowIfAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public ShowIfAttribute(string fieldName, object fieldValue)
        {
            FieldName = fieldName;
            FieldValue = (int)fieldValue;
        }
    }
}