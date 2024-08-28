using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CicloDia : MonoBehaviour
{
    [SerializeField] private Light2D luz;
    [SerializeField] private List<EstadosDia> estadosDia;

    private int i = 0;

    void Start()
    {
        luz= GetComponent<Light2D>();
    }

    
    void Update()
    {
        ComprobarEstadoDia();
    }

    private void ComprobarEstadoDia()
    {
        //Comprobar cuanto queda hasta el siguiente estadio del dia y aplicar el cambio de la luz

        float horasRestantes, minutosRestantes, minutosTotales;

        if (i == estadosDia.Count - 1)
        {
            return;
        }

        else if (estadosDia[i].hora <= HudHora.Instance.hora && estadosDia[i + 1].hora > HudHora.Instance.hora)
        {
            minutosTotales = (estadosDia[i + 1].hora - estadosDia[i].hora) * 60;
            
            horasRestantes = estadosDia[i + 1].hora - HudHora.Instance.hora;

            minutosRestantes = horasRestantes * 60;

            if (HudHora.Instance.minutos != 0)
            {
                minutosRestantes -= HudHora.Instance.minutos;
            }

            minutosRestantes = minutosTotales - minutosRestantes;

            float tiempo = minutosRestantes / minutosTotales;

            luz.color = Color.Lerp(estadosDia[i].iluminacion, estadosDia[i + 1].iluminacion, tiempo);
        }

        else
        {
            i++;
        }        
    }
}


[Serializable]
public class EstadosDia
{
    public int hora;

    public Color iluminacion;
}