using UnityEngine;

namespace MobileControls
{
    public class PauseHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] disableableObjects;

        public void TogglePause(bool activate)
        {
            if (activate)
            {
                foreach (GameObject obj in disableableObjects)
                    obj.SetActive(false);
            }
            else
            {
                foreach (GameObject obj in disableableObjects)
                    obj.SetActive(true);
            }
        }
    }
}