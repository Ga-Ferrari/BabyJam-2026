using Assets.Core;
using UnityEngine;
using UnityEngine.SceneManagement;


public class scr_DeamScreen : MonoBehaviour
{
    [SerializeField] private SceneToTransition mainMenuSceneName;

    public void TryAgain()
    {
        TransitionManager.Instance.ReloadLastGameplayScene();
    }

    public void MainMenu()
    {
        TransitionManager.Instance.LoadScene(mainMenuSceneName);
    }
}
