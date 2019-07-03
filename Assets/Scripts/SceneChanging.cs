using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanging : MonoBehaviour
{
    public void Changing()
    {
        SceneManager.LoadScene(1);
    }

    public void Exiting()
    {
        Application.Quit();
    }
}
