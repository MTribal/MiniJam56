using System.Collections.Generic;
using UnityEngine;

namespace My_Utils.PathFinding
{
    [System.Serializable]
    public class PlatformerEdge<TPos>
    {
        public PlatformerNode from;
        public PlatformerNode to;
        public TPos fromPosition;
        public TPos toPosition;
        public List<PlatformerFrame> frames;
        public float wheight;

        public PlatformerEdge(PlatformerNode from, TPos fromPos, PlatformerNode to, TPos toPos, List<PlatformerFrame> frames)
        {
            this.from = from;
            this.to = to;
            this.frames = frames;
            toPosition = toPos;
            fromPosition = fromPos;
            wheight = frames.Count * Time.fixedDeltaTime;
        }

        public bool IsSameEdge(PlatformerEdge<TPos> other)
        {
            return other != null && other.from.Equals(from) && other.to.Equals(to);
        }

        public override string ToString()
        {
            string fromSection = from.section ? "T" : "F";
            string toSection = to.section ? "T" : "F";

            return $"({from.platformId}{fromSection}, {to.platformId}{toSection})";
        }
    }
}
