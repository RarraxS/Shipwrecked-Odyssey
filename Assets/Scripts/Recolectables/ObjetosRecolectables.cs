using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjetosRecolectables : MonoBehaviour, IObserver
{
    [SerializeField] private bool iniciarSinObjeto;

    public string herramientaNecesaria;
    [SerializeField] private int nivelMinimoDeHerramienta;
    public bool permitirGolpear;
    [SerializeField] private int energiaGolpear;
    [SerializeField] private int puntosDeVida;

    public List<Drops> drops;
    [SerializeField] private float distanciaMaximaAparicion;


    [SerializeField] private bool semilla;
    [SerializeField] private string estacionDeCultivo;
    public int numDiasPasados, numDiasParaCrecer;
    public bool arado, regado;
    private ParentVariablesColectableItems varParent;


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

        varParent = GetComponentInParent<ParentVariablesColectableItems>();


        if (iniciarSinObjeto == true)
        {
            DesactivarObjeto();
        }

        Observar();
    }


    #region Golpear objetos
    public void ClasificarGolpe()
    {
        if (permitirGolpear == true && componenteSpriteRenderer.enabled == true)
        {
            if (Toolbar.Instance.herramientaSeleccionada.item != null)
            {
                if (Toolbar.Instance.herramientaSeleccionada.item.tool == herramientaNecesaria && 
                    Toolbar.Instance.herramientaSeleccionada.item.tool != "")
                {
                    Golpear(Toolbar.Instance.herramientaSeleccionada.item.toolDamage);

                    Jugador.Instance.energia -= Toolbar.Instance.herramientaSeleccionada.item.energyPerHit;
                }

                else if (herramientaNecesaria == "")
                {
                    Golpear(1);
                }
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

            GenerarDrops();
        }
    }

    #endregion

    #region Aparecer y desaparecer objetos
    private void GenerarDrops()
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

        DesactivarObjeto();
    }

    private void DesactivarObjeto()
    {
        componenteSpriteRenderer.enabled = false;
        componenteHitboxColision.isTrigger = true;
        componenteAnimator.enabled = false;
    }

    public void CambiarAndAparecerObjeto(ObjetosRecolectables objeto)
    {
        componenteSpriteRenderer.sprite = objeto.componenteSpriteRenderer.sprite;

        // Variables sobre la herramienta para recolectar y los drops -----------------------------
        herramientaNecesaria = objeto.herramientaNecesaria;
        nivelMinimoDeHerramienta = objeto.nivelMinimoDeHerramienta;
        permitirGolpear = objeto.permitirGolpear;
        energiaGolpear = objeto.energiaGolpear;

        puntosDeVida = objeto.puntosDeVida;

        drops = objeto.drops;
        //-----------------------------------------------------------------------------------------

        // Variables para las semillas y el paso del tiempo ---------------------------------------
        semilla = objeto.semilla;
        estacionDeCultivo = objeto.estacionDeCultivo;
        numDiasParaCrecer = objeto.numDiasParaCrecer;
        //-----------------------------------------------------------------------------------------

        // Variables de interaccion fisica --------------------------------------------------------
        rotarEntrarColision = objeto.rotarEntrarColision;
        ignorarTransparencia = objeto.ignorarTransparencia;
        //-----------------------------------------------------------------------------------------

        // Variables para el resto de componentes -------------------------------------------------
        componenteHitboxColision.points = objeto.componenteHitboxColision.points;
        componenteHitboxColision.isTrigger = objeto.componenteHitboxColision.isTrigger;
        componenteHitboxSinColision.points = objeto.componenteHitboxSinColision.points;
        componenteAnimator.runtimeAnimatorController = null;
        componenteAnimator.runtimeAnimatorController = objeto.componenteAnimator.runtimeAnimatorController;
        //-----------------------------------------------------------------------------------------

        componenteSpriteRenderer.enabled = true;
        componenteHitboxColision.enabled = true;
        componenteAnimator.enabled = true;

        if (semilla == true)
        {
            numDiasPasados = componenteAnimator.GetInteger("dias");
        }

        RestablecerColor();
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

    private void ProbabilidadSpawnear()
    {
        float probabilidad = UnityEngine.Random.Range(0f,100f);

        float probabilidadTecho = 0;

        for (int i = 0; i < spawneables.Count; i++)
        {
            probabilidadTecho += spawneables[i].probabilidad;

            if(probabilidad <= probabilidadTecho)
            {
                CambiarAndAparecerObjeto(spawneables[i].worldItem);

                return;
            }
        }
    }

    private void Observar()
    {
        ObserverManager.Instance.AddObserver(this);
        observando = true;
    }

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "Day completed")
        {
            if (componenteSpriteRenderer.enabled == false)
            {
                ProbabilidadSpawnear();


                if (regado == true)
                {
                    regado = false;
                }
            }

            if (regado == true)
            {
                if (semilla == true)
                {
                    componenteAnimator.SetInteger("dias", numDiasPasados += 1);
                }

                regado = false;
            }
        }

        if (eventInfo == "Rainy day")
        {
            if (arado == true)
            {
                Vector3Int position = Vector3Int.FloorToInt(tr.position);
                position += new Vector3Int(-1, -1, 0);


                varParent.tilemapWatered.SetTile(position, varParent.tileWatered);
            }
        }
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