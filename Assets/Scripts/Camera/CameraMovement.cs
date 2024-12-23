using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 5f; // Geschwindigkeit des Zooms
    [SerializeField] private float minZoom = 2f;  // Minimaler Zoom (näher ran)
    [SerializeField] private float maxZoom = 10f; // Maximaler Zoom (weiter weg)
    private Vector3 dragOrigin; // Startpunkt für das Ziehen

    void Update()
    {
        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        // Zoomen mit Mausrad
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }

        // Zoomen mit Touch
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Unterschied der Positionen zwischen den Frames berechnen
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            // Unterschied zwischen den beiden Distanzen berechnen
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Camera.main.orthographicSize += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandlePan()
    {
        // Kamerabewegung mit linker Maustaste
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += difference;
        }

        // Kamerabewegung mit Touch
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                dragOrigin = Camera.main.ScreenToWorldPoint(touch.position);
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(touch.position);
                Camera.main.transform.position += difference;
            }
        }
    }
}
