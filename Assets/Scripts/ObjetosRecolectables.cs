using UnityEngine;

public class ObjetosRecolectables : MonoBehaviour
{
    [SerializeField] private int puntosDeVida;
    public string herramientaNecesaria;
    [SerializeField] private int nivelMinimoDeHerramienta;
    [SerializeField] private float distanciaMaximaAparicion;
    [SerializeField] private Dropeables[] dropeables;

    private Transform tr;// Esta variable es privada para poder generar luego los items al rededor del lugar en el que se encuentra este objeto

    // Son las variables que van a permitir pasar la informacion de los objetos de un tipo a otro

    [SerializeField] private GameObject[] objeto;// Es el array que contiene todos los distintos objetos recolectables del juego

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ObjetosRecolectables objetoRecolectable;
    [SerializeField] private Collider2D hitboxColision;
    [SerializeField] private Collider2D hitboxSinColision;


    private void Start()
    {
        tr = GetComponent<Transform>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        objetoRecolectable = GetComponent<ObjetosRecolectables>();
        hitboxColision = GetComponent<Collider2D>();
        hitboxSinColision = GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        
    }


    public void ClasificarGolpe() 
    {
        //Si se golpea con una herramienta se llama a esta funcion que comprobara si esa herramienta
        //es la adecuada para ese tipo de objeto y en caso de ser asi le resta una cantidad de puntos
        //de vida indicados establecidos en la propia herramienta
        if (Toolbar.Instance.herramientaSeleccionada.item != null)
        {
            if (Toolbar.Instance.herramientaSeleccionada.item.herramienta == herramientaNecesaria || 
                herramientaNecesaria == "" && Toolbar.Instance.herramientaSeleccionada.item.herramienta != "")
            {
                Golpear(Toolbar.Instance.herramientaSeleccionada.item.damageHerramienta);
            }
        }

        //Si se golpea con la mano u otro ojeto se llama a esta funcion que comprobara si esa
        //herramienta es la adecuada para ese tipo de objeto y en caso de ser asi le resta una
        //cantidad de puntos de vida indicados establecidos en la propia herramienta
        else if (herramientaNecesaria == "")
        {
            Golpear(1);
        }

        else
            return;
    }

    public void Golpear(int damage) 
    {
        puntosDeVida -= damage;
        
        if (puntosDeVida <= 0)
        {
            puntosDeVida= 0;

            OcultarObjeto();

            gameObject.SetActive(false);
        }
    }

    private void OcultarObjeto()
    {
        for (int i = 0; i < dropeables.Length; i++)
        {
            int random = Random.Range(1, 101);
            Debug.Log(random);
            for (int j = (dropeables[i].probabilidad.Length - 1); j >= 0 ; j--)
            {
                if (random <= dropeables[i].probabilidad[j])
                {
                    for (int k = dropeables[i].cantidad[j]; k > 0; k--)
                    {
                        float distanciaAparicion;

                        distanciaAparicion = Random.Range(0, distanciaMaximaAparicion);

                        Vector3 posicionAleatoria = (Random.insideUnitSphere * distanciaAparicion) + tr.position;
                        posicionAleatoria.z = 0;

                        Instantiate(dropeables[0].recolectable, posicionAleatoria, Quaternion.identity);
                    }
                    break;
                }
            }
        }
    }
}
