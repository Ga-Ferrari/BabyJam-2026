using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using JetBrains.Annotations;

public class Node
{
    // --- 1. Identificação e Terreno ---
    public int gridX; // Posição X na nossa matriz
    public int gridY; // Posição Y na nossa matriz
    public bool isWalkable; // Dá para andar aqui? (False para paredes)
    public int movementCost; // Custo do terreno (Chão = 1, Lama = 3)
    public bool temOuro;
    public bool temOuroFalso;

    // --- 2. Variáveis exclusivas do algoritmo A* ---
    public int gCost; // Custo do caminho percorrido do início até este Node
    public int hCost; // Heurística: Distância estimada deste Node até o alvo final
    
    // O Custo Total (F = G + H). O A* sempre vai escolher o Node com o menor F.
    public int FCost 
    {
        get { return gCost + hCost; }
    }

    // --- 3. A "Migalha de pão" ---
    public Node parent; 

    // --- Construtor ---
    // Usado pela nossa função InicializarGrid() para criar a matriz
    public Node(int _gridX, int _gridY, bool _isWalkable, int _movementCost,bool _temOuro)
    {
        gridX = _gridX;
        gridY = _gridY;
        isWalkable = _isWalkable;
        movementCost = _movementCost;
        temOuro = _temOuro;
    }
}

public class MapGrid : MonoBehaviour
{
    public Tilemap chaoTilemap;
    public Tilemap paredesTilemap;
    public Tilemap LamaTilemap; // Ex: espinhos, lama
    public Tilemap ouroTilemap;
    public Tilemap ouroFalsoTilemap;

    public BoundsInt mapaBounds;

    public Node[,] grid;

    public List<Node> TodosOsOurosReais = new List<Node>();
    public List<Node> TodosOsOurosFalsos = new List<Node>();

    public void InicializarGrid()
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
                int custoMovimento = 1; // Custo padrão
                bool temOuro = false;
                // Verifica se há uma parede nesta coordenada
                if (paredesTilemap.HasTile(posTile))
                {
                    éAndavel = false; // Se tem parede, o inimigo não pode passar
                }

                // Verifica se há lama/espinhos nesta coordenada para aumentar o custo
                if (LamaTilemap&&LamaTilemap.HasTile(posTile))
                {
                    custoMovimento = GameManager.Instance.LamaSlowDown; // Caminhar aqui é 3x mais "pesado" para o A*
                }

                if (ouroTilemap.HasTile(posTile)||ouroFalsoTilemap.HasTile(posTile))
                {
                    temOuro = true;
                }

                // Convertendo a coordenada do Tilemap para o índice da nossa matriz (começa em 0,0)
                int gridX = x - mapaBounds.xMin;
                int gridY = y - mapaBounds.yMin;

                // 4. Cria o nó consolidado na memória
                grid[gridX, gridY] = new Node(gridX, gridY, éAndavel,(int) custoMovimento,temOuro);
                if (grid[gridX, gridY].temOuro)
                {
                    TodosOsOurosReais.Add(grid[gridX,gridY]);
                }
                if (grid[gridX, gridY].temOuroFalso)
                {
                    TodosOsOurosFalsos.Add(grid[gridX,gridY]);
                }
            }
        }
    }

    public Vector3 ObterPosicaoMundo(Node node)
    {
        // 1. Reverte o índice da matriz para a coordenada interna do Tilemap
        int tileX = node.gridX + mapaBounds.xMin;
        int tileY = node.gridY + mapaBounds.yMin;
        
        Vector3Int posTile = new Vector3Int(tileX, tileY, 0);

        // 2. A Unity calcula automaticamente o centro físico daquele Tile no mundo
        return chaoTilemap.GetCellCenterWorld(posTile);
    }

}
