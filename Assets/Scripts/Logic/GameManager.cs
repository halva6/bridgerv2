using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool multiplayer;

    private int[,] matrix;
    private PresentMatrix presentMatrix;
    private ManageMatrix manageMatrix;

    private MatrixLogic matrixLogic;

    [SerializeField] private string currentPlayer = "Green";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        matrix = gameObject.GetComponent<Matrix>().getMatrix();
        presentMatrix = gameObject.GetComponent<PresentMatrix>();
        manageMatrix = gameObject.GetComponent<ManageMatrix>();
        matrixLogic = gameObject.GetComponent<MatrixLogic>();

        manageMatrix.CurrentPlayerType = currentPlayer;
        presentMatrix.SpawnMatrix(this.matrix);
        
    }

    // Update is called once per frame 
    void Update()
    {
        if(multiplayer)
        {
            manageMatrix.CurrentPlayerType = currentPlayer;

            if(manageMatrix.IsSetBridge)
            {
                int [,] transformedMatrix = TransformMatrix(matrix);
                ManageMatrix.PrintMatrix(transformedMatrix);
                bool greenWin = matrixLogic.checkWinner(transformedMatrix, 1);
                bool redWin = matrixLogic.checkWinner(transformedMatrix, 2);

                if(greenWin)
                {
                    Debug.Log("[DEBUG] green wins");
                }
                
                if(redWin)
                {
                    Debug.Log("[DEBUG] red wins");
                }

                if(currentPlayer.Equals("Green"))
                {
                    currentPlayer = "Red";
                }else
                {
                    //currentPlayer = "Green";
                }
                manageMatrix.IsSetBridge = false;
            }
        }
    }

    private int[,] TransformMatrix(int [,] matrix)
    {
        for (int y = 0; y < matrix.GetLength(0); y++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                if (matrix[y, x] == 3)
                {
                    matrix[y, x] = 1;
                }
                else if (matrix[y, x] == 4)
                {
                    matrix[y, x] = 2;
                }
            }
        }
        return matrix;
    }
}
