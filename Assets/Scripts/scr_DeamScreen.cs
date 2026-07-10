using UnityEngine;
using UnityEngine.SceneManagement;


public class scr_DeamScreen : MonoBehaviour
{
    public void TryAgain()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
