using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Cinemachine;

public class FuncoesCena : MonoBehaviour
{

    [SerializeField]private MineiroMovement mineiro;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private GameObject MenuPerder;
    private SistemaDeConstrucao sistemaDeConstrucao;
    private MapGrid map;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sistemaDeConstrucao = GetComponent<SistemaDeConstrucao>();
        map = GetComponent<MapGrid>();
    }

    public void PlayTheGame()
    {
        // Em vez de rodar tudo de uma vez, iniciamos a sequência de eventos
        StartCoroutine(SequenciaDeInicio());
    }

    private IEnumerator SequenciaDeInicio()
    {
        // 1. Volta o tempo ao normal para a física voltar a funcionar
        timeUnfreeze();
        camera.Follow = mineiro.transform;
        sistemaDeConstrucao.AplicarMudancas();
        yield return null;
        map.InicializarGrid();
        yield return null;
        mineiro.setGrid(map);
        mineiro.AcharOuroMaisProximo();
    }

    public void MineiroChegou()
    {
        if (sistemaDeConstrucao.tileMapTemTileAt(TipoTilemap.OuroFalso, mineiro.destinoAtual))
        {
            sistemaDeConstrucao.removerTile(TipoTilemap.OuroFalso,mineiro.transform.position);
            map.InicializarGrid();
            mineiro.AcharOuroMaisProximo();
        }
        if (sistemaDeConstrucao.tileMapTemTileAt(TipoTilemap.Ouro, mineiro.destinoAtual))
        {
            Perdeu();
        }
    }

    private void Perdeu()
    {
        CanvasGroup canva = MenuPerder.GetComponent<CanvasGroup>();
        canva.alpha = 1f;
        canva.interactable = false;  // Impede que o botão reaja a cliques
        canva.blocksRaycasts = false;// Faz o clique do mouse "atravessar" o botão
        Time.timeScale = 0;
    }

    public void timeUnfreeze()
    {
        Time.timeScale = 1;
    }

    public void Ganhou()
    {
        Time.timeScale = 0;
    }

}
