using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CicloDia : MonoBehaviour
{
    [SerializeField] private Light2D luz;
    [SerializeField] private List<EstadosDia> estadosDia; 

    void Start()
    {
        luz= GetComponent<Light2D>();
    }

    
    void Update()
    {
        
    }

    private void ComprobarEtadoDia()
    {
        //Comprobar cuanto queda hasta el siguiente estadio del dia y aplicar el cambio de la luz

        //luz.color = Color.Lerp(ColorActual, ColorNuevo, DuracionTransicion);
    }
}


[Serializable]
public class EstadosDia
{
    public int hora, minutos;

    public Color iluminacion;
}