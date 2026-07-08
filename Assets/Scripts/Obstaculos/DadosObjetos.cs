using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NovoObstaculo", menuName = "BabyJam 2026/Criar Novo Obstaculo")]
public class DadosDoObstaculo : ScriptableObject
{
    public string nomeDoObstaculo;
    public Sprite icone;
    
    [Header("Configurações do Tilemap")]
    public TipoTilemap tipoDeMapaAlvo; 
    public TileBase tileAsset;  
}

