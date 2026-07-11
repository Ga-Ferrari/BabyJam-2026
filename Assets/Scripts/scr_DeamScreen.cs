using Assets.Core;
using UnityEngine;
using UnityEngine.SceneManagement;


public class scr_DeamScreen : MonoBehaviour
{
    [SerializeField] private string tryAgainSceneName;
    [SerializeField] private string mainMenuSceneName;

    public void TryAgain()
    {
        TransitionManager.Instance.LoadScene(tryAgainSceneName);
    }

    public void MainMenu()
    {
        TransitionManager.Instance.LoadScene(mainMenuSceneName);
    }
}
