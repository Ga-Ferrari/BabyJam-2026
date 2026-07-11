using DG.Tweening;
using UnityEngine;

namespace MenuSystem
{
    [CreateAssetMenu(fileName = "SectionEffectsData", menuName = "Scriptable Objects/MenuSystem/SectionEffectsData")]
    public class SectionEffectsData : ScriptableObject
    {
        [Header("Activation")]
        public float activationDuration;
        public Ease activationEase;
        public AudioClip openSFX;

        [Header("Deactivation")]
        public float finalSizeMultiplier;
        public float deactivationDuration;
        public Ease deactivationEase;
        public AudioClip closeSFX;
    }
}