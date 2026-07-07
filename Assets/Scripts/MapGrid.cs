using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Node
{
    // --- 1. Identificação e Terreno ---
    public int gridX; // Posição X na nossa matriz
    public int gridY; // Posição Y na nossa matriz
    public bool isWalkable; // Dá para andar aqui? (False para paredes)
    public float movementCost; // Custo do terreno (Chão = 1, Lama = 3)

    // --- 2. Variáveis exclusivas do algoritmo A* ---
    public float gCost; // Custo do caminho percorrido do início até este Node
    public float hCost; // Heurística: Distância estimada deste Node até o alvo final
    
    // O Custo Total (F = G + H). O A* sempre vai escolher o Node com o menor F.
    public float FCost 
    {
        get { return gCost + hCost; }
    }

    // --- 3. A "Migalha de pão" ---
    public Node parent; // Guarda o Node de onde viemos para chegar até aqui

    // --- Construtor ---
    // Usado pela nossa função InicializarGrid() para criar a matriz
    public Node(int _gridX, int _gridY, bool _isWalkable, float _movementCost)
    {
        gridX = _gridX;
        gridY = _gridY;
        isWalkable = _isWalkable;
        movementCost = _movementCost;
    }
}

public class MapGrid : MonoBehaviour
{


    // Arraste seus diferentes tilemaps aqui pelo Inspector
    public Tilemap chaoTilemap;
    public Tilemap paredesTilemap;
    public Tilemap perigosTilemap; // Ex: espinhos, lama

    // Limites do mapa
    public BoundsInt mapaBounds;

    // A matriz lógica que o A* vai usar de verdade
    private Node[,] grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InicializarGrid()
    {
        // 1. Define o tamanho do mapa baseado no tilemap principal (chão)
        mapaBounds = chaoTilemap.cellBounds;
        grid = new Node[mapaBounds.size.x, mapaBounds.size.y];

        // 2. Percorre as coordenadas X e Y do mapa
        for (int x = mapaBounds.xMin; x < mapaBounds.xMax; x++)
        {
            for (int y = mapaBounds.yMin; y < mapaBounds.yMax; y++)
            {
                Vector3Int posTile = new Vector3Int(x, y, 0);

                // 3. Lê e processa a informação de cada camada para esta coordenada específica
                bool éAndavel = true;
                float custoMovimento = 1.0f; // Custo padrão

                // Verifica se há uma parede nesta coordenada
                if (paredesTilemap.HasTile(posTile))
                {
                    éAndavel = false; // Se tem parede, o inimigo não pode passar
                }

                // Verifica se há lama/espinhos nesta coordenada para aumentar o custo
                if (perigosTilemap.HasTile(posTile))
                {
                    custoMovimento = 3.0f; // Caminhar aqui é 3x mais "pesado" para o A*
                }

                // Convertendo a coordenada do Tilemap para o índice da nossa matriz (começa em 0,0)
                int gridX = x - mapaBounds.xMin;
                int gridY = y - mapaBounds.yMin;

                // 4. Cria o nó consolidado na memória
                grid[gridX, gridY] = new Node(gridX, gridY, éAndavel, custoMovimento);
            }
        }
    }

}
