using UnityEngine;
using System.Collections.Generic;
using System;

public class ResetMove : MonoBehaviour
{
    private Stack<int[,]> matrixStack = new Stack<int[,]>();
    private Matrix matrix;

    void Start()
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
        if (matrixStack.Count > 0)
        {
            int[,] popMatrix = matrixStack.Pop();
            (int row, int col) = FindDifference(matrix.GetMatrix(), popMatrix);
            GameObject toRemoveGameObject = GameObject.Find($"{row} {col}");

            Destroy(toRemoveGameObject);
            Debug.Log($"Removed: {col};{row}");

            matrix.SetMatrix(popMatrix);
            Debug.Log("Matrix zurückgesetzt. Stackgröße: " + matrixStack.Count);
        }
        else
        {
            Debug.Log("Keine vorherigen Zustände vorhanden.");
        }
    }

    private (int row, int col) FindDifference(int[,] matrix1, int[,] matrix2)
    {
        int rows = matrix1.GetLength(0);
        int cols = matrix1.GetLength(1);

        // Iteriere durch die Matrizen und finde die erste unterschiedliche Position
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (matrix1[row, col] != matrix2[row, col])
                {
                    // Unterschied gefunden
                    return (row, col);
                }
            }
        }

        // Keine Unterschiede gefunden
        throw new Exception("Die Matrizen sind identisch.");
    }

}

