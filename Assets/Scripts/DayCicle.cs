using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayCicle : MonoBehaviour, IObserver
{
    [SerializeField] private Light2D sunlight;
    [SerializeField] private List<EstadosDia> sunnyDayStates;

    [SerializeField] private float rainingProbability;
    [SerializeField] private List<EstadosDia> rainyDayStates;
    [SerializeField] private GameObject rain;
    private bool raining = false;

    private float timeTimer;

    private int indexDayState = 0;

    void Start()
    {
        CheckRain();

        ObserverManager.Instance.AddObserver(this);

        sunlight= GetComponent<Light2D>();

        timeTimer = HudHora.Instance.timeTimer;
    }

    
    void Update()
    {
        if (raining == false)
        {
            CheckDayState(sunnyDayStates);
        }

        else
        {
            CheckDayState(rainyDayStates);
        }
    }

    private void CheckDayState(List<EstadosDia> estadosDia)
    {
        //Comprobar cuanto queda hasta el siguiente estadio del dia y aplicar el cambio de la luz

        float horasRestantes, minutosRestantes, minutosTotales;

        if (indexDayState == estadosDia.Count - 1)
        {
            return;
        }

        else if (estadosDia[indexDayState].hora <= HudHora.Instance.hora && estadosDia[indexDayState + 1].hora > HudHora.Instance.hora)
        {
            minutosTotales = ((estadosDia[indexDayState + 1].hora - estadosDia[indexDayState].hora) * 6) * timeTimer;
            
            horasRestantes = estadosDia[indexDayState + 1].hora - HudHora.Instance.hora;

            minutosRestantes = horasRestantes * 6;

            if (HudHora.Instance.minutos != 0)
            {
                minutosRestantes -= HudHora.Instance.minutos / 10;
            }

            minutosRestantes = (minutosTotales - (minutosRestantes * timeTimer)) - HudHora.Instance.timeTimer;

            float tiempo = minutosRestantes / minutosTotales;

            sunlight.color = Color.Lerp(estadosDia[indexDayState].iluminacion, estadosDia[indexDayState + 1].iluminacion, tiempo);
        }

        else
        {
            indexDayState++;
        }        
    }

    private void CheckRain()
    {
        rain.SetActive(raining);
    }

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "Day completed")
        {
            indexDayState = 0;


            float numeroAleatorio = UnityEngine.Random.Range(0f, 100f);

            if (numeroAleatorio <= rainingProbability)
            {
                raining = true;
                ObserverManager.Instance.NotifyObserver("Rainy day");
            }

            else
            {
                raining = false;
            }

            CheckRain();
        }
    }
}


[Serializable]
public class EstadosDia
{
    public int hora;

    public Color iluminacion;
}