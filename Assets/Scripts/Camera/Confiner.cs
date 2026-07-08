using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine; // Necessário para acessar o Confiner

[RequireComponent(typeof(PolygonCollider2D))]
public class AjustarConfinerAoTilemap : MonoBehaviour
{
    [Header("Referências")]
    public Tilemap tilemapReferencia;
    
    [Tooltip("Arraste a sua Câmera Virtual que contém o componente CinemachineConfiner2D aqui")]
    public CinemachineConfiner2D confinerDaCamera;

    void Start()
    {
        AjustarTamanhoDoPoligono();
    }

    public void AjustarTamanhoDoPoligono()
    {
        if (tilemapReferencia == null)
        {
            Debug.LogWarning("Tilemap de referência não foi atribuído!");
            return;
        }

        PolygonCollider2D colisor = GetComponent<PolygonCollider2D>();
        
        // Pega os limites exatos de onde existem tiles desenhados
        Bounds bounds = tilemapReferencia.localBounds;
        Transform tilemapTransform = tilemapReferencia.transform;

        Vector2[] pontos = new Vector2[4];

        // Mapeia os 4 cantos. 
        // Usamos TransformPoint e InverseTransformPoint para garantir que os pontos 
        // fiquem no lugar certo, não importa onde o GameObject do Colisor esteja.
        
        // Canto Inferior Esquerdo
        pontos[0] = transform.InverseTransformPoint(tilemapTransform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, 0)));
        
        // Canto Superior Esquerdo
        pontos[1] = transform.InverseTransformPoint(tilemapTransform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, 0)));
        
        // Canto Superior Direito
        pontos[2] = transform.InverseTransformPoint(tilemapTransform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, 0)));
        
        // Canto Inferior Direito
        pontos[3] = transform.InverseTransformPoint(tilemapTransform.TransformPoint(new Vector3(bounds.max.x, bounds.min.y, 0)));

        // Aplica os novos 4 pontos ao colisor
        colisor.points = pontos;

        // O SEGREDO DO CINEMACHINE: Avisa que o polígono mudou de tamanho
        if (confinerDaCamera != null)
        {
            confinerDaCamera.InvalidateCache();
        }
    }
}