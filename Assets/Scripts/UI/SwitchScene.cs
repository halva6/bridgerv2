using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
