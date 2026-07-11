using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class MineiroMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    private int custoMovimentoAtual;
    private float tempoEntrePassos;


    private float movimentoTimer = 0.0f;
    private Vector3 ultimaPosicao;
    public Vector3 destinoAtual;

    public MapGrid mapa;

    public List<Node> caminhoParaOOuro = new List<Node>();

    public UnityEvent AoChegarNoDestino;
    public UnityEvent<int> AndouTile;

    [SerializeField] private SistemaDeConstrucao sistemaDeConstrucao;
    private Animator animator;
    private bool temCaminho = false;

    [SerializeField] private FuncoesCena funcoesCena;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Andando", false);
        ultimaPosicao = transform.position;
        destinoAtual = transform.position;
        tempoEntrePassos = GameManager.Instance.VelocidadeMineiroBase;
    }

    void Update()
    {
        // Se temos um caminho a seguir, o relógio começa a contar
        if (temCaminho)
        {
            animator.SetBool("Andando", true);
            ArrumarSprite();
            movimentoTimer += Time.deltaTime;

            /* Quando o relógio bater o tempo do passo, nós andamos
            if (movimentoTimer >= tempoEntrePassos)
            {
                DarUmPasso();
                
                // Zera o relógio para o próximo passo
                movimentoTimer = 0f; 
            }*/
            transform.position = math.lerp(ultimaPosicao, destinoAtual, movimentoTimer / tempoEntrePassos);
            if (movimentoTimer >= tempoEntrePassos)
            {
                DarUmPasso();
                movimentoTimer = 0;
            }

        }
    }

    private void ArrumarSprite()
    {
        int direcaoAtual = Math.Sign(destinoAtual.x - ultimaPosicao.x);

        if (direcaoAtual != 0)
        {
            if (direcaoAtual > 0 && transform.localScale.x < 0 || direcaoAtual < 0 && transform.localScale.x > 0)
                transform.localScale *= new Vector2(-1, 1);
        }
    }

    private void DarUmPasso()
    {
        // 4. Verifica se acabaram os passos
        if (caminhoParaOOuro.Count == 0)
        {
            ChegouDestino();
            return;
        }
        // 1. Pega o próximo destino
        Node proximoNo = caminhoParaOOuro[0];

        AndouTile?.Invoke(custoMovimentoAtual);
        // 2. Remove da lista para não andarmos para o mesmo lugar duas vezes
        caminhoParaOOuro.RemoveAt(0);

        ultimaPosicao = destinoAtual;
        transform.position = ultimaPosicao;
        destinoAtual = mapa.ObterPosicaoMundo(proximoNo);
        ResolverEfeitoTile();


    }

    private void ResolverEfeitoTile()
    {
        if (sistemaDeConstrucao.tileMapTemTileAt(TipoTilemap.ObstaculoLama, ultimaPosicao))
        {
            tempoEntrePassos = GameManager.Instance.VelocidadeMineiroBase * GameManager.Instance.LamaSlowDown;
            custoMovimentoAtual = GameManager.Instance.LamaSlowDown;
        }
        else
        {
            tempoEntrePassos = GameManager.Instance.VelocidadeMineiroBase;
            custoMovimentoAtual = 1;
        }
    }


    private void ChegouDestino()
    {
        Debug.Log("Cheguei no destino final!");
        temCaminho = false;
        animator.SetBool("Andando", false);
        // AoChegarNoDestino?.Invoke();
        // Aqui você pode colocar a lógica para pegar o ouro, tocar animação, etc.

        if (sistemaDeConstrucao.tileMapTemTileAt(TipoTilemap.OuroFalso, destinoAtual))
        {
            sistemaDeConstrucao.removerTile(TipoTilemap.OuroFalso, destinoAtual);
            mapa.InicializarGrid();
            AcharOuroMaisProximo();
            return;
        }
        if (sistemaDeConstrucao.tileMapTemTileAt(TipoTilemap.Ouro, destinoAtual))
        {
            funcoesCena.MineiroChegou();
            return;
        }
    }



    private void ResetarGridAEstrela()
    {
        // Percorre todos os nós do grid e reseta os valores do A*
        for (int x = 0; x < mapa.mapaBounds.size.x; x++)
        {
            for (int y = 0; y < mapa.mapaBounds.size.y; y++)
            {
                Node node = mapa.grid[x, y];
                if (node != null)
                {
                    node.gCost = int.MaxValue; // Reseta para "infinito"
                    node.hCost = 0;
                    node.parent = null;
                }
            }
        }
    }

    public bool AcharOuroMaisProximo()
    {
        ultimaPosicao = transform.position;
        destinoAtual = transform.position;
        temCaminho = true;
        List<Node> todosOurosGenerico = new List<Node>();
        todosOurosGenerico.AddRange(mapa.TodosOsOurosReais);
        todosOurosGenerico.AddRange(mapa.TodosOsOurosFalsos);
        if (mapa == null || mapa.grid == null || (mapa.TodosOsOurosFalsos.Count == 0 && mapa.TodosOsOurosReais.Count == 0))
        {
            Debug.Log("Null");
            return false;
        }

        Vector3Int pos_tile_atual = mapa.chaoTilemap.WorldToCell(transform.position);
        int startX = pos_tile_atual.x - mapa.mapaBounds.xMin;
        int startY = pos_tile_atual.y - mapa.mapaBounds.yMin;

        if (startX < 0 || startX >= mapa.mapaBounds.size.x || startY < 0 || startY >= mapa.mapaBounds.size.y)
            return false;

        Node startNode = mapa.grid[startX, startY];

        List<Node> melhorCaminhoEncontrado = null;
        int menorCustoEncontrado = int.MaxValue;

        // Roda o A* para cada ouro conhecido
        foreach (Node ouroNode in todosOurosGenerico)
        {
            List<Node> caminhoTestado = CalcularAEstrela(startNode, ouroNode);

            if (caminhoTestado != null && caminhoTestado.Count > 0)
            {
                // O custo total do caminho estará armazenado no gCost do último nó (o ouro)
                int custoDesteCaminho = ouroNode.gCost;

                if (custoDesteCaminho < menorCustoEncontrado)
                {
                    menorCustoEncontrado = custoDesteCaminho;
                    melhorCaminhoEncontrado = caminhoTestado;
                }
            }
        }

        // Se achamos pelo menos um caminho válido
        if (melhorCaminhoEncontrado != null)
        {
            temCaminho = true;
            caminhoParaOOuro = melhorCaminhoEncontrado;
            Debug.Log($"Melhor ouro escolhido! Passos: {caminhoParaOOuro.Count} | Custo Total: {menorCustoEncontrado}");
            return true;
        }
        temCaminho = false;

        return false; // Nenhum ouro é alcançável
    }

    private List<Node> ObterVizinhos(Node node)
    {
        List<Node> vizinhos = new List<Node>();
        int width = mapa.mapaBounds.size.x;
        int height = mapa.mapaBounds.size.y;

        // Direita
        if (node.gridX + 1 < width)
            vizinhos.Add(mapa.grid[node.gridX + 1, node.gridY]);
        // Esquerda
        if (node.gridX - 1 >= 0)
            vizinhos.Add(mapa.grid[node.gridX - 1, node.gridY]);
        // Cima
        if (node.gridY + 1 < height)
            vizinhos.Add(mapa.grid[node.gridX, node.gridY + 1]);
        // Baixo
        if (node.gridY - 1 >= 0)
            vizinhos.Add(mapa.grid[node.gridX, node.gridY - 1]);

        return vizinhos;
    }

    public void setGrid(MapGrid _mapa)
    {
        mapa = _mapa;
    }


    private List<Node> CalcularAEstrela(Node startNode, Node targetNode)
    {
        ResetarGridAEstrela();
        startNode.gCost = 0;   // O custo inicial deve ser 0


        List<Node> openSet = new List<Node>(); // Nós a serem avaliados
        HashSet<Node> closedSet = new HashSet<Node>(); // Nós já avaliados

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            // Acha o nó com o menor fCost na lista aberta
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Se chegou no destino, reconstrói o caminho e retorna
            if (currentNode == targetNode)
            {
                return RetornarCaminhoCalculado(startNode, targetNode);
            }

            // Verifica os vizinhos (reaproveitando o seu método ObterVizinhos)
            foreach (Node vizinho in ObterVizinhos(currentNode))
            {
                if (!vizinho.isWalkable || closedSet.Contains(vizinho))
                    continue;

                // O pulo do gato: Aqui somamos o PESO DO TILE ao custo do caminho
                int custoMovimentoAteVizinho = (int)currentNode.gCost + ObterDistanciaManhattan(currentNode, vizinho) + vizinho.movementCost;

                if (custoMovimentoAteVizinho < vizinho.gCost || !openSet.Contains(vizinho))
                {
                    vizinho.gCost = custoMovimentoAteVizinho;
                    vizinho.hCost = ObterDistanciaManhattan(vizinho, targetNode);
                    vizinho.parent = currentNode; // Migalha de pão

                    if (!openSet.Contains(vizinho))
                        openSet.Add(vizinho);
                }
            }
        }

        return null; // Não encontrou nenhum caminho possível para este ouro
    }

    // Heurística de distância para grids de 4 direções (Manhattan Distance)
    private int ObterDistanciaManhattan(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return dstX + dstY;
    }

    // Constrói a lista do caminho final
    private List<Node> RetornarCaminhoCalculado(Node inicio, Node alvo)
    {
        List<Node> caminho = new List<Node>();
        Node atual = alvo;

        while (atual != inicio)
        {
            caminho.Add(atual);
            atual = atual.parent;
        }

        caminho.Reverse();
        return caminho;
    }
    // Transforma um Node da matriz em uma posição Vector3 real no mundo


}
