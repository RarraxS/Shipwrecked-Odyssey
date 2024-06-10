using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ControladorHerramientas : MonoBehaviour
{
    private Jugador personaje;
    private Rigidbody2D rb;

    // General ------------------------------------------------------------

    [SerializeField] private Tilemap worldRecolectables;//Tilemap que contiene todaslas posiciones en las que pueden haber objetos recolectables

    // Marcar casilla actual ----------------------------------------------

    [SerializeField] private float tamañoAreaInteractuable = 1.2f;
    [SerializeField] private MarkerManager markerManager;
    [SerializeField] private float distanciaMaximaMarcado;

    private Vector3Int posicionTileSeleccionado;
    private bool seleccionado;

    // Tiles Recolectables ------------------------------------------------

    [SerializeField] public Recolectables[] tilesRecolectables;
    [SerializeField] private Tilemap tilemapRecolectables;
    [SerializeField] private float distanciaMaximaAparicion;

    private List<SingleTile> tiles;
    private Vector3Int dimension;


    // Arar ---------------------------------------------------------------

    [SerializeField] private TileBase piezaArada;
    [SerializeField] private Tilemap arado;
    [SerializeField] private int energiaArar;

    //----------------------------------------------------------------------

    private static ControladorHerramientas instance;
    public static ControladorHerramientas Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);




        personaje = GetComponent<Jugador>();
        rb = GetComponent<Rigidbody2D>();

        TilesRecolectablesOcupados();
    }

    private void Update()
    {
        Indicador();

        
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.permitirUsarHerramineta == true)
        {
            //Coge la posición del tile que se está seleccionando y al tileset que se le pasa por parámetro 
            //se le apllica el tile paasado por referencia
            Vector3Int posicionMouse = GetMouseTilePosition();
            



            Arar(posicionMouse);

            Hacha(posicionMouse);

            UsarHerramientaWorld();
        }
    }

    #region Marcador
    //------------------------------------------------------------------------------------------------------------

    private void Indicador()
    {
        GetGridPosition(Input.mousePosition, true);
        Mostrar();
        Marker();
    }


    public void GetGridPosition(Vector2 position, bool mousePosition)
    {
        //Guarda la posición del mouse y la convierte a tile para saber sobre cuál está

        Vector3 worldPosition;

        //Guarda la posicion del mouse en el mundo
        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        //Convierte esa posicion del mundo en una posicion de tile
        Vector3Int gridPosition = worldRecolectables.WorldToCell(worldPosition);

        posicionTileSeleccionado = gridPosition;
    }

    private void Mostrar()
    {
        //Mientras el ratón se encuentre a una cierta distancia del jugador permite interacruar con lo seleccionado

        //Guarda la posición tanto del jugador como del mouse en ese momento
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Si el ratón está más lejos de la distancia máxima permitida, el marcador tile del ratón no se mostrará
        seleccionado = Vector2.Distance(characterPosition, cameraPosition) < distanciaMaximaMarcado;
        markerManager.Show(seleccionado);
    }

    private void Marker()
    {
        //Marca el tile que se ha seleccionado en este frame
        markerManager.markedCellPosition = posicionTileSeleccionado;
    }



    private Vector3Int GetMouseTilePosition()
    {
        //Coje la posición del tile marcado por el ratón
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return worldRecolectables.WorldToCell(mouseWorldPos);
    }

    //------------------------------------------------------------------------------------------------------------
    #endregion
    
    // Guardar variables de tiles recolectables ----------------------------
    private void TilesRecolectablesOcupados()
    {
        tiles = new List<SingleTile>();
        dimension = tilemapRecolectables.size;
        BoundsInt limites = worldRecolectables.cellBounds;

        for (int fila = limites.min.y; fila < limites.max.y; fila++)
        {
            for (int columna = limites.min.x; columna < limites.max.x; columna++)
            {
                Vector3Int posicion = new Vector3Int(columna, fila, 0);
                SingleTile tile = new();
                TileBase tileReal = tilemapRecolectables.GetTile(posicion);
                int ptsVida = 0;
                GameObject recolectables = null;
                int numDrops = 0;
                string herramienta = null;
                int nivelMin = 0;

                //Aquí hacer un bucle for para comprobar cual de todos los objetos es este tile y así ver cuánta vida tiene ese determinado tile
                for (int i = 0; i < tilesRecolectables.Length; i++)
                {
                    if(tileReal == tilesRecolectables[i].tile)
                    {
                        ptsVida = tilesRecolectables[i].puntosDeVida;
                        herramienta = tilesRecolectables[i].herramienta;
                        nivelMin = tilesRecolectables[i].nivelMinimoHerramienta;
                        recolectables = tilesRecolectables[i].objetoRecolectable;
                        numDrops = tilesRecolectables[i].numeroRecolectables;

                        break;
                    }
                }
                //Debug.Log(tileReal + " vida: " + ptsVida + ", herramienta: " + herramienta + ", recolectable: " + recolectables);
                tile.SetearTile(ptsVida, posicion, recolectables, numDrops, herramienta, nivelMin, tileReal != null);
                tiles.Add(tile);
            }
        }
    }

    //----------------------------------------------------------------------

    // Hacha ---------------------------------------------------------------

    private void Hacha(Vector3Int posicion)
    {
        TileBase tileEnPosicion = tilemapRecolectables.GetTile(posicion);

        if (tileEnPosicion != null)
        {
            tiles.ForEach(tile =>
            {
                if (tile.posicion == posicion)
                {
                    tile.puntosVida -= 1;
                    Jugador.Instance.energia -= energiaArar;
                    Debug.Log(tile.puntosVida);


                    if (tile.puntosVida <= 0)
                    {                        
                        Tile newTile = ScriptableObject.CreateInstance<Tile>();     //Se crea un nuevo tile
                        newTile.sprite = null;                                      //Se le asigna el sprite de reemplazo
                        tilemapRecolectables.SetTile(posicion, newTile);            //Se reemplaza el tile

                        if (tile.hayTile == true)
                        {
                            Vector3 centroTile = worldRecolectables.GetCellCenterWorld(posicion);

                            for (int i = 0; i < tile.numDropeables; i++)
                            {
                                float distanciaAparicion;

                                distanciaAparicion = Random.Range(0, distanciaMaximaAparicion);

                                Vector3 posicionAleatoria = (Random.insideUnitSphere * distanciaAparicion) + centroTile;
                                posicionAleatoria.z = 0;

                                Instantiate(tile.objetoRecolectable, posicionAleatoria, Quaternion.identity);
                            }
                        }

                        tile.hayTile = false;
                    }
                }
            });
        }
    }

    //----------------------------------------------------------------------

    // Arar ----------------------------------------------------------------

    private void Arar(Vector3Int posicion)
    {
        TileBase tileEnPosicion = arado.GetTile(posicion);

        if (tileEnPosicion == null)
        {
            // No hay un tile en la posición actual, puedes hacer algo aquí
            arado.SetTile(posicion, piezaArada); // Ejemplo: establecer un nuevo tile en la posición
            Jugador.Instance.energia -= energiaArar;
        }
    }

    //----------------------------------------------------------------------

    private void UsarHerramientaWorld()
    {
        //Lo que hace es guardar la última posición hacia la que se
        //movió el jugador y permite picar para esa dirección 

        Vector2 posicion = rb.position + personaje.ultimoMotionVector;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(posicion, tamañoAreaInteractuable);

        foreach (Collider2D c in colliders)
        {
            HitHerramientas hit = c.GetComponent<HitHerramientas>();
            if (hit != null)
            {
                hit.Hit();
                break;
            }
        }
    }
}



public class SingleTile
{
    public int puntosVida;
    public Vector3Int posicion;
    public GameObject objetoRecolectable;
    public int numDropeables;
    public bool hayTile;
    public string herramienta;
    public int nivelMinimo;

    public void SetearTile(int vidaTile, Vector3Int posicionTile, GameObject recolectable, int numeroDrops, string herramientaTile, int nivelMin, bool existeTile)
    {
        puntosVida = vidaTile;
        posicion = posicionTile;
        objetoRecolectable = recolectable;
        numDropeables = numeroDrops;
        hayTile = existeTile;
        herramienta= herramientaTile;
        nivelMinimo= nivelMin;
    }
}