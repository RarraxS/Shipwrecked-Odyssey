using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosRecolectables : MonoBehaviour
{
    public string herramientaNecesaria;
    [SerializeField] private int nivelMinimoDeHerramienta;
    [SerializeField] private int puntosDeVida;
    [SerializeField] private float distanciaMaximaAparicion;

    [SerializeField] private GameObject objetoSinColision;
    [SerializeField] private Color colorTransparencia;

    private Transform tr;

    public SpriteRenderer componenteSpriteRenderer;
    public ObjetosRecolectables componenteObjetoRecolectable;
    public PolygonCollider2D componenteHitboxColision;
    public PolygonCollider2D componenteHitboxSinColision;


    public List<Drops> drops;

    [SerializeField] private List<ItemAndProbability> spawneables;



    private void Start()
    {
        tr = GetComponent<Transform>();

        componenteSpriteRenderer = GetComponent<SpriteRenderer>();
        componenteObjetoRecolectable = GetComponent<ObjetosRecolectables>();
        componenteHitboxColision = GetComponent<PolygonCollider2D>();
        componenteHitboxSinColision = objetoSinColision.GetComponent<PolygonCollider2D>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CambiarAndAparecerObjeto(spawneables[1].worldItem.name);
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

            GenerarDropsAndOcultar();
        }
    }

    private void GenerarDropsAndOcultar()
    {
        for (int numDrop = 0; numDrop < drops.Count; numDrop++)
        {
            if (drops[numDrop].dropeable != null)
            {
                for (int numProbabilidades = drops[numDrop].probabilidades.Count - 1; numProbabilidades >= 0; numProbabilidades--)
                {
                    int random = UnityEngine.Random.Range(1, 101);

                    if (random <= drops[numDrop].probabilidades[numProbabilidades].probabilidad)
                    {
                        for (int cantidadDrops = drops[numDrop].probabilidades[numProbabilidades].cantidad; cantidadDrops > 0; cantidadDrops--)
                        {
                            float distanciaAparicion;

                            distanciaAparicion = UnityEngine.Random.Range(0, distanciaMaximaAparicion);

                            Vector3 posicionAleatoria = (UnityEngine.Random.insideUnitSphere * distanciaAparicion) + tr.position;
                            posicionAleatoria.z = 0;

                            Instantiate(drops[numDrop].dropeable, posicionAleatoria, Quaternion.identity);
                        }
                        break;
                    }
                }
            }
        }
        gameObject.SetActive(false);
    }

    private void CambiarAndAparecerObjeto(string name)
    {
        for (int i = 0; i < spawneables.Count; i++)
        {
            if (spawneables[i].worldItem.gameObject.name == name)
            {
                //Necesario buscar una forma optima y escalable de pasar los datos de un objeto recolectable a otro
                //para no tener que andar modificando esta parte del código cada que se implemente una nueva variable


                componenteSpriteRenderer.sprite = spawneables[i].worldItem.componenteSpriteRenderer.sprite;



                puntosDeVida = spawneables[i].worldItem.componenteObjetoRecolectable.puntosDeVida;
                herramientaNecesaria = spawneables[i].worldItem.componenteObjetoRecolectable.herramientaNecesaria;
                nivelMinimoDeHerramienta = spawneables[i].worldItem.componenteObjetoRecolectable.nivelMinimoDeHerramienta;
                distanciaMaximaAparicion = spawneables[i].worldItem.componenteObjetoRecolectable.distanciaMaximaAparicion;

                for (int j = 0; j < drops.Count; j++)
                {
                    if (spawneables[i].worldItem.drops[j] != null)
                    {
                        drops[j] = spawneables[i].worldItem.drops[j];
                    }

                    else
                    {
                        drops[j] = null;
                    }
                }


                componenteHitboxColision.points = spawneables[i].worldItem.componenteHitboxColision.points;
                componenteHitboxSinColision.points = spawneables[i].worldItem.componenteHitboxSinColision.points;

                //----------------------------------------------------------------------------------------------

                gameObject.SetActive(true);

                break;
            }
        }
    }

    public void TransparentarObjeto()
    {
        componenteSpriteRenderer.color = colorTransparencia;
    }

    public void RestablecerColor()
    {
        componenteSpriteRenderer.color = new Color(1, 1, 1, 1);
    }
}


//Esta va a ser la clase que contenga las variables de probabilidad y los gameobjects
//que van a swawnear de forma aleatoria por el mapa a lo largo de los dias

[Serializable]
public class ItemAndProbability
{
    public ObjetosRecolectables worldItem;

    public float probabilidad;
}

//-----------------------------------------------------------------------------------------

//Estas clases se van a encargar de almacenar la informacion de los dropeables

[Serializable]
public class Drops
{
    public GameObject dropeable;

    public List<CuantityProbabilities> probabilidades;
}

[Serializable]
public class CuantityProbabilities
{
    public int cantidad;

    public float probabilidad;
}