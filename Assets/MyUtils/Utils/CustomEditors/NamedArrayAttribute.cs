using System;
using UnityEngine;

namespace My_Utils
{
    public class NamedArrayAttribute : PropertyAttribute
    {
        public readonly string name;

        public NamedArrayAttribute(string name)
        {
            this.name = name;
        }
    }
}