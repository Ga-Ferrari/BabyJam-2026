using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public enum TipoTilemap
{
    Fundo,
    Paredes,
    Ouro,
    OuroFalso
}

public class ObstaculoUi : MonoBehaviour
{
    public obstaculoslevel obstaculoACriar; 
    TextMeshProUGUI textoQuantidade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 2. Muda a imagem do botão para o ícone do obstáculo
            Image imagemDoBotao = GetComponent<Image>();
            imagemDoBotao.sprite = obstaculoACriar.obstaculo.icone;

            // 3. Pega o Texto filho do botão e muda para a quantidade
            textoQuantidade = GetComponentInChildren<TextMeshProUGUI>();
            textoQuantidade.text = "x" + obstaculoACriar.quantidade;

    }

    // Update is called once per frame
    void Update()
    {
        if (obstaculoACriar.quantidade <= 0)
        {
            enabled = false;
        }
        AtualizarQuantidade();
    }

    public void AtualizarQuantidade()
    {
        textoQuantidade.text = "x" + obstaculoACriar.quantidade;
    }




}
