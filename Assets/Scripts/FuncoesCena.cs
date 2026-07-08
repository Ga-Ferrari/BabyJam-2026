using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Cinemachine;

public class FuncoesCena : MonoBehaviour
{

    [SerializeField]private MineiroMovement mineiro;
    [SerializeField] private CinemachineVirtualCamera camera;
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
            Debug.Log("Tentando ir para o proximo lugar");
        }
        if (sistemaDeConstrucao.tileMapTemTileAt(TipoTilemap.Ouro, mineiro.destinoAtual))
        {
            Debug.Log("Você perdeu");
        }
    }

    public void timeUnfreeze()
    {
        Time.timeScale = 1;
    }

}
