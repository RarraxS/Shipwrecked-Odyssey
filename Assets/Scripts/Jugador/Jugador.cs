using UnityEngine;
using UnityEngine.InputSystem;

//Juego dedicado a Carlos Martin, Mauricio Gavidia, Carlos Saldana, Guillermo Perez y Luis Rubio por toda su ayuda durante el desarrollo

public class Jugador : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private bool mirandoDerecha = true;
    [SerializeField] private bool andando = false;
    public float vidaMaximaHUD, energiaMaximaHUD, comidaMaximaHUD;
    public int vidaMaxima, vida, energiaMaxima, energia, comida, comidaMaxima;

    private Rigidbody2D rb;

    private float temporizadorComida = 30f;

    private Animator animator;


    [SerializeField] private Controles controles;

    private static Jugador instance;
    public static Jugador Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        controles = new();
    }

    private void OnEnable()
    {
        controles.Enable();
    }

    private void OnDisable()
    {
        controles.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaMaxima = 100;
        energiaMaxima = 200;
        comidaMaxima = 200;
        StatsComienzoDiaNormal();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movimiento();

        Hambre();
    }

    

    private void Movimiento()
    {
        Vector2 direccion = new Vector2();

        direccion = controles.Base.Movimiento.ReadValue<Vector2>();

        rb.position += direccion * Time.deltaTime * velocidad;

        if (direccion != new Vector2(0, 0))
        {
            andando = true;
        }

        else
        {
            andando = false;
        }

        Animaciones(direccion);
    }

    private void Animaciones(Vector2 direccion)
    {
        if (direccion != new Vector2 (0, 0))
        {
            animator.SetBool("Andando", true);
        }

        else
        {
            animator.SetBool("Andando", false);
        }


        if ((mirandoDerecha == true && direccion.x < 0) || (mirandoDerecha == false && direccion.x > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void StatsComienzoDiaNormal()
    {
        //Estadisticas de vida al comenzar el juego cuando duermes normal
        vidaMaximaHUD = vidaMaxima;
        vida = vidaMaxima;

        //Estadisticas de la energía al comenzar el juego cuando duermes normal
        energiaMaximaHUD = energiaMaxima;
        energia = energiaMaxima;

        //Estadisticas de la comida al comenzar el juego cuando duermes normal
        comidaMaximaHUD = comidaMaxima;
        comida = comidaMaxima;
    }

    public void StatsComienzoDiaKO()
    {
        GameManager.Instance.Dormir();
        //Estadisticas de vida al comenzar el juego cuando te quedas KO
        vidaMaximaHUD = vidaMaxima;
        vida = vidaMaxima;

        //Estadisticas de la energía al comenzar el juego cuando te quedas KO
        energiaMaximaHUD = energiaMaxima;
        energia = energiaMaxima / 2;

        //Estadisticas de la comida al comenzar el juego cuando te quedas KO
        comidaMaximaHUD = comidaMaxima;
        comida = comidaMaxima;
    }

    void Hambre()
    {
        if(GameManager.Instance.pausarTiempo == false)
            temporizadorComida -= Time.deltaTime;

        if( temporizadorComida <= 0 )
        {
            comida -= 10;
            temporizadorComida = 30f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemigo")
            vida -= 10;

        if (collision.tag == "Cama")
            GameManager.Instance.menuDormir = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Tilemap con colisiones")
            print(collision.transform.name);

    }
}