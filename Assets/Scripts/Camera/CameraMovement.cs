using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 10f;

    private Vector2 direcao;

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 _direcao = new Vector3(direcao.x,direcao.y,0);
        Vector3 novaPosicao = transform.position + (_direcao * velocidade * Time.deltaTime);

        // 4. Aplica a nova posição na câmera (mantendo o Z original dela intacto)
        transform.position = novaPosicao;
    }

    public void Move(Vector2 _direcao)
    {
        direcao = _direcao;
    }

}