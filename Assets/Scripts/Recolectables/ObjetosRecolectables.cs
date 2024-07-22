using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ObjetosRecolectables : MonoBehaviour, IObserver
{
    public VariablesObjeto objeto;

    public string herramientaNecesaria;
    [SerializeField] private int nivelMinimoDeHerramienta;
    [SerializeField] private bool permitirGolpear;
    [SerializeField] private int energiaGolpear;
    [SerializeField] private int puntosDeVida;

    public List<Drops> drops;
    [SerializeField] private float distanciaMaximaAparicion;


    [SerializeField] private bool semilla;
    [SerializeField] private string estacionDeCultivo;
    private int numDiasPasados;


    public bool rotarEntrarColision;
    [SerializeField] private bool ignorarTransparencia;
    private bool observando;


    [SerializeField] private GameObject objetoSinColision;
    [SerializeField] private Color colorTransparencia;


    private Transform tr;

    public SpriteRenderer componenteSpriteRenderer;
    public PolygonCollider2D componenteHitboxColision;
    public PolygonCollider2D componenteHitboxSinColision;
    public Animator componenteAnimator;


    [SerializeField] private List<ItemAndProbability> spawneables;//Esta lista solo es para los objetos que pueden spawnear con el paso de los dias



    private void Start()
    {
        tr = GetComponent<Transform>();

        componenteSpriteRenderer = GetComponent<SpriteRenderer>();
        componenteHitboxColision = GetComponent<PolygonCollider2D>();
        componenteHitboxSinColision = objetoSinColision.GetComponent<PolygonCollider2D>();
        componenteAnimator = GetComponent<Animator>();

        Observar();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Provisional hasta rehacer el script del GameManager
            componenteAnimator.SetInteger("dias", numDiasPasados += 1);
        }
    }

    #region Golpear objetos
    public void ClasificarGolpe()
    {
        if (permitirGolpear == true && componenteSpriteRenderer.enabled == true)
        {
            if (Toolbar.Instance.herramientaSeleccionada.item != null)
            {
                if ((Toolbar.Instance.herramientaSeleccionada.item.herramienta == herramientaNecesaria ||
                herramientaNecesaria == "") && Toolbar.Instance.herramientaSeleccionada.item.herramienta != "")
                {
                    Golpear(Toolbar.Instance.herramientaSeleccionada.item.damageHerramienta);
                }

                else
                    Golpear(1);
            }

            else if (herramientaNecesaria == "")
            {
                Golpear(1);
            }

            else
                return;
        }
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

    #endregion

    #region Aparecer y desaparecer objetos
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

        componenteSpriteRenderer.enabled = false;
        componenteHitboxColision.isTrigger = true;
        componenteHitboxSinColision.enabled = false;
        componenteAnimator.enabled = false;
    }

    public void CambiarAndAparecerObjeto(ObjetosRecolectables objeto)
    {
        //Necesario buscar una forma optima y escalable de pasar los datos de un objeto recolectable a otro
        //para no tener que andar modificando esta parte del código cada que se implemente una nueva variable


        componenteSpriteRenderer.sprite = objeto.componenteSpriteRenderer.sprite;


        puntosDeVida = objeto.puntosDeVida;
        herramientaNecesaria = objeto.herramientaNecesaria;
        nivelMinimoDeHerramienta = objeto.nivelMinimoDeHerramienta;
        distanciaMaximaAparicion = objeto.distanciaMaximaAparicion;
        permitirGolpear = objeto.permitirGolpear;
        energiaGolpear = objeto.energiaGolpear;
        ignorarTransparencia = objeto.ignorarTransparencia;
        rotarEntrarColision = objeto.rotarEntrarColision;
        numDiasPasados = objeto.numDiasPasados;
        drops = objeto.drops;


        componenteHitboxColision.points = objeto.componenteHitboxColision.points;
        componenteHitboxColision.isTrigger = objeto.componenteHitboxColision.isTrigger;
        componenteHitboxSinColision.points = objeto.componenteHitboxSinColision.points;
        componenteAnimator.runtimeAnimatorController = null;
        componenteAnimator.runtimeAnimatorController = objeto.componenteAnimator.runtimeAnimatorController;

        //----------------------------------------------------------------------------------------------

        componenteSpriteRenderer.enabled = true;
        componenteHitboxColision.enabled = true;
        componenteHitboxSinColision.enabled = true;
        componenteAnimator.enabled = true;

        if (semilla == true)
        {
            numDiasPasados = componenteAnimator.GetInteger("dias");
        }

        Observar();
    }

    #endregion

    #region Alternar color

    public void TransparentarObjeto()
    {
        if (ignorarTransparencia == false)
        {
            componenteSpriteRenderer.color = colorTransparencia;
        }
    }

    public void RestablecerColor()
    {
        if (ignorarTransparencia == false)
        {
            componenteSpriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    #endregion

    private void Observar()
    {
        if (semilla == true || gameObject.activeSelf)
        {
            ObserverManager.Instance.AddObserver(this);
            observando = true;
        }

        else if (observando == true)
        {
            ObserverManager.Instance.RemoveObserver(this);
            observando = false;
        }
    }

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "dia completado")
        {
            //Programar el paso de un dia para las plantas regadas, y la probabilidad de aparicion para cuando no hay ningun objeto
            if (!gameObject.activeSelf)
            {
                //Programar aqui el tirar los dados para ver si spawnea algún objeto al cambiar de dia
            }

            if (semilla == true)
            {
                //Programar el avance de dia para las semillas
                componenteAnimator.SetInteger("dias", numDiasPasados += 1);
            }
        }
    }
}

//-----------------------------------------------------------------------------------

[Serializable]
public class VariablesObjeto
{
    public string herramientaNecesaria;
    [SerializeField] private int nivelMinimoDeHerramienta;
    [SerializeField] private bool permitirGolpear;
    [SerializeField] private int energiaGolpear;
    [SerializeField] private int puntosDeVida;

    public List<Drops> drops;
    [SerializeField] private float distanciaMaximaAparicion;


    [SerializeField] private bool semilla;
    [SerializeField] private string estacionDeCultivo;
    private int numDiasPasados;


    public bool rotarEntrarColision;
    [SerializeField] private bool ignorarTransparencia;
    private bool observando;


    [SerializeField] private GameObject objetoSinColision;
    [SerializeField] private Color colorTransparencia;
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