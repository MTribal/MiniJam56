using UnityEngine;

namespace My_Utils.PathFinding
{
    [System.Serializable]
    public class GraphData : Data
    {
        public readonly PlatformerGraph<float[]> serializableGraph;

        /// <summary>
        /// Instantiate a serializable graph from a Vector2 graph
        /// </summary>
        /// <param name="graph"></param>
        public GraphData(PlatformerGraph<Vector2> graph)
        {
            serializableGraph = PathFindingUtils.VectorGraphToFloatGraph(graph);
        }

        /// <summary>
        /// Return the saved graph.
        /// </summary>
        public PlatformerGraph<Vector2> Load()
        {
            return PathFindingUtils.FloatGraphToVectorGraph(serializableGraph);
        }
    }
}
