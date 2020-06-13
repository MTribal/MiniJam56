using System;
using System.Collections.Generic;

namespace My_Utils.PathFinding
{
    public enum AddType { CreateNewVertex, UseExistentVertex };

    [Serializable]
    public class PlatformerGraph<TPos>
    {
        public Dictionary<PlatformerNode, List<PlatformerEdge<TPos>>> AdjacencyList { get; private set; }

        /// <summary>
        /// The quantity of platforms (different IDs) in the graph.
        /// </summary>
        public int PlatformsCount { get; private set; }

        /// <summary>
        /// The quantity of edges in the graph.
        /// </summary>
        public int EdgesCount { get; private set; }

        /// <summary>
        /// The quantity of vertices in the graph.
        /// </summary>
        public int VerticesCount 
        {
            get
            {
                return AdjacencyList.Count;
            }
        }

        public PlatformerGraph()
        {
            AdjacencyList = new Dictionary<PlatformerNode, List<PlatformerEdge<TPos>>>();
        }


        /// <summary>
        /// Add a vertex to the graph if the vertex not already exist.
        /// </summary>
        /// <param name="vertex">The vertex that you want to add.</param>
        public void AddVertex(PlatformerNode vertex)
        {
            if (!AdjacencyList.ContainsKey(vertex))
            {
                AdjacencyList[vertex] = new List<PlatformerEdge<TPos>>();
                if (!AdjacencyList.ContainsKey(vertex.GetOtherSection()))
                    PlatformsCount++;
            }
        }


        /// <summary>
        /// Add an edge to the graph.
        /// </summary>
        /// <param name="edgeToAdd">The edge that you want to add.</param>
        /// <param name="addType">UseExistentVertex if just want to add a new edge in the existent vertices.
        /// CreateNewVertex if want to create new vertices if necessary.</param>
        public void AddEdge(PlatformerEdge<TPos> edgeToAdd, RecordType recordType, AddType addType = AddType.UseExistentVertex)
        {
            PlatformerNode vertexFrom = edgeToAdd.from;
            PlatformerNode vertexTo = edgeToAdd.to;

            if (!AdjacencyList.ContainsKey(vertexFrom))
            {
                if (addType == AddType.CreateNewVertex)
                {
                    AddVertex(vertexFrom);
                }
                else
                {
                    throw new Exception($"Graph not contains key {vertexFrom}");
                }
            }
            else if (!AdjacencyList.ContainsKey(vertexTo))
            {
                if (addType == AddType.CreateNewVertex)
                {
                    AddVertex(vertexTo);
                }
                else
                {
                    throw new Exception($"Graph not contains key {vertexTo}");
                }
            }
            if (ExistEdge(edgeToAdd))
            {
                switch (recordType)
                {
                    case RecordType.IfNotExist:
                        return;

                    case RecordType.Override:
                        RemoveEdge(edgeToAdd);
                        break;

                    case RecordType.Better:
                        PlatformerEdge<TPos> edgeToCompare = FindEdge(edgeToAdd.from, edgeToAdd.to);
                        if (edgeToCompare == null)
                        {
                            PlatformerNode toVertex = new PlatformerNode(edgeToAdd.to.platformId, !edgeToAdd.to.section);
                            edgeToCompare = FindEdge(edgeToAdd.from, toVertex);
                        }

                        if (edgeToAdd.wheight >= edgeToCompare.wheight)
                            return;
                        else
                            RemoveEdge(edgeToAdd);
                        break;
                }
            }

            AdjacencyList[vertexFrom].Add(edgeToAdd);
            EdgesCount++;
        }


        /// <summary>
        /// Remove an edge from the graph. Return false if edge not found.
        /// </summary>
        /// <param name="edgeToDelete">The edge that you want to delete.</param>
        public bool RemoveEdge(PlatformerEdge<TPos> edgeToDelete)
        {
            PlatformerNode fromVertex = edgeToDelete.from;
            PlatformerNode toVertex = edgeToDelete.to;

            if (!AdjacencyList.ContainsKey(fromVertex) || !AdjacencyList.ContainsKey(toVertex))
                return false;

            List<PlatformerEdge<TPos>> edges = AdjacencyList[fromVertex];
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].Equals(edgeToDelete))
                {
                    edges.RemoveAt(i);
                    EdgesCount--;
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Remove all edges of the graph.
        /// </summary>
        public void ClearEdges()
        {
            foreach (PlatformerNode vertex in AdjacencyList.Keys)
            {
                AdjacencyList[vertex].RemoveAll(edge => true);
            }
            EdgesCount = 0;
        }


        /// <summary>
        /// Return a list of all edges in the graph.
        /// </summary>
        public List<PlatformerEdge<TPos>> GetAllEdges()
        {
            List<PlatformerEdge<TPos>> allEdges = new List<PlatformerEdge<TPos>>();

            foreach (List<PlatformerEdge<TPos>> edgesList in AdjacencyList.Values)
            {
                allEdges.AddRange(edgesList);
            }

            return allEdges;
        }


        /// <summary>
        /// Return if a vertex already exist in the graph.
        /// </summary>
        /// <param name="vertex">The vertex that you want to test.</param>
        /// <returns></returns>
        public bool ExistVertex(PlatformerNode vertex)
        {
            return AdjacencyList.ContainsKey(vertex);
        }


        /// <summary>
        /// Return if an edge exist in the graph.
        /// </summary>
        /// <param name="edge">The edge that you want to test.</param>
        /// <returns></returns>
        public bool ExistEdge(PlatformerEdge<TPos> edge)
        {
            PlatformerNode key = edge.from;
            if (!AdjacencyList.ContainsKey(key))
                return false;

            foreach (PlatformerEdge<TPos> adjacentEdge in AdjacencyList[key])
            {
                if (adjacentEdge.to.IsSamePlatform(edge.to))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Return a specific edge in the graph. Return null if not exist.
        /// </summary>
        public PlatformerEdge<TPos> FindEdge(PlatformerNode fromVertex, PlatformerNode toVertex)
        {
            if (!AdjacencyList.ContainsKey(fromVertex) || !AdjacencyList.ContainsKey(toVertex))
                return null;

            foreach (PlatformerEdge<TPos> edge in AdjacencyList[fromVertex])
            {
                if (edge.to.IsSameSection(toVertex))
                    return edge;
            }

            return null;
        }
    }
}
