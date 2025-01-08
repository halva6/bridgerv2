using System;
using UnityEngine;

public class MatrixVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject greenPierPrefab;
    [SerializeField] private GameObject redPierPrefab;
    [SerializeField] private GameObject greenBridgePrefab;
    [SerializeField] private GameObject redBridgePrefab;
    [SerializeField] private GameObject wallPrefab;

    [SerializeField] private float cellSize = 1.0f;

    private int[,] matrix;

    public void VisualizeMatrix(int[,] inputMatrix)
    {
        matrix = inputMatrix;
        Vector2 offset = CalculateMatrixOffset(matrix);

        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                SpawnCell(row, col, offset);
            }
        }
    }

    private Vector2 CalculateMatrixOffset(int[,] matrix)
    {
        float offsetX = -matrix.GetLength(1) * cellSize / 2 + cellSize / 2;
        float offsetY = matrix.GetLength(0) * cellSize / 2 - cellSize / 2;
        return new Vector2(offsetX, offsetY);
    }

    private void SpawnCell(int row, int col, Vector2 offset)
    {
        int cellValue = matrix[row, col];
        Vector3 position = new Vector3(col * cellSize + offset.x, -row * cellSize + offset.y, 0);
        GameObject cellPrefab = GetPrefabForCellValue(cellValue);

        if (cellPrefab != null)
        {
            Quaternion rotation = GetRotationForCell(row, col, cellValue);
            GameObject instance = Instantiate(cellPrefab, position, rotation, transform);
            AttachMetadata(instance, row, col, cellValue);
        }
    }

    private GameObject GetPrefabForCellValue(int value)
    {
        return value switch
        {
            -1 => wallPrefab,
            1 => greenPierPrefab,
            2 => redPierPrefab,
            3 => greenBridgePrefab,
            4 => redBridgePrefab,
            _ => null,
        };
    }

    private Quaternion GetRotationForCell(int row, int col, int value)
    {
        if (value == 3 || value == 4)
        {
            int targetValue = value == 3 ? 1 : 2;
            if (ShouldRotate(row, col, targetValue))
            {
                return Quaternion.Euler(0, 0, 90);
            }
        }
        else if (value == -1 && (col == 0 || col == matrix.GetLength(1) - 1))
        {
            return Quaternion.Euler(0, 0, 90);
        }

        return Quaternion.identity;
    }

    private bool ShouldRotate(int row, int col, int targetValue)
    {
        return (row > 0 && matrix[row - 1, col] == targetValue) ||
               (row < matrix.GetLength(0) - 1 && matrix[row + 1, col] == targetValue);
    }

    private void AttachMetadata(GameObject instance, int row, int col, int value)
    {
        if (value == 1 || value == 2)
        {
            var pierData = instance.AddComponent<PierMetadata>();
            pierData.Row = row;
            pierData.Column = col;
            pierData.Type = value == 1 ? "GreenPier" : "RedPier";
        }
    }
}

public class PierMetadata : MonoBehaviour
{
    public int Row { get; set; }
    public int Column { get; set; }
    public string Type { get; set; }
}
