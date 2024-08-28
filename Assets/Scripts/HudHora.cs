using TMPro;
using UnityEngine;

public class HudHora : MonoBehaviour, IObserver
{
    [SerializeField] private TMP_Text textHora, textDia;

    
    public int dia, hora, minutos;
    [SerializeField] private int horaDeInicioDelDia, horaDeFinalDelDia, minutosEnUnaHora, horasEnUnDia, diasEnUnMes;

    [SerializeField] private float temporizadorPasoDelTiempo;
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

        contenedorTimerTiempo = temporizadorPasoDelTiempo;

        textHora.text = hora.ToString("0") + ":" + minutos.ToString("00");
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
            temporizadorPasoDelTiempo -= Time.deltaTime;


            if (temporizadorPasoDelTiempo <= 0)
            {
                temporizadorPasoDelTiempo = contenedorTimerTiempo;
                minutos += 10;

                if (minutos >= minutosEnUnaHora)
                {
                    minutos -= minutosEnUnaHora;
                    hora++;

                    if (hora >= horasEnUnDia)
                    {
                        hora -= horasEnUnDia;
                    }

                    else if (hora >= horaDeFinalDelDia && hora < horaDeInicioDelDia)
                    {
                        ObserverManager.Instance.NotifyObserver("dia completado");
                    }
                }

                textHora.text = hora.ToString("0") + ":" + minutos.ToString("00");
            }
        }
    }

    void ActualizarDia()
    {
        dia++;

        hora = horaDeInicioDelDia;


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
    }

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "dia completado")
        {
            ActualizarDia();
        }
    }
}