using UnityEngine;

public class ObjetosRecolectables : MonoBehaviour
{
    [SerializeField] private int puntosDeVida;
    public string herramientaNecesaria;
    [SerializeField] private int nivelMinimoDeHerramienta;
    [SerializeField] private float distanciaMaximaAparicion;
    [SerializeField] private Dropeables[] dropeables;

    [SerializeField] private Color colorTransparencia;

    private Transform tr;// Esta variable es privada para poder generar luego los items al rededor del lugar en el que se encuentra este objeto

    [SerializeField] private GameObject objetoSinColision;

    // Son las variables que van a permitir pasar la informacion de los objetos de un tipo a otro

    public SpriteRenderer spriteRenderer;
    public ObjetosRecolectables objetoRecolectable;
    public PolygonCollider2D hitboxColision;
    public PolygonCollider2D hitboxSinColision;

    [SerializeField] private ObjetosRecolectables[] objeto;// Es el array que contiene todos los distintos objetos recolectables del juego


    private void Start()
    {
        tr = GetComponent<Transform>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        objetoRecolectable = GetComponent<ObjetosRecolectables>();
        hitboxColision = GetComponent<PolygonCollider2D>();
        hitboxSinColision = objetoSinColision.GetComponent<PolygonCollider2D>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AparecerObjeto(objeto[1].name);
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

    private void AparecerObjeto(string name)
    {
        //Comprobamos que objeto es el que estamos buscando y cambiamos las variables de los componentes de este objeto por las del objeto objetivo
        for (int i = 0; i < objeto.Length; i++)
        {
            if (objeto[i].gameObject.name == name)
            {
                // Esto es provisional hasta que se encuentre una forma distinta de hacerlo --------------------
                
                spriteRenderer.sprite = objeto[i].spriteRenderer.sprite;



                puntosDeVida = objeto[i].objetoRecolectable.puntosDeVida;
                herramientaNecesaria = objeto[i].objetoRecolectable.herramientaNecesaria;
                nivelMinimoDeHerramienta = objeto[i].objetoRecolectable.nivelMinimoDeHerramienta;
                distanciaMaximaAparicion = objeto[i].objetoRecolectable.distanciaMaximaAparicion;
                
                for (int j = 0; j < dropeables.Length; j++)
                {
                    dropeables[j] = null; 
                }

                for (int k = 0; k < dropeables.Length; k++)
                {
                    dropeables[k] = objeto[i].dropeables[k];
                }



                hitboxColision.points = objeto[i].hitboxColision.points;
                hitboxSinColision.points = objeto[i].hitboxSinColision.points;

                //----------------------------------------------------------------------------------------------

                gameObject.SetActive(true);

                break;
            }
        }
    }

    public void TransparentarObjeto()
    {
        spriteRenderer.color = colorTransparencia;
    }

    public void RestablecerColor()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
