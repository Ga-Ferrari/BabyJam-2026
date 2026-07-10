using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

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

    private List<GameObject> botoes = new List<GameObject>();
    

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
            ObstaculoUi scriptBotao = novoBotao.GetComponent<ObstaculoUi>();
            // 4. Configura o clique do botão (Opcional, mas útil)
            Button componenteBotao = novoBotao.GetComponent<Button>();
            componenteBotao.onClick.AddListener(() => 
            {
                BotaoObstaculoClicado(obstaculo,scriptBotao);
            });
            botoes.Add(novoBotao);
        }
    }


    public void BotaoObstaculoClicado(obstaculoslevel obstaculo,ObstaculoUi scrBtn)
    {
        
        bool estaSelecionado = scrBtn.selecionado;
        foreach(GameObject botao in botoes)
        {
            botao.GetComponent<ObstaculoUi>().Desselecionar();
        }
        if (estaSelecionado)
        {
            // Se já estava selecionado, nós queremos soltar o item.
            // O foreach já apagou a UI, então só falta tirar do sistema.
            sistema.SelecionarObstaculo(null); 
        }
        else
        {
            // Se NÃO estava selecionado, nós queremos pegar o item.
            // Acendemos a UI dele e enviamos o obstáculo para o sistema.
            scrBtn.Selecionar();
            sistema.SelecionarObstaculo(obstaculo);
        }
    }


}
