using UnityEngine;

public class SwitchCanvas : MonoBehaviour
{
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject pauseCanvas;

    public void gameToPauseCanvas()
    {
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }

    public void pauseToGameCanvas()
    {
        pauseCanvas.SetActive(false);
        gameCanvas.SetActive(true);
    }
}
