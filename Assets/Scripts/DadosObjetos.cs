using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NovoObstaculo", menuName = "Meu Jogo/Criar Novo Obstaculo")]
public class DadosDoObstaculo : ScriptableObject
{
    public string nomeDoObstaculo;
    public Sprite icone;
    public int quantidade; // Quantidade inicial que o jogador terá
    
    [Header("Configurações do Tilemap")]
    public TipoTilemap tipoDeMapaAlvo; 
    public TileBase tileAsset;  
}
