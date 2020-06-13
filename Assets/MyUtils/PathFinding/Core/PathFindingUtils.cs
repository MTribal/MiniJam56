using System.Collections.Generic;
using UnityEngine;

namespace My_Utils.PathFinding
{
    public static class PathFindingUtils
    {
        /// <summary>
        /// Convert a PlatformGraph'Vector2' in a PlatformGraph'float[2]'.
        /// </summary>
        /// <param name="vectorGraph">The graph that you want to convert.</param>
        /// <returns></returns>
        public static PlatformerGraph<float[]> VectorGraphToFloatGraph(PlatformerGraph<Vector2> vectorGraph)
        {
            if (vectorGraph == null || vectorGraph.AdjacencyList == null)
                return null;

            PlatformerGraph<float[]> floatGraph = new PlatformerGraph<float[]>();

            foreach (PlatformerNode vertex in vectorGraph.AdjacencyList.Keys)
            {
                floatGraph.AddVertex(vertex);
            }

            foreach (List<PlatformerEdge<Vector2>> edgesList in vectorGraph.AdjacencyList.Values)
            {
                foreach (PlatformerEdge<Vector2> edge in edgesList)
                {
                    PlatformerEdge<float[]> floatEdge = new PlatformerEdge<float[]>(edge.from, edge.fromPosition.ToFloatArray(),
                                                                                edge.to, edge.toPosition.ToFloatArray(), edge.frames);
                    floatGraph.AddEdge(floatEdge, RecordType.IfNotExist);
                }
            }

            return floatGraph;
        }


        /// <summary>
        /// Convert a PlatformGraph'float[2]' in a PlatformGraph'Vector2'.
        /// </summary>
        /// <param name="floatGraph">The graph that you want to convert.</param>
        /// <returns></returns>
        public static PlatformerGraph<Vector2> FloatGraphToVectorGraph(PlatformerGraph<float[]> floatGraph)
        {
            if (floatGraph == null || floatGraph.AdjacencyList == null)
                return null;

            PlatformerGraph<Vector2> vectorGraph = new PlatformerGraph<Vector2>();

            foreach (PlatformerNode vertex in floatGraph.AdjacencyList.Keys)
            {
                vectorGraph.AddVertex(vertex);
            }

            foreach (List<PlatformerEdge<float[]>> edgesList in floatGraph.AdjacencyList.Values)
            {
                foreach (PlatformerEdge<float[]> edge in edgesList)
                {
                    PlatformerEdge<Vector2> vectorEdge = new PlatformerEdge<Vector2>(edge.from, edge.fromPosition.ToVector2(), edge.to,
                                                                                  edge.toPosition.ToVector2(), edge.frames);
                   
                    vectorGraph.AddEdge(vectorEdge, RecordType.IfNotExist);
                }
            }

            return vectorGraph;
        }
    }
}