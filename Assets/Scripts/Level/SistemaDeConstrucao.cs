using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Tilemaps;
using UnityEditor;
using Cinemachine;

public class SistemaDeConstrucao : MonoBehaviour
{
    
    [NonSerialized]public bool podeMexer = true;

    [Header("Configuração do nível")]
    [SerializeField] private bool podePosicionarEmParedes;

    private obstaculoslevel obstaculoSelecionado;
    [SerializeField] private DadosDoObstaculo obstaculoNaoPosicionavel;
    private Tilemap mapaProibidoPosicionar;
    private Tilemap mapaPosicionadoPeloPlayer;
    private Dictionary<Vector3Int,obstaculoslevel> obstaculoPosicionado = new Dictionary<Vector3Int, obstaculoslevel>();

    private Dictionary<TipoTilemap, Tilemap> tabelaDeMapas = new Dictionary<TipoTilemap, Tilemap>();


    

    void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MapearTilemapsDaCena();
        if(tabelaDeMapas.TryGetValue(TipoTilemap.NaoPosicionavel,out Tilemap t))
            mapaProibidoPosicionar = t;
        if(tabelaDeMapas.TryGetValue(TipoTilemap.PosicionadoPeloPlayer,out Tilemap c))
            mapaPosicionadoPeloPlayer = c;
        BloquearPosicionamentos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TipoTilemap TipoDaTileAt(Vector3 posicao)
    {
        tabelaDeMapas.TryGetValue(TipoTilemap.Fundo,out Tilemap c);
        Vector3Int posicaoCelula = c.WorldToCell(posicao);
        
        tabelaDeMapas.TryGetValue(TipoTilemap.OuroFalso,out Tilemap of);
        if(of.HasTile(posicaoCelula))return TipoTilemap.OuroFalso;
        return TipoTilemap.NaoPosicionavel;
    }

    public bool tileMapTemTileAt(TipoTilemap tipo,Vector3 posicao)
    {
        if(tabelaDeMapas.TryGetValue(tipo,out Tilemap t))
        {
            return t.HasTile(t.WorldToCell(posicao));
        }
        return false;
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

    private void BloquearPosicionamentos()
    {
        if(tabelaDeMapas.TryGetValue(TipoTilemap.Ouro,out Tilemap mapaOuro))
        {
            Debug.Log("Achou na tabela");
            foreach (Vector3Int posicao in mapaOuro.cellBounds.allPositionsWithin)
            {
                Debug.Log("Loopando");
                if (mapaOuro.HasTile(posicao))
                {
                    Debug.Log("Antes do posicionamento");
                    mapaProibidoPosicionar.SetTile(posicao,obstaculoNaoPosicionavel.tileAsset);
                    Debug.Log("Ouro na posicao"+posicao);
                }
            }
        }
        if (!podePosicionarEmParedes)
        {
            if(tabelaDeMapas.TryGetValue(TipoTilemap.Paredes,out Tilemap mapaParede))
            {
                foreach (Vector3Int posicao in mapaParede.cellBounds.allPositionsWithin)
                {
                    if (mapaParede.HasTile(posicao))
                    {
                        mapaProibidoPosicionar.SetTile(posicao,obstaculoNaoPosicionavel.tileAsset);
                    }
                }
            }
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

        // 1. Pega o Vector2 da tela e converte para Vector3 do mundo usando a Câmera
        Vector3 posicaoMouseMundo = Camera.main.ScreenToWorldPoint(new Vector3(posicaoTela.x, posicaoTela.y, Camera.main.nearClipPlane));
        posicaoMouseMundo.z = 0; // Trava no plano 2D

        Vector3Int coordenadaPlayer = mapaPosicionadoPeloPlayer.WorldToCell(posicaoMouseMundo);
        Vector3Int coordenadaNaoPode = mapaProibidoPosicionar.WorldToCell(posicaoMouseMundo);

        if (!mapaProibidoPosicionar.HasTile(coordenadaNaoPode))
        {
            if (mapaPosicionadoPeloPlayer.HasTile(coordenadaPlayer))
            {
                RemoverTileDoMapa(posicaoMouseMundo);
            }
            mapaPosicionadoPeloPlayer.SetTile(coordenadaPlayer,obstaculoSelecionado.obstaculo.tileAsset);
            obstaculoPosicionado.Add(coordenadaPlayer,obstaculoSelecionado);
            obstaculoSelecionado.quantidade--;
            
            if (obstaculoSelecionado.quantidade <= 0)
            {
                obstaculoSelecionado = null; // Deseleciona se acabar
            }
        }
                
        
    }
                
    public void removerTile(TipoTilemap tipo,Vector3 posicao)
    {
        if(tabelaDeMapas.TryGetValue(tipo,out Tilemap t))
        {
            t.SetTile(t.WorldToCell(posicao),null);
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
        
        }
    }


    public void DesativarMapas()
    {
        podeMexer = false;
        mapaPosicionadoPeloPlayer.GetComponent<Renderer>().enabled = false;
        mapaProibidoPosicionar.GetComponent<Renderer>().enabled = false;
    }

    public void AplicarMudancas()
    {   
        DesativarMapas();
        foreach( (Vector3Int posicao,obstaculoslevel obstaculo) in obstaculoPosicionado)
        {
            posicionarTile(posicao,obstaculo.obstaculo);
        }
        
    }

    private bool posicionarTile(Vector3Int pos,DadosDoObstaculo _dadosDoObstaculo)
    {
        if(tabelaDeMapas.TryGetValue(_dadosDoObstaculo.tipoDeMapaAlvo,out Tilemap alvo))
        {
            alvo.SetTile(pos,_dadosDoObstaculo.tileAsset);
            return true;
        }

        return false;

    }

    public void OnPosicionar()
    {
        Debug.Log("Ação feita");
        if (podeMexer)
        {
            Vector2 posicaoMouseTela = Mouse.current.position.value;    
            ColocarTileNoMapa(posicaoMouseTela);
        }
    }

    public void OnRemover()
    {
        if (podeMexer)
        {
            Vector2 posicaoMouseTela = Mouse.current.position.value;    
            Vector3 posicaoMouseMundo = Camera.main.ScreenToWorldPoint(new Vector3(posicaoMouseTela.x, posicaoMouseTela.y, Camera.main.nearClipPlane));
            posicaoMouseMundo.z = 0; // Trava no plano 2D
            

            RemoverTileDoMapa(posicaoMouseMundo);
        }
        
    }

}
