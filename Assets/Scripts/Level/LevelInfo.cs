using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class obstaculoslevel
{
    public DadosDoObstaculo obstaculo;
    public int quantidade;
}

public class LevelInfo : MonoBehaviour
{
    [Header("Funcionamento da UI")]
    [SerializeField] private GameObject botaoPrefab;
    [SerializeField] private Transform containerDeBotoes; 

    [Header("Funcionamento seleção")]
    [SerializeField] private SistemaDeConstrucao sistema;

    [Header("OBSTÁCULOS DO NÍVEL, INSIRA OS OBSTÁCULOS DESEJADOS ABAIXO")]
    public obstaculoslevel[] obstaculos;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GerarBotoesNaUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GerarBotoesNaUI()
    {
        // Passa por cada obstáculo que você configurou na lista
        foreach (obstaculoslevel obstaculo in obstaculos)
        {
            // 1. Cria uma cópia do botão dentro do Container
            GameObject novoBotao = Instantiate(botaoPrefab, containerDeBotoes);

            novoBotao.GetComponent<ObstaculoUi>().obstaculoACriar = obstaculo;

            // 4. Configura o clique do botão (Opcional, mas útil)
            Button componenteBotao = novoBotao.GetComponent<Button>();
            componenteBotao.onClick.AddListener(() => 
            {
                BotaoObstaculoClicado(obstaculo);
            });
        }
    }


    public void BotaoObstaculoClicado(obstaculoslevel obstaculo)
    {
        sistema.SelecionarObstaculo(obstaculo);
    }


}
