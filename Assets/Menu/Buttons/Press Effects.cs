using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MenuSystem.Buttons
{
    public class PressEffects
    {
        public void ApplyEffects(Button button, ButtonEffectsData buttonEffectsData)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();

            if (button.interactable == false)
            {
                //Animação de pressionar o botão inválido
                buttonTransform.DOShakeAnchorPos(buttonEffectsData.pressDuration, buttonEffectsData.shakeStrength, buttonEffectsData.vibrato, buttonEffectsData.randomness)
                .SetUpdate(true);
            }
            else
            {
                //Animação de pressionar o botão válido, se quiser implementar

            }
        }
    }
}