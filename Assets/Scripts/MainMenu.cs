using Assets.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneToTransition gameSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        // SceneManager.LoadSceneAsync(1);
        TransitionManager.Instance.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
