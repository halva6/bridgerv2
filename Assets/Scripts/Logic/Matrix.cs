using UnityEngine;

public class Matrix : MonoBehaviour
{
    [SerializeField] private int[,] matrix = new int[,]
    {
        { -1,  1, -1,  1, -1,  1, -1,  1, -1,  1, -1 },
        {  2,  0,  2,  0,  2,  0,  2,  3,  2,  0,  2 },
        { -1,  1,  0,  1,  0,  1,  0,  1,  0,  1, -1 },
        {  2,  4,  2,  3,  2,  0,  2,  0,  2,  0,  2 },
        { -1,  1,  0,  1,  0,  1,  0,  1,  0,  1, -1 },
        {  2,  0,  2,  0,  2,  0,  2,  0,  2,  0,  2 },
        { -1,  1,  0,  1,  4,  1,  0,  1,  0,  1, -1 },
        {  2,  0,  2,  0,  2,  0,  2,  0,  2,  0,  2 },
        { -1,  1,  0,  1,  0,  1,  0,  1,  0,  1, -1 },
        {  2,  0,  2,  0,  2,  0,  2,  4,  2,  0,  2 },
        { -1,  1,  0,  1,  0,  1,  0,  1,  0,  1, -1 },
        {  2,  0,  2,  0,  2,  0,  2,  0,  2,  0,  2 },
        { -1,  1, -1,  1, -1,  1, -1,  1, -1,  1, -1 }
    };

    public int[,] getMatrix()
    {
        return matrix;
    }

    public void setMatrix(int[,] matrix)
    {
        this.matrix = matrix;
    }
}