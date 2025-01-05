using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool multiplayer;
    [SerializeField] int simulationsNumber;

    private int[,] matrix;
    private bool gameOver = false;
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
        if (!gameOver)
        {
            int[,] transformedMatrix = TransformMatrix(matrix);

            if (multiplayer)
            {
                manageMatrix.CurrentPlayerType = currentPlayer;

                if (manageMatrix.IsSetBridge)
                {

                    if (currentPlayer.Equals("Green"))
                    {
                        currentPlayer = "Red";
                    }
                    else
                    {
                        currentPlayer = "Green";
                    }
                    manageMatrix.IsSetBridge = false;
                }
            }
            else
            {
                manageMatrix.CurrentPlayerType = currentPlayer;

                if (manageMatrix.IsSetBridge)
                {

                    if (currentPlayer.Equals("Green"))
                    {
                        currentPlayer = "Computer";
                    }

                    manageMatrix.IsSetBridge = false;

                }
                else if (currentPlayer.Equals("Computer"))
                {
                    currentPlayer = "Green";
                    StartCoroutine(RunMCTSAsync(transformedMatrix));
                    manageMatrix.IsSetBridge = false;

                }
            }

            //ManageMatrix.PrintMatrix(transformedMatrix);
            bool greenWin = matrixLogic.checkWinner(transformedMatrix, 1);
            bool redWin = matrixLogic.checkWinner(transformedMatrix, 2);

            if (greenWin)
            {
                Debug.Log("[DEBUG] green wins");
                gameOver = true;
            }

            if (redWin)
            {
                Debug.Log("[DEBUG] red wins");
                gameOver = true;
            }
        }
    }

    private IEnumerator RunMCTSAsync(int[,] transformedMatrix)
    {
        //isThinking = true;
        //loadingBar.SetActive(true);
        var task = matrixLogic.getBestMCTSAsync(transformedMatrix, simulationsNumber);
        yield return new WaitUntil(() => task.IsCompleted);

        (int, int) coordsEnemy = task.Result;
        Debug.Log($"Best Move: ({coordsEnemy.Item1}, {coordsEnemy.Item2})");
        manageMatrix.setEnemyBridge(coordsEnemy.Item2, coordsEnemy.Item1);
        //setEnemyBridge(redBridge, coordsEnemy.Item1, coordsEnemy.Item2);
        //loadingBar.SetActive(false);
        //isThinking = false;
    }

    private int[,] TransformMatrix(int[,] matrix)
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
