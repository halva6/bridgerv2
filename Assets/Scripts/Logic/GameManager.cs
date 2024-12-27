using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool multiplayer;

    private int[,] matrix;
    private PresentMatrix presentMatrix;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        matrix = gameObject.GetComponent<Matrix>().getMatrix();
        gameObject.GetComponent<PresentMatrix>().SpawnMatrix(this.matrix);
    }

    // Update is called once per frame 
    void Update()
    {
        
    }
}
