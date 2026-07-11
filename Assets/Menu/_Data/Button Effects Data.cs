using DG.Tweening;
using UnityEngine;

namespace MenuSystem
{
    [CreateAssetMenu(fileName = "ButtonEffectsData", menuName = "Scriptable Objects/MenuSystem/ButtonEffectsData")]
    public class ButtonEffectsData : ScriptableObject
    {
        [Header("Select Effects")]
        public float sizeMultiplier;
        public float selectDuration;
        public Ease selectEase;
        public AudioClip selectSFX;

        [Header("Press Effects")]
        public float shakeStrength;
        public int vibrato;
        public float pressDuration;
        [Range(0, 180)] public float randomness;
        public AudioClip pressSFX;
    }
}