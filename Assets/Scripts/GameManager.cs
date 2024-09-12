using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject canvasToolbar, canvasInventario, descripciones, canvasDormir, canvasControles;
    [SerializeField] Image barraVida, barraEnergia, barraComida;
    [SerializeField] TMP_Text textVida, textEnergia, textComida;

    public bool menuDormir = false;
    bool permitirAbrirInventario;
    

    public string controles;
    


    // Variables para recolectar objetos-------------------------------------------------

    public bool permitirUsarHerramineta;

    //-----------------------------------------------------------------------------------


    public bool pausarTiempo = false;//Es lo que hace que tanto el jugador como el tiempo no pase cuando un menu esta abierto 
    
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
        canvasInventario.SetActive(false);
        canvasDormir.SetActive(false);
        ActivarMenuControles();
        permitirAbrirInventario = true;
    }


    void Update()
    {
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

    
    
    
    
    void AbrirInventario()
    {
        if (Input.GetKeyDown(KeyCode.U) && permitirAbrirInventario == true)
        {
            if (canvasInventario.activeSelf)
            {
                ObserverManager.Instance.NotifyObserver("Cambio en la toolbar");
                canvasToolbar.SetActive(true);
                canvasInventario.SetActive(false);
                descripciones.SetActive(false);
                pausarTiempo = false;
            }
            else
            {
                canvasToolbar.SetActive(false);
                canvasInventario.SetActive(true);
                pausarTiempo = true;
            }
        }
    }

    #region Dormir
    void AbrirMenuDormir()
    {
        if (menuDormir == false)
            canvasDormir.SetActive(false);

        if (menuDormir == true)
        {
            canvasDormir.SetActive(true);
            pausarTiempo = true;
            permitirAbrirInventario = false;
        }
    }
    
    public void Dormir()
    {
        //hora = 6;
        //minutos = 0;
        //dia++;
    }

    public void DormirSi()
    {
        //dia++;
        Player.StatsComienzoDiaNormal();
        CerrarMenuDormir();
        ObserverManager.Instance.NotifyObserver("dia completado");
    }
    
    public void DormirNo()
    {
        CerrarMenuDormir();
    }
    
    void CerrarMenuDormir()
    {
        canvasDormir.SetActive(false);
        permitirAbrirInventario = true;
        pausarTiempo = false;
        menuDormir = false;
    }
    #endregion

    #region Menu controles
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
    #endregion
}