using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))] // Garante que o objeto tem um Tilemap
public class TilemapTag : MonoBehaviour
{
    // No Inspector da cena, você escolhe qual tipo este mapa é (ex: Escolha "Paredes" no mapa de paredes)
    public TipoTilemap tipoDoMapa; 
}

public enum TipoTilemap
{
    Fundo,
    Paredes,
    Ouro,
    OuroFalso,
    NaoPosicionavel,
    PosicionadoPeloPlayer
}