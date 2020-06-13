using UnityEngine;

namespace My_Utils.PathFinding
{
    /// <summary>
    /// use to generate an inspector popup of the graph data keys in the game.
    /// </summary>
    public class DataKeyAttribute : PropertyAttribute
    {
        public DataKeyType KeyType { get; private set; }

        public DataKeyAttribute(DataKeyType keyType)
        {
            this.KeyType = keyType;
        }
    }
}
