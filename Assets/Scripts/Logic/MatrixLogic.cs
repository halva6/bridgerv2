using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class MatrixLogic : MonoBehaviour
{
    private int[,] matrix;

    public bool checkWinner(int[,] board, int player)
    {
        if (player == 1)
        {
            for (int startCol = 0; startCol < board.GetLength(1); startCol++)
            {
                if (board[0, startCol] == player)
                {
                    if (DFS(board, 0, startCol, player, new HashSet<(int, int)>()))
                    {
                        return true;
                    }
                }
            }
        }
        else if (player == 2)
        {
            for (int startRow = 0; startRow < board.GetLength(0); startRow++)
            {
                if (board[startRow, 0] == player)
                {
                    if (DFS(board, startRow, 0, player, new HashSet<(int, int)>()))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool DFS(int[,] board, int row, int col, int player, HashSet<(int, int)> visited)
    {
        if (player == 1 && row == board.GetLength(0) - 1)
        {
            return true;
        }
        if (player == 2 && col == board.GetLength(1) - 1)
        {
            return true;
        }

        var directions = new List<(int, int)> { (1, 0), (0, 1), (-1, 0), (0, -1) };
        visited.Add((row, col));

        foreach (var (dr, dc) in directions)
        {
            int newRow = row + dr, newCol = col + dc;
            if (0 <= newRow && newRow < board.GetLength(0) && 0 <= newCol && newCol < board.GetLength(1))
            {
                if (!visited.Contains((newRow, newCol)) && board[newRow, newCol] == player)
                {
                    if (DFS(board, newRow, newCol, player, visited))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public int Minimax(int[,] board, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        if (checkWinner(board, 1))
            return -1000 + depth;
        if (checkWinner(board, 2))
            return 1000 - depth;
        if (depth == 0 || !getValidMoves(board).Any())
            return 0;

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            foreach (var move in getValidMoves(board))
            {
                int eval = Minimax(makeMove(board, move, 2), depth - 1, alpha, beta, false);
                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                    break;
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (var move in getValidMoves(board))
            {
                int eval = Minimax(makeMove(board, move, 1), depth - 1, alpha, beta, true);
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha)
                    break;
            }
            return minEval;
        }
    }

    public (int, int) getBestMove(int[,] board, int depth)
    {
        (int, int) bestMove = (-1, -1);
        int bestValue = int.MinValue;
        foreach (var move in getValidMoves(board))
        {
            int moveValue = Minimax(makeMove(board, move, 2), depth, int.MinValue, int.MaxValue, false);
            if (moveValue > bestValue)
            {
                bestValue = moveValue;
                bestMove = move;
            }
        }
        return bestMove;
    }

    public List<(int, int)> getValidMoves(int[,] board)
    {
        var validMoves = new List<(int, int)>();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == 0)
                    validMoves.Add((i, j));
            }
        }
        return validMoves;
    }

    public int[,] makeMove(int[,] board, (int, int) move, int player)
    {
        var newBoard = (int[,])board.Clone();
        newBoard[move.Item1, move.Item2] = player;
        return newBoard;
    }

    public static List<Vector2Int> FindDifferences(int[,] matrix1, int[,] matrix2)
    {
        if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
        {
            throw new ArgumentException("Matrices must have the same dimensions");
        }

        List<Vector2Int> differences = new List<Vector2Int>();

        for (int i = 0; i < matrix1.GetLength(0); i++)
        {
            for (int j = 0; j < matrix1.GetLength(1); j++)
            {
                if (matrix1[i, j] != matrix2[i, j])
                {
                    differences.Add(new Vector2Int(i, j));
                }
            }
        }

        return differences;
    }

    public (int, int) getBestMCTS(int[,] board, int simulationsNumber)
    {
        if (!checkWinner(board, 1) && !checkWinner(board, 2))
        {
            MCTS mCTS = new MCTS(board, 1);
            int[,] moveBoard = mCTS.BestMove(simulationsNumber);

            var differences = FindDifferences(board, moveBoard);
            if (differences.Count > 0)
            {
                var firstDifference = differences[0];
                return (firstDifference.x, firstDifference.y);
            }
        }

        return (-1, -1);
    }

    public async Task<(int, int)> getBestMCTSAsync(int[,] board, int simulationsNumber)
    {
        return await Task.Run(() =>
                {
                    return getBestMCTS(board, simulationsNumber);
                });
    }
}

public class MCTSNode
{
    public int[,] State;
    public MCTSNode Parent;
    public List<MCTSNode> Children;
    public int Wins;
    public int Visits;

    public MCTSNode(int[,] state, MCTSNode parent = null)
    {
        State = state;
        Parent = parent;
        Children = new List<MCTSNode>();
        Wins = 0;
        Visits = 0;
    }

    public List<Vector2Int> GetValidMoves(int[,] board, int player)
    {
        var validMoves = new List<Vector2Int>();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == 0)
                {
                    validMoves.Add(new Vector2Int(i, j));
                }
            }
        }
        return validMoves;
    }

    public bool IsFullyExpanded(int player)
    {
        return Children.Count == GetValidMoves(State, player).Count;
    }

    public MCTSNode BestChild(float explorationWeight = 1.41f)
    {
        float bestScore = float.NegativeInfinity;
        MCTSNode bestChild = null;
        foreach (var child in Children)
        {
            float uctValue = (float)child.Wins / (float)child.Visits + explorationWeight * Mathf.Sqrt(2 * Mathf.Log(Visits) / (float)child.Visits);
            if (uctValue > bestScore)
            {
                bestScore = uctValue;
                bestChild = child;
            }
        }
        return bestChild;
    }
}

public class MCTS
{
    public MCTSNode Root;
    public int Player;

    public MCTS(int[,] state, int player)
    {
        Root = new MCTSNode(state);
        Player = player;
    }

    public MCTSNode Select(MCTSNode node)
    {
        while (node.IsFullyExpanded(Player))
        {
            node = node.BestChild();
        }
        return node;
    }


    public MCTSNode Expand(MCTSNode node)
    {
        var validMoves = GetValidMoves(node.State, Player);

        // Nur nicht bereits expandierte Moves wählen
        var unexpandedMoves = validMoves
            .Where(move => !node.Children.Any(child => AreStatesEqual(child.State, MakeMove(node.State, move, Player))))
            .ToList();

        if (unexpandedMoves.Count == 0)
        {
            return node; // Keine Erweiterung möglich
        }

        var moveToExpand = unexpandedMoves[0];
        var newState = MakeMove(node.State, moveToExpand, Player);
        var childNode = new MCTSNode(newState, node);
        node.Children.Add(childNode);
        return childNode;
    }


    private bool AreStatesEqual(int[,] a, int[,] b)
    {
        if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1)) return false;

        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < a.GetLength(1); j++)
            {
                if (a[i, j] != b[i, j]) return false;
            }
        }
        return true;
    }

    public int Simulate(MCTSNode node)
    {
        int[,] currentState = (int[,])node.State.Clone();
        int currentPlayer = Player;
        System.Random random = new System.Random();
        int i = 0;
        while (!CheckWinner(currentState, 1) && !CheckWinner(currentState, 2) && GetValidMoves(currentState, currentPlayer).Count > 0)
        {
            //Debug.Log("[DEBUG] Next Random: " + NextRandom(0, GetValidMoves(currentState, currentPlayer).Count) +  " int i: " + i);
            var move = GetValidMoves(currentState, currentPlayer)[random.Next(0, GetValidMoves(currentState, currentPlayer).Count)];
            currentState = MakeMove(currentState, move, currentPlayer);
            currentPlayer = 3 - currentPlayer;
            i++;
        }
        if (CheckWinner(currentState, Player))
        {
            return 1;
        }
        else if (CheckWinner(currentState, 3 - Player))
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }


    public void Backpropagate(MCTSNode node, int result)
    {
        while (node != null)
        {
            node.Visits++;
            node.Wins += result;
            node = node.Parent;
        }
    }

    public int[,] BestMove(int simulationsNumber)
    {
        for (int i = 0; i < simulationsNumber; i++)
        {
            var node = Select(Root);
            if (!CheckWinner(node.State, 1) && !CheckWinner(node.State, 2) && GetValidMoves(node.State, Player).Count > 0)
            {
                node = Expand(node);
            }
            int result = Simulate(node);
            Backpropagate(node, result);
        }
        return Root.BestChild(0).State;
    }

    public List<Vector2Int> GetValidMoves(int[,] board, int player)
    {
        var validMoves = new List<Vector2Int>();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == 0)
                {
                    validMoves.Add(new Vector2Int(i, j));
                }
            }
        }
        return validMoves;
    }

    public int[,] MakeMove(int[,] board, Vector2Int move, int player)
    {
        int[,] newBoard = (int[,])board.Clone();
        newBoard[move.x, move.y] = player;
        return newBoard;
    }

    public bool CheckWinner(int[,] board, int player)
    {
        if (player == 1)
        {
            for (int startCol = 0; startCol < board.GetLength(1); startCol++)
            {
                if (board[0, startCol] == player)
                {
                    if (DFS(board, 0, startCol, player, new HashSet<Vector2Int>()))
                    {
                        return true;
                    }
                }
            }
        }
        else if (player == 2)
        {
            for (int startRow = 0; startRow < board.GetLength(0); startRow++)
            {
                if (board[startRow, 0] == player)
                {
                    if (DFS(board, startRow, 0, player, new HashSet<Vector2Int>()))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool DFS(int[,] board, int row, int col, int player, HashSet<Vector2Int> visited)
    {
        if (player == 1 && row == board.GetLength(0) - 1)
        {
            return true;
        }
        if (player == 2 && col == board.GetLength(1) - 1)
        {
            return true;
        }

        var directions = new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };
        visited.Add(new Vector2Int(row, col));

        foreach (var direction in directions)
        {
            int newRow = row + direction.x;
            int newCol = col + direction.y;
            if (newRow >= 0 && newRow < board.GetLength(0) && newCol >= 0 && newCol < board.GetLength(1))
            {
                if (!visited.Contains(new Vector2Int(newRow, newCol)) && board[newRow, newCol] == player)
                {
                    if (DFS(board, newRow, newCol, player, visited))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
