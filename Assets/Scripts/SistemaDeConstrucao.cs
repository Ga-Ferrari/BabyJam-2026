using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SistemaDeConstrucao : MonoBehaviour
{
    public bool podeMexer = true;
    public obstaculoslevel obstaculoSelecionado;
    private Tilemap mapaProibidoPosicionar;
    private Tilemap mapaPosicionadoPeloPlayer;

    private Dictionary<Vector3Int,obstaculoslevel> obstaculoPosicionado = new Dictionary<Vector3Int, obstaculoslevel>();

    [SerializeField]private Dictionary<TipoTilemap, Tilemap> tabelaDeMapas = new Dictionary<TipoTilemap, Tilemap>();

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MapearTilemapsDaCena();
        if(tabelaDeMapas.TryGetValue(TipoTilemap.NaoPosicionavel,out Tilemap t))
            mapaProibidoPosicionar = t;
        if(tabelaDeMapas.TryGetValue(TipoTilemap.NaoPosicionavel,out Tilemap c))
            mapaPosicionadoPeloPlayer = c;
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

        if ( obstaculoSelecionado == null||obstaculoSelecionado.obstaculo == null) return;

        if (tabelaDeMapas.TryGetValue(obstaculoSelecionado.obstaculo.tipoDeMapaAlvo, out Tilemap mapaAlvo))
        {
            // 1. Pega o Vector2 da tela e converte para Vector3 do mundo usando a Câmera
            Vector3 posicaoMouseMundo = Camera.main.ScreenToWorldPoint(new Vector3(posicaoTela.x, posicaoTela.y, Camera.main.nearClipPlane));
            posicaoMouseMundo.z = 0; // Trava no plano 2D

            // 2. Converte do mundo para a grade (Grid)
            Vector3Int coordenadaGrid = mapaAlvo.WorldToCell(posicaoMouseMundo);
            Vector3Int coordenadaPlayer = mapaPosicionadoPeloPlayer.WorldToCell(posicaoMouseMundo);
            Vector3Int coordenadaNaoPode = mapaProibidoPosicionar.WorldToCell(posicaoMouseMundo);

            if (!mapaProibidoPosicionar.HasTile(coordenadaNaoPode))
            {
                if (!mapaPosicionadoPeloPlayer.HasTile(coordenadaPlayer))
                {
                    RemoverTileDoMapa(posicaoMouseMundo);
                }

                mapaAlvo.SetTile(coordenadaGrid, obstaculoSelecionado.obstaculo.tileAsset);
                mapaPosicionadoPeloPlayer.SetTile(coordenadaPlayer,obstaculoSelecionado.obstaculo.tileAsset);
                obstaculoPosicionado.Add(coordenadaPlayer,obstaculoSelecionado);
                obstaculoSelecionado.quantidade--;
                
                if (obstaculoSelecionado.quantidade <= 0)
                {
                    obstaculoSelecionado = null; // Deseleciona se acabar
                }
            }
                
        }
    }
                


    private void RemoverTileDoMapa(Vector2 posicaoMundo)
    {
        Vector3Int celula = mapaPosicionadoPeloPlayer.WorldToCell(posicaoMundo);
        if(obstaculoPosicionado.TryGetValue(celula,out obstaculoslevel obstaculo))
        {
            mapaPosicionadoPeloPlayer.SetTile(celula,null);
            
            obstaculo.quantidade++;
            obstaculoPosicionado.Remove(celula);

            if (tabelaDeMapas.TryGetValue(obstaculo.obstaculo.tipoDeMapaAlvo, out Tilemap mapaAlvo))
            {
                Vector3Int celulaAlvo = mapaAlvo.WorldToCell(posicaoMundo);
                mapaAlvo.SetTile(celulaAlvo,null);
            }
        
        }
    }


    public void DesativarMapas()
    {
        podeMexer = false;
        mapaPosicionadoPeloPlayer.GetComponent<Renderer>().enabled = false;
        mapaProibidoPosicionar.GetComponent<Renderer>().enabled = false;
    }

    public void OnPosicionar(InputAction.CallbackContext context)
    {
        if (context.performed&& podeMexer)
        {
            
            Vector2 posicaoMouseTela = Mouse.current.position.value;    
            // 2. Passa essa posição para a sua função de colocar Tile
            ColocarTileNoMapa(posicaoMouseTela);
        }
    }

    public void OnRemover(InputAction.CallbackContext context)
    {
        if (podeMexer)
        {
            Debug.Log("Entrou Remover");

            Vector2 posicaoMouseTela = Mouse.current.position.value;    
            Vector3 posicaoMouseMundo = Camera.main.ScreenToWorldPoint(new Vector3(posicaoMouseTela.x, posicaoMouseTela.y, Camera.main.nearClipPlane));
            posicaoMouseMundo.z = 0; // Trava no plano 2D
            

            RemoverTileDoMapa(posicaoMouseMundo);
        }
        
    }

}
