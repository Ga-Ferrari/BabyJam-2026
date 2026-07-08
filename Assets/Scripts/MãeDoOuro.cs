using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UI;

public class MãeDoOuro : MonoBehaviour
{
    public Image Imagem;
         public Sprite idle;
    public Sprite MagicCast;

    public InputActionReference clickAction;

    private Coroutine castCoroutine;

    private void OnEnable()
    {
        if (clickAction != null && clickAction.action != null)
        {
                clickAction.action.Enable();
            clickAction.action.performed += OnClick;
        }
    }
            

    private void OnDisable()


    {
        if (clickAction != null && clickAction.action != null)
        {
                      clickAction.action.performed -= OnClick;
            clickAction.action.Disable();
        }
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Cast();
    }

    private void Cast()
    {
        if (castCoroutine != null)
        {


            StopCoroutine(castCoroutine);
        }
        castCoroutine = StartCoroutine(CastRoutine());
    }

    private IEnumerator CastRoutine()
    {
        if (Imagem != null) Imagem.sprite = MagicCast;


         yield return new WaitForSeconds(0.2f);




         if (Imagem != null) Imagem.sprite = idle;
        castCoroutine = null;
    }
}