using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MenuSystem.Buttons
{
    [RequireComponent(typeof(Button))]
    public class ButtonBehaviour : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerUpHandler,
        ISubmitHandler
    {
        [SerializeField] private ButtonEffectsData buttonEffectsData;
        [SerializeField] private AudioManager audioManager;

        private Button button;
        private SelectEffects selectEffects;
        private PressEffects pressEffects;

        public static event Action<Button> OnButtonSelected;

        private void Awake()
        {
            button = GetComponent<Button>();

            selectEffects = new();
            pressEffects = new();
        }

        private void OnEnable()
        {
            OnButtonSelected += RespondToButtonSelected;
        }
        private void OnDisable()
        {
            OnButtonSelected -= RespondToButtonSelected;
        }

        private void Start()
        {
            if (audioManager == null)
                audioManager = AudioManager.Instance;
        }

        //Detecta se outro botão foi selecionado e se desativa
        private void RespondToButtonSelected(Button otherButton)
        {
            if (button != otherButton)
                SelectButton(false);
        }

        public void OnSelect(BaseEventData eventData) { SelectButton(true); }
        public void OnDeselect(BaseEventData eventData) { SelectButton(true); }

        public void OnPointerEnter(PointerEventData eventData) { SelectButton(true); }
        public void OnPointerExit(PointerEventData eventData) { SelectButton(false); }

        public void OnPointerUp(PointerEventData eventData) { PressButton(); }
        public void OnSubmit(BaseEventData eventData) { PressButton(); }

        private void SelectButton(bool activate)
        {
            if (activate)
            {
                OnButtonSelected?.Invoke(button);
                selectEffects.ApplyEffects(button, buttonEffectsData);

                if (audioManager != null)
                    audioManager.PlaySFX(buttonEffectsData.selectSFX);
            }
            else
                selectEffects.RemoveEffects(button, buttonEffectsData);
        }

        private void PressButton()
        {
            pressEffects.ApplyEffects(button, buttonEffectsData);

            if (audioManager != null)
                audioManager.PlaySFX(buttonEffectsData.pressSFX);
        }
    }
}