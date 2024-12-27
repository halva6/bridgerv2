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

    public float cellSize = 1.0f;

    void Start()
    {
        matrix = gameObject.GetComponent<Matrix>().getMatrix();
        SpawnMatrix(this.matrix);
    }

    void SpawnMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        // Berechne den Offset, um die Matrix in die Bildschirmmitte zu setzen
        float offsetX = -cols * cellSize / 2 + cellSize / 2;
        float offsetY = rows * cellSize / 2 - cellSize / 2;

        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                int value = matrix[row, col];
                Vector3 position = new Vector3(col * cellSize + offsetX, -row * cellSize + offsetY, 0);
                GameObject toSpawn = null;
                Quaternion rotation = Quaternion.identity;

                switch (value)
                {
                    case -1:
                        toSpawn = Instantiate(wall, position, Quaternion.identity);
                        if (col == 0 || col == matrix.GetLength(1) - 1)
                        {
                            toSpawn.transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        break;

                    case 1:
                        toSpawn = Instantiate(greenPier, position, Quaternion.identity);
                        addData(toSpawn, row, col, "GreenPier");
                        break;

                    case 2:
                        toSpawn = Instantiate(redPier, position, Quaternion.identity);
                        addData(toSpawn, row, col, "RedPier");
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
        ObjectData data = toSpawn.AddComponent<ObjectData>();
        data.X = row;
        data.Y = col;
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