using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject canvasToolbar, canvasInventario, descripciones, canvasDormir, canvasControles;
    [SerializeField] Image barraVida, barraEnergia, barraComida;
    [SerializeField] TMP_Text textVida, textEnergia, textComida, textHora, textDia;

    public static bool menuDormir = false;
    bool permitirAbrirInventario;
    
    public string controles;

    

    // Variables de los dias -------------------------------------------------------------------------------------------
    [SerializeField] int dia = 1;
    int hora, minutos;
    string estacion;
    float temporizadorTiempo = 7f;
    int opcEstacion = 1;
    public int diaEstaciones;
    //------------------------------------------------------------------------------------------------------------------

    public bool pausa = false;//es lo que hace que tanto el jugador como el tiempo no pase cuando un menu esta abierto 
    
    public Jugador Player;

    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        hora = 6;
        minutos = 0;
        canvasInventario.SetActive(false);
        canvasDormir.SetActive(false);
        ActivarMenuControles();
        permitirAbrirInventario = true;
    }


    void Update()
    {
        diaEstaciones = dia;

        ComprobarEnergiaYSaludDelJugador();
        ActualizarHUD();
        AbrirInventario();
        AbrirMenuDormir();
        AbrirMenuControles();
    }

    private void ComprobarEnergiaYSaludDelJugador()
    {
        if (Player.energia <= 0 || Player.vida <= 0)
        {
            Player.StatsComienzoDiaKO();
        }
    }

    void ActualizarHUD()
    {
        ActualizarVida();
        ActualizarEnergia();
        ActualizarComida();
        ActualizarHora();
        ActualizarDia();
    }
    
    void ActualizarVida()
    {
        textVida.text = Player.vida.ToString("0") + "/" + Player.vidaMaxima.ToString("0");
        barraVida.fillAmount = Player.vida / Player.vidaMaximaHUD;
    }
    
    void ActualizarEnergia()
    {
        textEnergia.text = Player.energia.ToString("0") + "/" + Player.energiaMaxima.ToString("0");
        barraEnergia.fillAmount = Player.energia / Player.energiaMaximaHUD;
    }

    void ActualizarComida()
    {
        textComida.text = Player.comida.ToString("0") + "/" + Player.comidaMaxima.ToString("0");
        barraComida.fillAmount = Player.comida / Player.comidaMaximaHUD;
    }

    void ActualizarHora()
    {
        textHora.text = hora.ToString("0") + ":" + minutos.ToString("00");
        //Paso del tiempo
        if (pausa == false)
            temporizadorTiempo -=  Time.deltaTime;
        if(temporizadorTiempo <= 0)
        {
            temporizadorTiempo = 7f;
            minutos += 10;
        }
        if(minutos >= 60)
        {
            minutos -= 60;
            hora++;
        }
        if (hora >= 24)
            hora -= 24;
    }
    
    void ActualizarDia()
    {
        textDia.text = dia.ToString("0") + ". " + estacion;

        if (dia > 30)
        {
            dia = 1;
            opcEstacion++;
        }

        if (dia >= 30 && estacion == "Inv.")
            opcEstacion = 1;

        switch(opcEstacion)
        {
            case 1:
                estacion = "Prim.";
                break;
            
            case 2:
                estacion = "Ver.";
                break;

            case 3:
                estacion = "Oto.";
                break;

            case 4:
                estacion = "Inv.";
                break;
        }
    }
    
    void AbrirInventario()
    {
        if (Input.GetKeyDown(KeyCode.U) && permitirAbrirInventario == true)
        {
            if (canvasInventario.activeSelf)
            {
                canvasToolbar.SetActive(true);
                canvasInventario.SetActive(false);
                descripciones.SetActive(false);
                pausa = false;
            }
            else
            {
                canvasToolbar.SetActive(false);
                canvasInventario.SetActive(true);
                pausa = true;
            }
        }
    }
    
    void AbrirMenuDormir()
    {
        if (menuDormir == false)
            canvasDormir.SetActive(false);

        if (menuDormir == true)
        {
            canvasDormir.SetActive(true);
            pausa = true;
            permitirAbrirInventario = false;
        }
    }
    
    public void Dormir()
    {
        hora = 6;
        minutos = 0;
        dia++;
    }

    public void DormirSi()
    {
        dia++;
        Player.StatsComienzoDiaNormal();
        CerrarMenuDormir();
    }
    
    public void DormirNo()
    {
        CerrarMenuDormir();
    }
    
    void CerrarMenuDormir()
    {
        canvasDormir.SetActive(false);
        permitirAbrirInventario = true;
        pausa = false;
        menuDormir = false;
    }
    
    void AbrirMenuControles()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ActivarMenuControles();
        }
    }

    void ActivarMenuControles()
    {
        canvasControles.SetActive(true);
    }
    
    void CerrarMenuControles()
    {
        canvasControles.SetActive(false);
    }

    public void botonDiestro()
    {
        controles = "diestro";
        CerrarMenuControles();
    }
    
    public void botonZurdo()
    {
        controles = "zurdo";
        CerrarMenuControles();
    }
}