using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PresentMatrix : MonoBehaviour
{
    // GameObjects für die verschiedenen Werte
    [SerializeField] private GameObject greenPier;
    [SerializeField] private GameObject redPier;
    [SerializeField] private GameObject greenBridge;
    [SerializeField] private GameObject redBridge;
    [SerializeField] private GameObject wall;

    // Die Matrix
    private int[,] matrix;

    [SerializeField] private float cellSize = 1.0f;

    public void SpawnMatrix(int[,] matrix)
    {
        this.matrix = matrix;
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        // Berechne den Offset, um die Matrix in die Bildschirmmitte zu setzen
        float offsetX = -cols * cellSize / 2 + cellSize / 2;
        float offsetY = rows * cellSize / 2 - cellSize / 2;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int value = matrix[row, col];
                Vector3 position = new Vector3(col * cellSize + offsetX, -row * cellSize + offsetY, 0);
                GameObject toSpawn = null;
                Quaternion rotation = Quaternion.identity;

                switch (value)
                {
                    case -1:
                        toSpawn = Instantiate(wall, position, Quaternion.identity);
                        if (col == 0 || col == cols - 1)
                        {
                            toSpawn.transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        break;

                    case 1:
                        toSpawn = Instantiate(greenPier, position, Quaternion.identity);
                        addData(toSpawn, col, row, "GreenPier");
                        break;

                    case 2:
                        toSpawn = Instantiate(redPier, position, Quaternion.identity);
                        addData(toSpawn, col, row, "RedPier");
                        break;

                    case 3:
                        rotation = ShouldRotate(row, col, 1) ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
                        toSpawn = Instantiate(greenBridge, position, rotation);
                        break;

                    case 4:
                        rotation = ShouldRotate(row, col, 2) ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
                        toSpawn = Instantiate(redBridge, position, rotation);
                        break;
                }

                if (toSpawn != null)
                {
                    toSpawn.transform.parent = this.transform; // Ordnung im Hierarchy-Fenster
                }
            }
        }
    }

    void addData(GameObject toSpawn, int col, int row, string type)
    {
        ObjectDataPier data = toSpawn.AddComponent<ObjectDataPier>();
        data.X = col;
        data.Y = row;
        data.Type = type;
    }

    bool ShouldRotate(int row, int col, int target)
    {
        bool rotate = false;

        // Überprüfe die obere Zelle
        if (row > 0 && matrix[row - 1, col] == target)
        {
            rotate = true;
        }

        // Überprüfe die untere Zelle
        if (row < matrix.GetLength(0) - 1 && matrix[row + 1, col] == target)
        {
            rotate = true;
        }

        return rotate;
    }
}
