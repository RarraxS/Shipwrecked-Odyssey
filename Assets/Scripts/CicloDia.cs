using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CicloDia : MonoBehaviour, IObserver
{
    [SerializeField] private Light2D luz;
    [SerializeField] private List<EstadosDia> estadosDiaSoleado;


    [SerializeField] private float probabilidadDeLlover;
    [SerializeField] private List<EstadosDia> estadosDiaLluvioso;
    [SerializeField] private GameObject lluvia;
    private bool lloviendo = false;


    private float contenedorDeTiempo;

    private int i = 0;

    void Start()
    {
        ComprobarEstadoLluvia();

        ObserverManager.Instance.AddObserver(this);

        luz= GetComponent<Light2D>();

        contenedorDeTiempo = HudHora.Instance.temporizadorPasoDelTiempo;
    }

    
    void Update()
    {
        if (lloviendo == false)
        {
            ComprobarEstadoDia(estadosDiaSoleado);
        }

        else
        {
            ComprobarEstadoDia(estadosDiaLluvioso);
        }
    }

    private void ComprobarEstadoDia(List<EstadosDia> estadosDia)
    {
        //Comprobar cuanto queda hasta el siguiente estadio del dia y aplicar el cambio de la luz

        float horasRestantes, minutosRestantes, minutosTotales;

        if (i == estadosDia.Count - 1)
        {
            return;
        }

        else if (estadosDia[i].hora <= HudHora.Instance.hora && estadosDia[i + 1].hora > HudHora.Instance.hora)
        {
            minutosTotales = ((estadosDia[i + 1].hora - estadosDia[i].hora) * 6) * contenedorDeTiempo;
            
            horasRestantes = estadosDia[i + 1].hora - HudHora.Instance.hora;

            minutosRestantes = horasRestantes * 6;

            if (HudHora.Instance.minutos != 0)
            {
                minutosRestantes -= HudHora.Instance.minutos / 10;
            }

            minutosRestantes = (minutosTotales - (minutosRestantes * contenedorDeTiempo)) - HudHora.Instance.temporizadorPasoDelTiempo;

            float tiempo = minutosRestantes / minutosTotales;

            luz.color = Color.Lerp(estadosDia[i].iluminacion, estadosDia[i + 1].iluminacion, tiempo);
        }

        else
        {
            i++;
        }        
    }

    private void ComprobarEstadoLluvia()
    {
        lluvia.SetActive(lloviendo);
    }

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "dia completado")
        {
            i = 0;


            float numeroAleatorio = UnityEngine.Random.Range(0f, 100f);

            if (numeroAleatorio <= probabilidadDeLlover)
            {
                lloviendo = true;
            }

            else
            {
                lloviendo = false;
            }

            ComprobarEstadoLluvia();
        }
    }
}


[Serializable]
public class EstadosDia
{
    public int hora;

    public Color iluminacion;
}