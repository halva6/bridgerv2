using System;
using UnityEngine;

public class ManageMatrix : MonoBehaviour
{
    [SerializeField] private GameObject tempBridgePrefab;
    [SerializeField] private GameObject greenBridgePrefab;
    [SerializeField] private GameObject redBridgePrefab;
    [SerializeField] private float cellSize = 1f;
    private int[,] matrix;
    private string type;

    public void SetBridge(int x, int y, string type, Quaternion rotation, Vector3 position)
    {
        RemoveTempBridges();
        Debug.Log($"Setting bridge at ({x}, {y}) with type: {type}");

        GameObject newObject = null;

        if (type.Equals("RedPier"))
        {
            newObject = Instantiate(redBridgePrefab, position, rotation);
            matrix[x, y] = 4; // Red Bridge
        }
        else if (type.Equals("GreenPier"))
        {
            newObject = Instantiate(greenBridgePrefab, position, rotation);
            matrix[x, y] = 3; // Green Bridge
        }

        if (newObject != null)
        {
            newObject.transform.parent = this.transform;
        }

        UpdateMatrix(matrix);
        PrintMatrix(matrix);
    }

    public void CheckAndPlace(int x, int y, string type)
    {
        this.type = type;
        matrix = gameObject.GetComponent<Matrix>().getMatrix();
        PrintMatrix(matrix);
        RemoveTempBridges();

        // Überprüfen und Platzieren für oben, unten, links, rechts
        TryPlaceAt(x, y - 1, "o"); // Oben
        TryPlaceAt(x, y + 1, "u"); // Unten
        TryPlaceAt(x - 1, y, "l"); // Links
        TryPlaceAt(x + 1, y, "r"); // Rechts
    }

    private void TryPlaceAt(int x, int y, string direction)
    {
        // Überprüfen, ob die Position innerhalb der Matrix liegt
        if (x < 0 || y < 0 || x >= matrix.GetLength(0) || y >= matrix.GetLength(1)) return;

        // Nur weiter machen, wenn die Matrix an der Stelle 0 ist
        if (matrix[x, y] == 0)
        {
            // Position für das neue GameObject berechnen
            float offsetX = -matrix.GetLength(0) * cellSize / 2 + cellSize / 2;
            float offsetY = matrix.GetLength(1) * cellSize / 2 - cellSize / 2;
            Vector3 position = new Vector3(x * cellSize + offsetX, -y * cellSize + offsetY, 0);

            // Rotation basierend auf Richtung setzen
            Quaternion rotation = Quaternion.identity;
            if (direction == "o" || direction == "u") // Oben oder Unten
            {
                rotation = Quaternion.Euler(0, 0, 90);
            }

            // Temporäres Objekt instanziieren
            GameObject tempObject = Instantiate(tempBridgePrefab, position, rotation);
            tempObject.transform.parent = this.transform;

            // Daten dem temporären Objekt hinzufügen
            ObjectDataTempBridge tempData = tempObject.AddComponent<ObjectDataTempBridge>();
            tempData.Type = this.type;
            tempData.Rotation = rotation;
            tempData.Position = position;
            tempData.X = x;
            tempData.Y = y;

            // Matrix aktualisieren
            matrix[x, y] = 5; // Temporäre Brücke markieren
        }
    }

    private void RemoveTempBridges()
    {
        // Alle temporären Brücken entfernen
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Temp"))
        {
            Destroy(temp);
        }

        // Temporäre Brücken in der Matrix zurücksetzen
        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                if (matrix[x, y] == 5) // Temporäre Brücken
                {
                    matrix[x, y] = 0; // Zurücksetzen
                }
            }
        }
    }

    private void UpdateMatrix(int[,] updatedMatrix)
    {
        gameObject.GetComponent<Matrix>().setMatrix(updatedMatrix);
    }

    public static void PrintMatrix(int[,] matrix)
    {
        string output = "";

        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                output += matrix[x, y] == -1 ? "-" : matrix[x, y].ToString();
            }
            output += "\n";
        }

        Debug.Log(output);
    }
}
