using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;


[RequireComponent(typeof(CanvasGroup))]
public class ObstaculoUi : MonoBehaviour
{
    public obstaculoslevel obstaculoACriar; 
    private CanvasGroup canvasGroup;
    TextMeshProUGUI textoQuantidade;
    
    void Start()
    {
        Image imagemDoBotao = GetComponent<Image>();
        imagemDoBotao.sprite = obstaculoACriar.obstaculo.icone;

        textoQuantidade = GetComponentInChildren<TextMeshProUGUI>();
        textoQuantidade.text = "x" + obstaculoACriar.quantidade;

        canvasGroup = GetComponent<CanvasGroup>();

    }

    void Update()
    {
        if (obstaculoACriar.quantidade <= 0)
        {
            EsconderBotao();
        }
        else MostrarBotao();
        AtualizarQuantidade();
    }

    public void AtualizarQuantidade()
    {
        textoQuantidade.text = "x" + obstaculoACriar.quantidade;
    }

    public void EsconderBotao()
    {
        canvasGroup.alpha = 0f;          // Torna o botão e os textos filhos invisíveis
        canvasGroup.interactable = false;  // Impede que o botão reaja a cliques
        canvasGroup.blocksRaycasts = false;// Faz o clique do mouse "atravessar" o botão
    }

    // Chame esta função para MOSTRAR o botão de volta
    public void MostrarBotao()
    {
        canvasGroup.alpha = 1f;          // Torna visível novamente
        canvasGroup.interactable = true;   // Ativa os cliques
        canvasGroup.blocksRaycasts = true; // Bloqueia o mouse para poder clicar
    }


}
