using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Core
{
    public enum SceneToTransition
    {
        MainMenu,
        Tutorial,
        Level1,
        Level2,
        WinScreen,
        DefeatScreen
    }

    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager Instance { get; private set; }

        [Header("Configurações Visuais")]
        [SerializeField] private CanvasGroup transitionCanvasGroup;
        [SerializeField] private float fadeDuration = 0.5f;
        public string lastGameplayScene;
        private bool isTransitioning = false;

        [SerializeField] private AudioClip musicaIngame;
        [SerializeField] private AudioClip musicaMenu;
        [SerializeField] private AudioClip musicaVitoria;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            transitionCanvasGroup.alpha = 0f;
            transitionCanvasGroup.blocksRaycasts = false;
        }

        private void Start()
        {
            transitionCanvasGroup.alpha = 1f;
            transitionCanvasGroup.DOFade(0f, fadeDuration).SetUpdate(true);
        }

        public void LoadScene(SceneToTransition sceneName)
        {
            if (isTransitioning) return;

            lastGameplayScene = SceneManager.GetActiveScene().name;

            if (sceneName == SceneToTransition.Tutorial || sceneName == SceneToTransition.Level1 || sceneName == SceneToTransition.Level2)
            {
                AudioManager.Instance.PlayMusic(musicaIngame);
            }
            else if (sceneName == SceneToTransition.MainMenu || sceneName == SceneToTransition.DefeatScreen)
            {
                AudioManager.Instance.PlayMusic(musicaMenu);
            }
            else if (sceneName == SceneToTransition.DefeatScreen)
            {
                AudioManager.Instance.PlayMusic(musicaVitoria);
            }

            StartCoroutine(TransitionRoutine(sceneName.ToString()));

        }

        public void ReloadLastGameplayScene()
        {
            if (isTransitioning || string.IsNullOrEmpty(lastGameplayScene))
                return;

            StartCoroutine(TransitionRoutine(lastGameplayScene));
        }

        private IEnumerator TransitionRoutine(string sceneName = "")
        {
            isTransitioning = true;
            transitionCanvasGroup.blocksRaycasts = true;

            yield return transitionCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true).WaitForCompletion();

            Time.timeScale = 1f;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            yield return new WaitUntil(() => asyncLoad.isDone);

            yield return null;

            yield return transitionCanvasGroup.DOFade(0f, fadeDuration).SetUpdate(true).WaitForCompletion();

            transitionCanvasGroup.blocksRaycasts = false;
            isTransitioning = false;
        }
    }
}