using TMPro;
using UnityEngine;

public class HudHora : MonoBehaviour, IObserver
{
    [SerializeField] private TMP_Text textHora, textDia;

    
    public int dia, hour, minutos;
    [SerializeField] private int horaDeInicioDelDia, horaDeFinalDelDia, minutosEnUnaHora, horasEnUnDia, diasEnUnMes;

    public float timeTimer;
    private float contenedorTimerTiempo;


    [SerializeField] private int numeroDeEstacionesTotales, estacionActualNumerica;


    [SerializeField] private GameObject objetoEstaciones;
    private Animator animatorEstaciones;



    private static HudHora instance;
    public static HudHora Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        animatorEstaciones = objetoEstaciones.gameObject.GetComponent<Animator>();

        ObserverManager.Instance.AddObserver(this);

        contenedorTimerTiempo = timeTimer;

        textHora.text = hour.ToString("0") + ":" + minutos.ToString("00");
        textDia.text = "Día " + dia.ToString("0");
    }

    
    void Update()
    {
        ActualizarHora();
    }

    void ActualizarHora()
    {
        //Paso del tiempo
        if (GameManager.Instance.pausarTiempo == false)
        {
            timeTimer -= Time.deltaTime;


            if (timeTimer <= 0)
            {
                timeTimer = contenedorTimerTiempo;
                minutos += 10;

                if (minutos >= minutosEnUnaHora)
                {
                    minutos -= minutosEnUnaHora;
                    hour++;

                    if (hour >= horasEnUnDia)
                    {
                        hour -= horasEnUnDia;
                    }

                    else if (hour >= horaDeFinalDelDia && hour < horaDeInicioDelDia)
                    {
                        ObserverManager.Instance.NotifyObserver("Day completed");
                    }
                }

                textHora.text = hour.ToString("0") + ":" + minutos.ToString("00");
            }
        }
    }

    void ActualizarDia()
    {
        dia++;

        hour = horaDeInicioDelDia;

        minutos = 0;

        timeTimer = contenedorTimerTiempo;


        if (dia > diasEnUnMes)
        {
            estacionActualNumerica++;

            dia = 1;

            if (estacionActualNumerica >= numeroDeEstacionesTotales)
            {
                estacionActualNumerica = 0;
            }

            animatorEstaciones.SetInteger("estacion", estacionActualNumerica);
        }

        textDia.text = "Día " + dia.ToString("0");

        textHora.text = hour.ToString("0") + ":" + minutos.ToString("00");
    }

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "Day completed")
        {
            ActualizarDia();
        }
    }
}