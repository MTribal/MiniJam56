namespace My_Utils.PathFinding
{
    [System.Serializable]
    public class TopdownData : Data
    {
        public bool drawGrid, drawLines, editMode;
        public int obstaclesLayerMask;
        public float tolerance;
        public MyGrid<GridPathNode> grid;

        public TopdownData(bool drawGrid, bool drawLines, bool editMode, float tolerance, int layerMask, MyGrid<GridPathNode> topdownGrid)
        {
            this.drawGrid = drawGrid;
            this.drawLines = drawLines;
            this.editMode = editMode;
            this.tolerance = tolerance;
            obstaclesLayerMask = layerMask;
            grid = topdownGrid;
        }
    }
}
