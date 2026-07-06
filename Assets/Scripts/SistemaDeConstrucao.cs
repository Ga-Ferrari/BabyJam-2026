using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SistemaDeConstrucao : MonoBehaviour
{

    public obstaculoslevel obstaculoSelecionado;
    [SerializeField]private Dictionary<TipoTilemap, Tilemap> tabelaDeMapas = new Dictionary<TipoTilemap, Tilemap>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MapearTilemapsDaCena();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SelecionarObstaculo(obstaculoslevel dadosObstaculo)
    {
        if (dadosObstaculo!=null && dadosObstaculo == obstaculoSelecionado)
        {
            obstaculoSelecionado = null;
            return;
        }

        if (dadosObstaculo.quantidade > 0)
        {
            obstaculoSelecionado = dadosObstaculo;
        }
    }

    private void MapearTilemapsDaCena()
    {
        // Procura na cena atual todos os objetos que possuem o script TilemapTag
        TilemapTag[] mapasAchados = FindObjectsByType<TilemapTag>();

        foreach (TilemapTag mapa in mapasAchados)
        {
            // Pega o componente Tilemap real daquele objeto
            Tilemap tilemapComponente = mapa.GetComponent<Tilemap>();

            // Salva na nossa tabela para uso posterior
            if (!tabelaDeMapas.ContainsKey(mapa.tipoDoMapa))
            {
                tabelaDeMapas.Add(mapa.tipoDoMapa, tilemapComponente);
            }
        }
    }
    private void ColocarTileNoMapa(Vector2 posicaoTela)
{
    if (obstaculoSelecionado == null) return;

    if (tabelaDeMapas.TryGetValue(obstaculoSelecionado.obstaculo.tipoDeMapaAlvo, out Tilemap mapaAlvo))
    {
        // 1. Pega o Vector2 da tela e converte para Vector3 do mundo usando a Câmera
        Vector3 posicaoMouseMundo = Camera.main.ScreenToWorldPoint(new Vector3(posicaoTela.x, posicaoTela.y, Camera.main.nearClipPlane));
        posicaoMouseMundo.z = 0; // Trava no plano 2D

        // 2. Converte do mundo para a grade (Grid)
        Vector3Int coordenadaGrid = mapaAlvo.WorldToCell(posicaoMouseMundo);

        // 3. Coloca a Tile se estiver vazio
        if (!mapaAlvo.HasTile(coordenadaGrid))
        {
            mapaAlvo.SetTile(coordenadaGrid, obstaculoSelecionado.obstaculo.tileAsset);
            obstaculoSelecionado.quantidade--;
            
            if (obstaculoSelecionado.quantidade <= 0)
            {
                obstaculoSelecionado = null; // Deseleciona se acabar
            }
        }
    }
}

    public void OnPosicionar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            Vector2 posicaoMouseTela = Mouse.current.position.value;    
            // 2. Passa essa posição para a sua função de colocar Tile
            ColocarTileNoMapa(posicaoMouseTela);
            Debug.Log("Clicado");
        }
    }

}
