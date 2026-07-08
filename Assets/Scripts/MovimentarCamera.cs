using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    [SerializeField]private CameraController cameraMovimentador;
    [SerializeField] private CinemachineVirtualCamera cameraCM ;
    [SerializeField] private SistemaDeConstrucao sistema;

    [SerializeField] private float velocidadeZoom;
    [SerializeField] private float limiteZoomBaixo;
    [SerializeField] private float limiteZoomAlto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        sistema.OnPosicionar();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        sistema.OnRemover();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direcao = context.ReadValue<Vector2>();
        cameraMovimentador.Move(direcao);
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        CameraZoom CZ = cameraCM.GetComponent<CameraZoom>();
        CZ.OnScroll(context);
    }
    
}
