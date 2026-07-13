using UnityEngine;

public class MineiroStepSound : MonoBehaviour
{
    public void PlayStep()
    {
        AudioManager.Instance.PlayStep();
    }
}
