using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isMultiplayer;
    [SerializeField] private int simulationCount;
    [SerializeField] private string initialPlayer = "Green";

    private int[,] gameMatrix;
    private bool isGameOver;

    private MatrixVisualizer matrixVisualizer;
    private MatrixManager matrixManager;
    private MatrixLogic matrixLogic;

    private string currentPlayer;

    private void Start()
    {
        InitializeComponents();
        InitializeGame();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            HandleGameLogic();
        }
    }

    private void InitializeComponents()
    {
        gameMatrix = GetComponent<Matrix>().GetMatrix();
        matrixVisualizer = GetComponent<MatrixVisualizer>();
        matrixManager = GetComponent<MatrixManager>();
        matrixLogic = GetComponent<MatrixLogic>();
    }

    private void InitializeGame()
    {
        currentPlayer = initialPlayer;
        matrixManager.CurrentPlayer = currentPlayer;
        matrixVisualizer.VisualizeMatrix(gameMatrix);
        isGameOver = false;
    }

    private void HandleGameLogic()
    {
        int[,] transformedMatrix = TransformMatrixValues(gameMatrix);

        if (isMultiplayer)
        {
            HandleMultiplayerLogic();
        }
        else
        {
            HandleSinglePlayerLogic(transformedMatrix);
        }

        CheckGameOver(transformedMatrix);
    }

    private void HandleMultiplayerLogic()
    {
        matrixManager.CurrentPlayer = currentPlayer;

        if (matrixManager.IsBridgePlaced)
        {
            SwitchPlayer();
            matrixManager.IsBridgePlaced = false;
        }
    }

    private void HandleSinglePlayerLogic(int[,] transformedMatrix)
    {
        matrixManager.CurrentPlayer = currentPlayer;

        if (matrixManager.IsBridgePlaced)
        {
            currentPlayer = currentPlayer.Equals("Green") ? "Computer" : "Green";
            matrixManager.IsBridgePlaced = false;
        }
        else if (currentPlayer.Equals("Computer"))
        {
            StartCoroutine(RunMCTSAsync(transformedMatrix));
        }
    }

    private IEnumerator RunMCTSAsync(int[,] transformedMatrix)
    {
        var mctsTask = matrixLogic.getBestMCTSAsync(transformedMatrix, simulationCount);
        yield return new WaitUntil(() => mctsTask.IsCompleted);

        (int row, int col) = mctsTask.Result;
        Debug.Log($"Best Move: ({row}, {col})");
        matrixManager.PlaceOpponentBridge(row, col);
    }

    private void CheckGameOver(int[,] transformedMatrix)
    {
        bool greenWins = matrixLogic.checkWinner(transformedMatrix, 1);
        bool redWins = matrixLogic.checkWinner(transformedMatrix, 2);

        if (greenWins)
        {
            Debug.Log("Green wins!");
            isGameOver = true;
        }

        if (redWins)
        {
            Debug.Log("Red wins!");
            isGameOver = true;
        }
    }

    private void SwitchPlayer()
    {
        currentPlayer = currentPlayer.Equals("Green") ? "Red" : "Green";
    }

    private int[,] TransformMatrixValues(int[,] matrix)
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
