using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class MineiroMovement : MonoBehaviour
{

    [SerializeField] private Tilemap mapaDoOuro;

    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();    
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.SetDestination(EncontrarOuroMaisProximoPorCaminho());
        
    }

    // Update is called once per frame
    void Update()
    {
        //agent.SetDestination(target.position);
    }


    public Vector3 EncontrarOuroMaisProximoPorCaminho()
    {
        Vector3 posicaoMineiro = transform.position;
        Vector3 posicaoMaisProxima = Vector3.zero;
        
        float menorCaminhoReal = Mathf.Infinity; 
        bool achouOuro = false;

        NavMeshPath caminhoSimulado = new NavMeshPath();

        foreach (Vector3Int posicaoGrid in mapaDoOuro.cellBounds.allPositionsWithin)
        {
            if (mapaDoOuro.HasTile(posicaoGrid))
            {
                Vector3 posicaoMundo = mapaDoOuro.GetCellCenterWorld(posicaoGrid);

                if (NavMesh.CalculatePath(posicaoMineiro, posicaoMundo, NavMesh.AllAreas, caminhoSimulado))
                {
                    if (caminhoSimulado.status == NavMeshPathStatus.PathComplete)
                    {
                        float comprimentoDoCaminho = CalcularComprimentoDoCaminho(caminhoSimulado);

                        if (comprimentoDoCaminho < menorCaminhoReal)
                        {
                            menorCaminhoReal = comprimentoDoCaminho;
                            posicaoMaisProxima = posicaoMundo;
                            achouOuro = true;
                        }
                    }
                }
            }
        }

        if (achouOuro) return posicaoMaisProxima;
        
        return posicaoMineiro; // Fica parado se não houver caminho alcançável
    }

    private float CalcularComprimentoDoCaminho(NavMeshPath caminho)
    {
        // Se o caminho tem menos de 2 pontos, a distância é zero
        if (caminho.corners.Length < 2) return 0f;

        float distanciaTotal = 0f;

        // 'corners' são os nós/vértices do caminho calculado pelo NavMesh
        for (int i = 1; i < caminho.corners.Length; i++)
        {
            // Soma a distância entre o ponto anterior e o ponto atual
            distanciaTotal += Vector3.Distance(caminho.corners[i - 1], caminho.corners[i]);
        }

        return distanciaTotal;
    }

}
