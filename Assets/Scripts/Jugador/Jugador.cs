using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//Dedicado a Carlos Martín, Mauricio Gavidia y Carlos Saldaña

public class Jugador : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private bool mirandoDerecha = true;
    [SerializeField] private bool andando = false;
    [SerializeField] public float vidaMaximaHUD, energiaMaximaHUD, comidaMaximaHUD;
    [SerializeField] public int vidaMaxima, vida, energiaMaxima, energia, comida, comidaMaxima;

    private Rigidbody2D rb;

    float temporizadorComida = 30f;

    Animator animator;

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
        Animaciones();
        Hambre();
    }


    void Movimiento()
    {
        Vector2 movimiento = new Vector2(0, 0);

        if (((Input.GetKey(KeyCode.I) && GameManager.Instance.controles == "zurdo") ||
            (Input.GetKey(KeyCode.W) && GameManager.Instance.controles == "diestro")) && GameManager.Instance.menuAbierto == false)
        {
            movimiento += (Vector2)(transform.up);//Alante
            andando = true;
        }

        if (((Input.GetKey(KeyCode.K) && GameManager.Instance.controles == "zurdo") ||
            (Input.GetKey(KeyCode.S) && GameManager.Instance.controles == "diestro")) && GameManager.Instance.menuAbierto == false)
        {
            movimiento += (Vector2)(-transform.up);//Atrás
            andando = true;
        }

        if (((Input.GetKey(KeyCode.J) && GameManager.Instance.controles == "zurdo") ||
            (Input.GetKey(KeyCode.A) && GameManager.Instance.controles == "diestro")) && GameManager.Instance.menuAbierto == false)
        {
            movimiento += (Vector2)(-transform.right);//Izquierda
            andando = true;
        }

        if (((Input.GetKey(KeyCode.L) && GameManager.Instance.controles == "zurdo") ||
            (Input.GetKey(KeyCode.D) && GameManager.Instance.controles == "diestro")) && GameManager.Instance.menuAbierto == false)
        {
            movimiento += (Vector2)(transform.right);//Derecha
            andando = true;
        }

        if (((!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.K)
            && !Input.GetKey(KeyCode.L)) && GameManager.Instance.controles == "zurdo") || ((!Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) && GameManager.Instance.controles == "diestro"))
            andando = false;



        movimiento.Normalize();

        movimiento *= velocidad * Time.deltaTime;

        rb.position += movimiento;
    }

    void Animaciones()
    {
        //El personaje se gira
        if (((mirandoDerecha == true && (((Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L)) && GameManager.Instance.controles == "zurdo") ||
            ((Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) && GameManager.Instance.controles == "diestro"))) || (mirandoDerecha == false &&
            (((Input.GetKey(KeyCode.L)) && !Input.GetKey(KeyCode.J) && GameManager.Instance.controles == "zurdo") || (Input.GetKey(KeyCode.D)) &&
            !Input.GetKey(KeyCode.A) && GameManager.Instance.controles == "diestro"))) && GameManager.Instance.menuAbierto == false)
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        //Animaciones
        if (andando == true)//Animación de andar
        {
            animator.SetBool("Andando", true);
        }
        else//Animación de idle
        {
            animator.SetBool("Andando", false);
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
        if(GameManager.Instance.menuAbierto == false)
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