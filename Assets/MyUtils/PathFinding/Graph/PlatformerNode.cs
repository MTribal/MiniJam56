namespace My_Utils.PathFinding
{
    [System.Serializable]
    public struct PlatformerNode
    {
        public int platformId;
        public bool section; // Represents wich part of the Platform this is. A or B, true of false...

        public PlatformerNode(int platformId, bool section)
        {
            this.platformId = platformId;
            this.section = section;
        }

        public bool IsSameSection(PlatformerNode otherNode)
        {
            return otherNode.platformId == platformId && otherNode.section == section;
        }

        public bool IsSamePlatform(PlatformerNode otherNode)
        {
            return otherNode.platformId == platformId;
        }

        /// <summary>
        /// Return the another section of this Section. Ex: Node{id: 10, section: false} => Node{id: 10, section: true}
        /// </summary>
        /// <returns></returns>
        public PlatformerNode GetOtherSection()
        {
            return new PlatformerNode(platformId, !section);
        }

        public override string ToString()
        {
            string sectionStr = section ? "T" : "F";

            return platformId + sectionStr;
        }

        public override int GetHashCode()
        {
            if (section)
                return platformId * 2 + 1;
            else
                return platformId * 2;
        }
    }
}
