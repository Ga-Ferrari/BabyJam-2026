using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.TextCore;
using UnityEngine.UI;
using Assets.Core;
using Unity.VisualScripting;
public class TempoLevel : MonoBehaviour
{
    [Header("Numero de passos para andar e acabar o nível")]
    [SerializeField] private int numeroPassosTotal;
    private int numeroPassos;
    [SerializeField] private Slider BarraPassos;
    public UnityEvent acabouPassos;

    [SerializeField] private SceneToTransition nextScene;
    [SerializeField] private MineiroMovement mineiro;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numeroPassos = numeroPassosTotal;
        BarraPassos.maxValue = numeroPassosTotal;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void decrementarPassos(int valorDecremento)
    {
        Debug.Log("EntrouDecremento");
        numeroPassos -= valorDecremento;
        BarraPassos.value = numeroPassosTotal - numeroPassos;
        if (numeroPassos <= 0)
        {
            Acabou();
        }

    }

    public void Acabou()
    {
        acabouPassos?.Invoke();
        TransitionManager.Instance.LoadScene(nextScene);
    }


}
