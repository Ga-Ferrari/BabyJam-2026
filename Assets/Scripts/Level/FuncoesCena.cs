using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Cinemachine;
using Assets.Core;

public class FuncoesCena : MonoBehaviour
{

    [SerializeField] private MineiroMovement mineiro;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private GameObject MenuPerder;
    [SerializeField] private string defeatSceneName;
    private SistemaDeConstrucao sistemaDeConstrucao;
    private bool iniciou = false;
    private MapGrid map;

    public GameObject loseCanvas;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sistemaDeConstrucao = GetComponent<SistemaDeConstrucao>();
        map = GetComponent<MapGrid>();
    }

    public void PlayTheGame()
    {
        if (!iniciou) StartCoroutine(SequenciaDeInicio());
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
        iniciou = true;
    }

    public void MineiroChegou()
    {
        // if (sistemaDeConstrucao.tileMapTemTileAt(TipoTilemap.OuroFalso, mineiro.destinoAtual))
        // {
        //     sistemaDeConstrucao.removerTile(TipoTilemap.OuroFalso, mineiro.destinoAtual);
        //     map.InicializarGrid();

        //     mineiro.AcharOuroMaisProximo();
        //     return;
        // }

        // CanvasGroup canva = MenuPerder.GetComponent<CanvasGroup>();
        // loseCanvas.SetActive(true);


        // CanvasGroup meuCanvasGroup = MenuPerder.GetComponent<CanvasGroup>();

        // if (meuCanvasGroup != null)
        // {
        //     meuCanvasGroup.alpha = 1f;
        //     meuCanvasGroup.interactable = true;
        //     meuCanvasGroup.blocksRaycasts = true;
        // }

        TransitionManager.Instance.LoadScene(defeatSceneName);
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
