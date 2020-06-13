namespace My_Utils.Collections
{
    [System.Serializable]
    /// <summary>
    /// Just a float2 serializable
    /// </summary>
    public struct FloatVector
    {
        public readonly float x;
        public readonly float y;

        public FloatVector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
