using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class FuncoesCena : MonoBehaviour
{

    [SerializeField]private NavMeshSurface nav;
    [SerializeField]private MineiroMovement mineiro;

    private SistemaDeConstrucao sistemaDeConstrucao;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0;
        sistemaDeConstrucao = GetComponent<SistemaDeConstrucao>();
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

        sistemaDeConstrucao.DesativarMapas();
        // 2. Pega o componente do agente no mineiro e DESLIGA ele
        NavMeshAgent agenteDoMineiro = mineiro.GetComponent<NavMeshAgent>();
        if (agenteDoMineiro != null) agenteDoMineiro.enabled = false;

        // 3. Cozinha (Bake) o novo mapa com os obstáculos
        nav.BuildNavMesh();

        // 4. Espera a física do jogo atualizar (Isso é crucial após mudar o TimeScale)
        yield return new WaitForFixedUpdate();

        // 5. LIGA o agente novamente. Ao ser ligado, ele é forçado a reconhecer o chão novo.
        if (agenteDoMineiro != null) agenteDoMineiro.enabled = true;

        // 6. Espera só mais 1 frame normal para garantir que ele está pronto
        yield return null;

        // 7. Agora sim! O agente está firme no chão e calcula perfeitamente
        mineiro.CalcularDestino();
    }

    public void timeUnfreeze()
    {
        Time.timeScale = 1;
    }

}
