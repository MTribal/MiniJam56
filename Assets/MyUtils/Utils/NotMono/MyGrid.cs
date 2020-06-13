using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

namespace My_Utils
{
    /// <summary>
    /// Corner - The left bottom corner of the cell.
    /// Center - The center of the cell.
    /// </summary>
    public enum CellWorldPositionType { Corner, Center };

    [System.Serializable]
    public class MyGrid<T>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float CellSize { get; set; }
        public float[] OriginPosition { get; private set; }

        private readonly T[,] grid;

        public MyGrid(int width, int height, float cellSize, Vector2 originPosition)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            OriginPosition = new float[] { originPosition.x, originPosition.y };

            grid = new T[width, height];
        }

        /// <summary>
        /// Return the world position equivalent to a grid position.
        /// </summary>
        public Vector2 GetWorldPosition(int2 gridPosition, CellWorldPositionType cellWorldPositionType = CellWorldPositionType.Corner)
        {
            return GetWorldPosition(gridPosition.x, gridPosition.y, cellWorldPositionType);
        }

        /// <summary>
        /// Return the world position equivalent to a grid position.
        /// </summary>
        public Vector2 GetWorldPosition(int x, int y, CellWorldPositionType cellWorldPositionType = CellWorldPositionType.Corner)
        {
            if (cellWorldPositionType == CellWorldPositionType.Corner)
            {
                return new Vector2(x * CellSize, y * CellSize) + OriginPosition.ToVector2();
            }
            else // CellWorldPositionType.Center
            {
                float centerBiasX = Width % 2 == 0 ? CellSize / 2 : 0;
                float centerBiasY = Height % 2 == 0 ? CellSize / 2 : 0;
                Vector2 centerBias = new Vector2(centerBiasX, centerBiasY);

                return new Vector2(x * CellSize, y * CellSize) + OriginPosition.ToVector2() + centerBias;
            }
        }

        /// <summary>
        /// Return the equivalent grid position from a world position. (Not necesseraily valid).
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public int2 GetGridPosition(Vector2 worldPosition, CellWorldPositionType cellWorldPositionType = CellWorldPositionType.Corner)
        {
            if (cellWorldPositionType == CellWorldPositionType.Corner)
            {
                return new int2(Mathf.RoundToInt((worldPosition.x - OriginPosition[0]) / CellSize), Mathf.RoundToInt((worldPosition.y - OriginPosition[1]) / CellSize));
            }
            else // CellWorldPositionType.Center
            {
                float centerBiasX = Width % 2 == 0 ? CellSize / 2 : 0;
                float centerBiasY = Height % 2 == 0 ? CellSize / 2 : 0;

                return new int2(Mathf.RoundToInt((worldPosition.x - OriginPosition[0] - centerBiasX) / CellSize),
                                Mathf.RoundToInt((worldPosition.y - OriginPosition[1]- centerBiasY) / CellSize));
            }
        }

        public T GetValue(int x, int y)
        {
            if (IsInsideGrid(x, y))
            {
                return grid[x, y];
            }
            return default;
        }

        public T GetValue(Vector2 worldPos, CellWorldPositionType cellWorldPositionType = CellWorldPositionType.Corner)
        {
            int2 gridPos = GetGridPosition(worldPos, cellWorldPositionType);
            return GetValue(gridPos.x, gridPos.y);
        }

        public T GetValue(int2 position)
        {
            return GetValue(position.x, position.y);
        }

        public void SetValue(int x, int y, T value)
        {
            if (IsInsideGrid(x, y))
            {
                grid[x, y] = value;
            }
        }

        public void SetValue(int2 pos, T value)
        {
            SetValue(pos.x, pos.y, value);
        }

        public void SetValue(Vector2 worldPos, T value, CellWorldPositionType cellWorldPositionType = CellWorldPositionType.Corner)
        {
            int2 gridPosition = GetGridPosition(worldPos, cellWorldPositionType);
            SetValue(gridPosition.x, gridPosition.y, value);
        }

        public bool IsInsideGrid(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool IsInsideGrid(int2 gridPosition)
        {
            return IsInsideGrid(gridPosition.x, gridPosition.y);
        }

        public bool IsInsideGrid(Vector2 worldPosition, CellWorldPositionType cellWorldPositionType)
        {
            int2 gridPos = GetGridPosition(worldPosition, cellWorldPositionType);
            return IsInsideGrid(gridPos.x, gridPos.y);
        }

        public void SetOriginPosition(Vector2 originPosition)
        {
            OriginPosition = originPosition.ToFloatArray();
        }

        public List<T> GetValuesInList()
        {
            List<T> values = new List<T>();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    values.Add(grid[i, j]);
                }
            }

            return values;
        }
    }
}
