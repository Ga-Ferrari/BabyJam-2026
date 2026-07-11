using UnityEngine;

namespace MobileControls
{
    public class ControlsEnabler : MonoBehaviour
    {
        public static ControlsEnabler Instance;

        [SerializeField] private GameObject canvasObject;

        [Header("Movement Controls")]
        [SerializeField] private GameObject movementStick;
        [SerializeField] private GameObject movementButtons;
        public bool usesStick;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            canvasObject.SetActive(false);
        }

        private void Start()
        {
            RuntimePlatform platform = Application.platform;

            if (
                platform == RuntimePlatform.Android ||
                platform == RuntimePlatform.IPhonePlayer ||
                (platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
            )
            {
                if (usesStick)
                {
                    movementStick.SetActive(true);
                    movementButtons.SetActive(false);
                }
                else
                {
                    movementStick.SetActive(false);
                    movementButtons.SetActive(true);
                }

                canvasObject.SetActive(true);
            }
        }
    }
}