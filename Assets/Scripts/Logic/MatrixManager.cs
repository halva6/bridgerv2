using System;
using UnityEngine;

public class MatrixManager : MonoBehaviour
{
    [SerializeField] private GameObject tempBridgePrefab;
    [SerializeField] private GameObject greenBridgePrefab;
    [SerializeField] private GameObject redBridgePrefab;
    [SerializeField] private float cellSize = 1f;

    private int[,] gameMatrix;
    private string currentPlayer;
    private bool isBridgePlaced;

    public bool IsBridgePlaced { get => isBridgePlaced; set => isBridgePlaced = value; }
    public string CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }

    public void PlaceBridge(int x, int y, string pierType, Quaternion rotation, Vector3 position)
    {
        ClearTemporaryBridges();
        Debug.Log($"Placing bridge at ({x}, {y}) for: {pierType}");

        GameObject newBridge = pierType == "Red"
            ? Instantiate(redBridgePrefab, position, rotation)
            : Instantiate(greenBridgePrefab, position, rotation);

        if (newBridge != null)
        {
            newBridge.transform.parent = this.transform;
            gameMatrix[y, x] = pierType == "Red" ? 4 : 3; // Red Bridge: 4, Green Bridge: 3
        }

        isBridgePlaced = true;
        UpdateGameMatrix(gameMatrix);
        LogMatrix(gameMatrix);
    }

    public void PlaceOpponentBridge(int x, int y)
    {
        Vector3 position = CalculatePosition(x, y);
        Quaternion rotation = ShouldRotate(y, x, 2) ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;

        GameObject newBridge = Instantiate(redBridgePrefab, position, rotation);

        if (newBridge != null)
        {
            newBridge.transform.parent = this.transform;
            gameMatrix[y, x] = 4; // Red Bridge
        }

        UpdateGameMatrix(gameMatrix);
        LogMatrix(gameMatrix);
    }

    public void EvaluateAndPlaceBridge(int x, int y, string pierType)
    {
        if (!IsValidPlayerPier(pierType)) return;

        gameMatrix = GetComponent<Matrix>().GetMatrix();
        ClearTemporaryBridges();

        TryPlaceBridge(x, y - 1, "Up");
        TryPlaceBridge(x, y + 1, "Down");
        TryPlaceBridge(x - 1, y, "Left");
        TryPlaceBridge(x + 1, y, "Right");
    }

    private void TryPlaceBridge(int x, int y, string direction)
    {
        if (!IsPositionValid(x, y)) return;

        Vector3 position = CalculatePosition(x, y);
        Quaternion rotation = CalculateRotation(direction);

        GameObject tempBridge = Instantiate(tempBridgePrefab, position, rotation);
        tempBridge.transform.parent = this.transform;

        AddTemporaryBridgeData(tempBridge, x, y, rotation);

        gameMatrix[y, x] = 5; // Mark as temporary bridge
    }

    private void ClearTemporaryBridges()
    {
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Temp"))
        {
            Destroy(temp);
        }

        ResetTemporaryBridgesInMatrix();
    }

    private void ResetTemporaryBridgesInMatrix()
    {
        for (int y = 0; y < gameMatrix.GetLength(0); y++)
        {
            for (int x = 0; x < gameMatrix.GetLength(1); x++)
            {
                if (gameMatrix[y, x] == 5)
                {
                    gameMatrix[y, x] = 0;
                }
            }
        }
    }

    private bool ShouldRotate(int row, int col, int target)
    {
        return (row > 0 && gameMatrix[row - 1, col] == target) ||
               (row < gameMatrix.GetLength(0) - 1 && gameMatrix[row + 1, col] == target);
    }

    private bool IsPositionValid(int x, int y)
    {
        return y >= 0 && x >= 0 && y < gameMatrix.GetLength(0) && x < gameMatrix.GetLength(1) && gameMatrix[y, x] == 0;
    }

    private bool IsValidPlayerPier(string pierType)
    {
        return (currentPlayer == "Green" && pierType == "GreenPier") || (currentPlayer == "Red" && pierType == "RedPier");
    }

    private Vector3 CalculatePosition(int x, int y)
    {
        float offsetX = -gameMatrix.GetLength(1) * cellSize / 2 + cellSize / 2;
        float offsetY = gameMatrix.GetLength(0) * cellSize / 2 - cellSize / 2;
        return new Vector3(x * cellSize + offsetX, -y * cellSize + offsetY, 0);
    }

    private Quaternion CalculateRotation(string direction)
    {
        return (direction == "Up" || direction == "Down") ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
    }

    private void AddTemporaryBridgeData(GameObject tempBridge, int x, int y, Quaternion rotation)
    {
        var tempData = tempBridge.AddComponent<ObjectDataTempBridge>();
        tempData.Type = currentPlayer;
        tempData.Rotation = rotation;
        tempData.Position = CalculatePosition(x, y);
        tempData.X = x;
        tempData.Y = y;
    }

    private void UpdateGameMatrix(int[,] updatedMatrix)
    {
        GetComponent<Matrix>().SetMatrix(updatedMatrix);
    }

    public static void LogMatrix(int[,] matrix)
    {
        string output = "";
        for (int y = 0; y < matrix.GetLength(0); y++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                output += matrix[y, x] == -1 ? "-" : matrix[y, x].ToString();
            }
            output += "\n";
        }
        Debug.Log(output);
    }
}
