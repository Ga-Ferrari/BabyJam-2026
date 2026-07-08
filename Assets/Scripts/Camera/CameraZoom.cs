using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private CinemachineVirtualCamera camVirtual; 
    
    // Precisamos do Tilemap para saber até onde o mapa vai!
    [SerializeField] private Tilemap mapaReferencia; 

    [Header("Configurações de Zoom")]
    [SerializeField] private float zoomMinimo = 3f; 
    [SerializeField] private float velocidadeZoom = 1f;

    // Removemos do Inspector. O script vai calcular isso sozinho agora.
    private float zoomMaximo; 

    void Start()
    {
        CalcularZoomMaximo();
    }

    public void CalcularZoomMaximo()
    {
        if (mapaReferencia == null) 
        {
            Debug.LogWarning("Faltou colocar o Tilemap no CameraZoom!");
            return;
        }

        // 1. Pega o tamanho real do seu mapa
        Bounds limitesDoMapa = mapaReferencia.localBounds;

        // 2. Descobre o máximo que a câmera pode abrir na Vertical (Eixo Y)
        float limiteMaximoY = limitesDoMapa.size.y / 2f;

        // 3. Descobre o máximo que a câmera pode abrir na Horizontal (Eixo X)
        // Precisamos dividir pela proporção da tela para compensar monitores Ultra-wide ou quadrados
        float proporcaoTela = (float)Screen.width / Screen.height; 
        float limiteMaximoX = (limitesDoMapa.size.x / 2f) / proporcaoTela;

        // 4. O Zoom Máximo perfeito é o MENOR entre esses dois limites.
        // Assim, garantimos que a câmera trava antes de vazar no eixo mais apertado.
        zoomMaximo = Mathf.Min(limiteMaximoX, limiteMaximoY);

        // Proteção: Se o mapa for muito pequenininho, evita bugar o zoom mínimo
        if (zoomMinimo > zoomMaximo) zoomMinimo = zoomMaximo;

        // Se o jogo começar com a câmera grande demais, já corta ela pro tamanho certo
        if (camVirtual.m_Lens.OrthographicSize > zoomMaximo)
        {
            camVirtual.m_Lens.OrthographicSize = zoomMaximo;
        }
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        Vector2 valorScroll = context.ReadValue<Vector2>();

        if (valorScroll.y != 0)
        {
            float direcao = Mathf.Sign(valorScroll.y);
            float tamanhoAtual = camVirtual.m_Lens.OrthographicSize;

            float novoTamanho = tamanhoAtual - (direcao * velocidadeZoom);

            // Agora o Clamp usa o limite matemático calculado lá no Start!
            novoTamanho = Mathf.Clamp(novoTamanho, zoomMinimo, zoomMaximo);

            camVirtual.m_Lens.OrthographicSize = novoTamanho;
        }
    }
}