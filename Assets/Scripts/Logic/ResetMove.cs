using UnityEngine;
using System.Collections.Generic;
using System;

public class ResetMove : MonoBehaviour
{
    private Stack<int[,]> matrixStack = new Stack<int[,]>();
    private Matrix matrix;
    void Awake()
    {
        matrix = gameObject.GetComponent<Matrix>();
    }

    public void SaveState()
    {
        int[,] matrixCopy = (int[,])matrix.GetMatrix().Clone();
        matrixStack.Push(matrixCopy);
        Debug.Log("Matrix gespeichert. Stackgröße: " + matrixStack.Count);
    }

    public void Undo()
    {
        if (matrixStack.Count >= 2)
        {
            // Entferne die letzten zwei Zustände
            matrixStack.Pop(); // letzter Zug
            int[,] previousMatrix = matrixStack.Pop(); // vorletzter Zug

            // Vergleiche den aktuellen Matrix-Zustand mit dem alten Zustand
            List<(int row, int col)> differences = FindDifferences(matrix.GetMatrix(), previousMatrix);

            foreach (var (row, col) in differences)
            {
                GameObject toRemove = GameObject.Find($"{col};{row}");
                if (toRemove != null)
                {
                    Destroy(toRemove);
                    Debug.Log($"Removed: {col};{row}");
                }
            }

            matrix.SetMatrix(previousMatrix);
            Debug.Log("Matrix zwei Schritte zurückgesetzt. Stackgröße: " + matrixStack.Count);
        }
        else
        {
            Debug.Log("Nicht genug Zustände zum Zurücksetzen vorhanden.");
        }
    }

    // Gibt alle Unterschiede zwischen zwei Matrizen zurück
    private List<(int row, int col)> FindDifferences(int[,] matrix1, int[,] matrix2)
    {
        List<(int row, int col)> differences = new List<(int, int)>();
        int rows = matrix1.GetLength(0);
        int cols = matrix1.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (matrix1[row, col] != matrix2[row, col])
                {
                    differences.Add((row, col));
                }
            }
        }

        if (differences.Count == 0)
        {
            Debug.Log("Die Matrizen sind identisch.");
        }

        return differences;
    }
}
