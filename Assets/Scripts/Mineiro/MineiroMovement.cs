using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MineiroMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float tempoEntrePassos = 1f; 
    
    private float movimentoTimer = 0.0f;
    private Vector3 ultimaPosicao;
    public Vector3 destinoAtual;

    public MapGrid mapa;

    public List<Node> caminhoParaOOuro = new List<Node>();
    
    public UnityEvent AoChegarNoDestino;
    public UnityEvent AndouTile;


    void Start()
    {
        ultimaPosicao = transform.position;
        destinoAtual = transform.position;
    }

    void Update()
    {
        // Se temos um caminho a seguir, o relógio começa a contar
        if (caminhoParaOOuro.Count > 0)
        {
            movimentoTimer += Time.deltaTime;

            /* Quando o relógio bater o tempo do passo, nós andamos
            if (movimentoTimer >= tempoEntrePassos)
            {
                DarUmPasso();
                
                // Zera o relógio para o próximo passo
                movimentoTimer = 0f; 
            }*/
            transform.position= math.lerp(ultimaPosicao,destinoAtual,movimentoTimer/tempoEntrePassos);
            if(movimentoTimer >= tempoEntrePassos)
            {
                DarUmPasso();
                movimentoTimer =0;
            }

        }
    }

    private void DarUmPasso()
    {
        // 1. Pega o próximo destino
        Node proximoNo = caminhoParaOOuro[0];

        AndouTile?.Invoke();
        // 2. Remove da lista para não andarmos para o mesmo lugar duas vezes
        caminhoParaOOuro.RemoveAt(0);

        ultimaPosicao = destinoAtual;
        transform.position = ultimaPosicao;
        destinoAtual = mapa.ObterPosicaoMundo(proximoNo);

        // 4. Verifica se acabaram os passos
        if (caminhoParaOOuro.Count == 0)
        {
            ChegouDestino();
        }
    }


    private void ChegouDestino()
    {
        Debug.Log("Cheguei no destino final!");
        AoChegarNoDestino?.Invoke();
        // Aqui você pode colocar a lógica para pegar o ouro, tocar animação, etc.
    }

    private bool chegouDestinoAtual()
    {
        if (Vector3.Distance(transform.position,destinoAtual)<0.05f)
        {
            return true;
        }
        return false;
    }

    private void chegouDestino()
    {
        
    }

    private void Caminhar()
    {
        if (chegouDestinoAtual())
        {
            if (!PegarProximoDestino())
            {
                chegouDestino();
                return;
            }
        }

        

    }

    private bool PegarProximoDestino()
    {
        if (caminhoParaOOuro.Count > 0)
        {
            destinoAtual = mapa.ObterPosicaoMundo(caminhoParaOOuro[0]);
            caminhoParaOOuro.RemoveAt(0);
            return true;
        }
        return false;
    }


    public bool AcharOuroMaisProximo()
    {
        // Garante que o mapa foi gerado antes de buscar
        if (mapa != null && mapa.grid != null) 
        {
            Vector3 pos_atual = transform.position;
            Vector3Int pos_tile_atual = mapa.chaoTilemap.WorldToCell(pos_atual);

            // Converte a posição do Tilemap para os índices da nossa matriz
            int startX = pos_tile_atual.x - mapa.mapaBounds.xMin;
            int startY = pos_tile_atual.y - mapa.mapaBounds.yMin;

            // Proteção: verifica se o mineiro está fora dos limites do mapa
            if (startX < 0 || startX >= mapa.mapaBounds.size.x || startY < 0 || startY >= mapa.mapaBounds.size.y)
                return false;

            Node startNode = mapa.grid[startX, startY];

            // Inicializa o BFS
            Queue<Node> fila = new Queue<Node>();
            HashSet<Node> visitados = new HashSet<Node>(); // Impede que ele olhe o mesmo bloco duas vezes

            fila.Enqueue(startNode);
            visitados.Add(startNode);

            while (fila.Count > 0)
            {
                Node atual = fila.Dequeue();

                // Achou o ouro!
                if (atual.temOuro)
                {
                    ConstruirCaminho(startNode, atual);
                    return true; 
                }

                // Pega os 4 vizinhos (Cima, Baixo, Esquerda, Direita)
                List<Node> vizinhos = ObterVizinhos(atual);

                foreach (Node vizinho in vizinhos)
                {
                    // Se ainda não olhamos esse bloco e se não for uma parede...
                    if (!visitados.Contains(vizinho) && vizinho.isWalkable)
                    {
                        vizinho.parent = atual; // Deixa a "migalha de pão" para saber de onde viemos
                        visitados.Add(vizinho);
                        fila.Enqueue(vizinho);
                    }
                }
            }
        }
        
        return false; // Varreu o mapa todo e não achou nenhum ouro alcançável
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

    private void ConstruirCaminho(Node inicio, Node alvo)
    {
        caminhoParaOOuro.Clear();
        Node atual = alvo;

        // Vai voltando pelos pais até chegar no início
        while (atual != inicio)
        {
            caminhoParaOOuro.Add(atual);
            atual = atual.parent;
        }

        // Como adicionamos do ouro para o mineiro, a lista está invertida. 
        // Invertemos para que o índice 0 seja o próximo passo do mineiro.
        caminhoParaOOuro.Reverse(); 
        
        Debug.Log("Ouro encontrado! Passos até ele: " + caminhoParaOOuro.Count);
    }

    // Transforma um Node da matriz em uma posição Vector3 real no mundo
    

}
