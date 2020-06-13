using UnityEngine;

namespace My_Utils.Examples
{
    [System.Serializable]
    public class PosData : Data
    {
        public float[] pos;

        public PosData(Vector3 vector3)
        {
            pos = vector3.ToFloatArray();
        }
    }
}