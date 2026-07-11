using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MenuSystem.Buttons
{
    public class SelectEffects
    {
        private Tween lastTween;

        public void ApplyEffects(Button button, ButtonEffectsData buttonEffectsData)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();

            if (lastTween != null)
                lastTween.Kill();

            lastTween = buttonTransform.DOScale(Vector3.one * buttonEffectsData.sizeMultiplier, buttonEffectsData.selectDuration)
            .SetEase(buttonEffectsData.selectEase)
            .SetUpdate(true);
        }

        public void RemoveEffects(Button button, ButtonEffectsData buttonEffectsData)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();

            if (lastTween != null)
                lastTween.Kill();

            lastTween = buttonTransform.DOScale(Vector3.one, buttonEffectsData.sizeMultiplier)
            .SetEase(buttonEffectsData.selectEase)
            .SetUpdate(true);
        }
    }
}