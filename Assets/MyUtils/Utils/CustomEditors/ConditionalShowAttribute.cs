using UnityEngine;
using System;

namespace My_Utils
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalShowAttribute : PropertyAttribute
    {
        //The name of the bool field that will be in control
        public string[] ConditionalSources;
        public object[] ObjectsToCompare;

        public ConditionalShowAttribute(string conditionalSource, object objectToCompare)
        {
            ConditionalSources = new string[] { conditionalSource };
            ObjectsToCompare = new object[] { objectToCompare };
        }

        public ConditionalShowAttribute(string conditionalSource, object objectToCompare, string conditionalSource2, object objectToCompare2)
        {
            ConditionalSources = new string[] { conditionalSource, conditionalSource2 };
            ObjectsToCompare = new object[] { objectToCompare, objectToCompare2 };
        }

        public ConditionalShowAttribute(string conditionalSource, object objectToCompare, string conditionalSource2, object objectToCompare2, string conditionalSource3, object objectToCompare3)
        {
            ConditionalSources = new string[] { conditionalSource, conditionalSource2, conditionalSource3 };
            ObjectsToCompare = new object[] { objectToCompare, objectToCompare2, objectToCompare3 };
        }

        public ConditionalShowAttribute(string[] conditionalSources, object[] objectsToCompare)
        {
            ConditionalSources = conditionalSources;
            ObjectsToCompare = objectsToCompare;
        }
    }
}