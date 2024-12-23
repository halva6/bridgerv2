using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class ManageMatrix : MonoBehaviour
{
    [SerializeField] private GameObject tempBridgePrefab;
    [SerializeField] private GameObject greenBridgePrefab;
    [SerializeField] private GameObject redBridgePrefab;
    [SerializeField] private float cellSize = 1f;
    private int[,] matrix;
    private string type;


    public void setBridge(int x, int y, string type, Quaternion rotation, Vector3 position)
    {
        removeTempBridge(matrix);
        //Debug.Log(type);
        if (type.Equals("RedPier"))
        {
            GameObject newObject = Instantiate(redBridgePrefab, position, rotation);
            matrix[x, y] = 4;

        }
        else if (type.Equals("GreenPier"))
        {
            GameObject newObject = Instantiate(greenBridgePrefab, position, rotation);
            matrix[x, y] = 3;
        }

        gameObject.GetComponent<Matrix>().setMatrix(matrix);
        PrintMatrix(gameObject.GetComponent<Matrix>().getMatrix());
    }

    public void CheckAndPlace(int x, int y, string type)
    {
        this.type = type;
        matrix = gameObject.GetComponent<Matrix>().getMatrix();
        PrintMatrix(matrix);
        removeTempBridge(matrix);

        // Überprüfe die Positionen oben, unten, links, rechts
        TryPlaceAt(x, y - 1, "o"); // Oben
        TryPlaceAt(x, y + 1, "u"); // Unten
        TryPlaceAt(x - 1, y, "l"); // Links
        TryPlaceAt(x + 1, y, "r"); // Rechts
    }

    private void TryPlaceAt(int x, int y, string direction)
    {

        // Überprüfen, ob die Position innerhalb der Matrix liegt
        if (x < 0 || y < 0 || x >= matrix.GetLength(1) || y >= matrix.GetLength(0)) return;

        // Nur weiter machen, wenn die Matrix an der Stelle 0 ist
        if (matrix[x, y] == 0)
        {
            // Position für das neue GameObject berechnen
            float offsetX = -matrix.GetLength(1) * cellSize / 2 + cellSize / 2;
            float offsetY = matrix.GetLength(0) * cellSize / 2 - cellSize / 2;
            Vector3 position = new Vector3(x * cellSize + offsetX, -y * cellSize + offsetY, 0);

            // Standard-Rotation
            Quaternion rotation = Quaternion.identity;

            // Drehe das Objekt, wenn es oben oder unten platziert wird
            if (direction.Equals("o") || direction.Equals("u"))
            {
                rotation = Quaternion.Euler(0, 0, 90);
            }

            // Neues GameObject instanziieren
            GameObject newObject = Instantiate(tempBridgePrefab, position, rotation);
            newObject.transform.parent = this.transform;

            TempBridge tb = newObject.AddComponent<TempBridge>();
            tb.Type = this.type;
            tb.Rotaion = rotation;
            tb.Position = position;
            tb.X = x;
            tb.Y = y;


            // Matrix aktualisieren
            matrix[x, y] = 5;
        }
    }

    void removeTempBridge(int[,] matrix)
    {
        GameObject[] tempBridge = GameObject.FindGameObjectsWithTag("Temp");

        foreach (GameObject gb in tempBridge)
        {
            Destroy(gb);
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == 5)
                {
                    matrix[i, j] = 0;
                }
            }
        }
    }

    public static void PrintMatrix(int[,] matrix)
    {
        string output = "";

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == -1)
                {
                    output += "-";
                }
                else
                {
                    output += matrix[i, j];
                }
            }
            output += "\n";
        }

        Debug.Log(output);
    }
}
