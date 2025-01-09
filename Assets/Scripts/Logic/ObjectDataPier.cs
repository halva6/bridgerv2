using Unity.Collections;
using UnityEngine;

public class ObjectDataPier : MonoBehaviour
{
    private int x;
    private int y;

    private string type;

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public string Type { get => type; set => type = value; }

    void Update()
    {
        // Überprüfen, ob die linke Maustaste gedrückt wurde
        if (Input.GetMouseButtonDown(0))
        {
            CheckClick(Input.mousePosition);
        }

        // Überprüfen, ob es Touch-Eingaben gibt
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckClick(Input.GetTouch(0).position);
        }
    }

    void CheckClick(Vector2 inputPosition)
    {
        // Umrechnung der Bildschirmkoordinaten in Weltkoordinaten
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);

        // Raycast für 2D-Physik
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                Debug.Log("Click: " + type + " Pos: " + X.ToString() + ";" + Y.ToString());
                MatrixManager mm = GetComponentInParent<MatrixManager>();
                mm?.EvaluateAndPlaceBridge(X, Y, type);
            }
        }
    }
}
