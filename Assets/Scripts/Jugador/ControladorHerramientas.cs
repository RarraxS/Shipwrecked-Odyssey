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
    [SerializeField] private float distanciaMaxima;

    private Vector3Int posicionTileSeleccionado;
    private bool seleccionado;

    // Tiles Recolectables ------------------------------------------------

    [SerializeField] private Tilemap tilemapRecolectables;
    private VariablesTilemapRecolectables[,] datosRecolectables;
    [SerializeField] public Recolectables[] tilesRecolectables;

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
        seleccionado = Vector2.Distance(characterPosition, cameraPosition) < distanciaMaxima;
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
        ////Obtiene los límites del tilemap en el que pueden haber tiles recolectables y crea una matriz de esas dimensiones
        //BoundsInt limites = worldRecolectables.cellBounds;
        //datosRecolectables = new VariablesTilemapRecolectables[limites.xMax, limites.yMax];

        ////Recorre todas las posiciones de la matriz viendo si hay un objeto es esa casilla
        ////o no, en caso de haberlo le establece los puntos de vida correspondientes a ese
        ////objeto, en caso de no haber nada ahí establece los puntos de vida de ese tile a 0
        //for (int x = limites.xMin; x < limites.xMax; x++)
        //{

        //    for (int y = limites.yMin; y < limites.yMax; y++)
        //    {
        //        Vector3Int tilePosition = new Vector3Int(x, y);

        //        // Obtén el tile en la posición actual
        //        TileBase tile = tilemapRecolectables.GetTile(tilePosition);

        //        if (tile != null)
        //        {
        //            for (int i = 0; i < tilesRecolectables.Length; i++)
        //            {
        //                if (tile == tilesRecolectables[i])
        //                {
        //                    datosRecolectables[x, y].matrizPuntosVidaTile = tilesRecolectables[i].puntosDeVida;
        //                    //Debug.Log("Hay un " + tilesRecolectables[i].);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}








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
                string herramienta = null;

                //Aquí hacer un bucle for para comprobar cual de todos los objetos es este tile y así ver cuánta vida tiene ese determinado tile
                for (int i = 0; i < tilesRecolectables.Length; i++)
                {
                    if(tileReal == tilesRecolectables[i].tile)
                    {
                        ptsVida = tilesRecolectables[i].puntosDeVida;
                        herramienta = tilesRecolectables[i].herramienta;
                        
                        break;
                    }
                }
                Debug.Log(tileReal + " vida: " + ptsVida + ", herramienta: " + herramienta);
                tile.SetearTile(ptsVida, herramienta, posicion, tileReal != null);
                tiles.Add(tile);
            }
        }
    }

    //----------------------------------------------------------------------

    // Hacha ---------------------------------------------------------------

    private void Hacha(Vector3Int posicion)
    {
        ////Coge la posición del tile que se está seleccionando y al tileset que se le pasa por parámetro 
        ////se le apllica el tile paasado por referencia
        //Vector3Int posicionMouse = GetMouseTilePosition();
        //TileBase tileEnPosicion = worldRecolectables.GetTile(posicionMouse);

        //Debug.Log(tileEnPosicion);

        //if (tileEnPosicion != null)
        //{
        //    datosRecolectables[posicionMouse.x, posicionMouse.y].matrizPuntosVidaTile -= 1;
        //    Debug.Log(datosRecolectables[posicionMouse.x, posicionMouse.y].matrizPuntosVidaTile);
        //    Jugador.Instance.energia -= energiaArar;
        //}



        // Obtiene la posición del mouse en el mundo
        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //// Convierte la posición del mouse en coordenadas de celda del Tilemap original
        //Vector3Int posicionCeldaOriginal = tilemapRecolectables.WorldToCell(mousePosition);

        //Vector3Int posicionDestinoCelda = worldRecolectables.WorldToCell(tilemapRecolectables.GetCellCenterWorld(posicionCeldaOriginal));

        //Vector3 destinationTileCenter = worldRecolectables.GetCellCenterWorld(posicionDestinoCelda);


        //// Obtiene la fila y la columna del Tilemap de destino
        //int row = posicionDestinoCelda.y;
        //int column = posicionDestinoCelda.x;

        //// Imprime la fila y la columna del Tilemap de destino en la consola
        //Debug.Log("Fila: " + row + ", Columna: " + column);


        ////Pilla la posición del tile
        //Vector3Int posicionMouse = GetMouseTilePosition();
        //TileBase tileEnPosicion = tilemapRecolectables.GetTile(posicionMouse);

        //if (tileEnPosicion != null)
        //{

        //}

        //if (tileEnPosicion == null)
        //{
        //    tilemapRecolectables.SetTile(posicionMouse, piezaArada);
        //    Jugador.Instance.energia -= energiaArar;
        //}








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
                        tile.hayTile = false;
                        
                        Tile newTile = ScriptableObject.CreateInstance<Tile>();     //Se crea un nuevo tile
                        newTile.sprite = null;                                      //Se le asigna el sprite de reemplazo
                        tilemapRecolectables.SetTile(posicion, newTile);            //Se reemplaza el tile

                        //Programar el dropear items al romper (intentar transpasar esa parte del script "Objetosdestruibles" al
                        //script de "Recolectables" y aprovechar la posición del ratón que se le pasa por parámetro para sacar el
                        //centro del tile y ya de ahí sacar un círuclo en el que puedan aparecer los objetos)
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
    public bool hayTile;

    public void SetearTile(int vidaTile, string herramienra, Vector3Int posicionTile, bool existeTile)
    {
        puntosVida = vidaTile;
        posicion = posicionTile;
        hayTile = existeTile;
    }

    public void ResetearSprite()
    {
        //sprite = null;
    }
}

//public class TileRecolectable : Tile
//{
//    public int puntosVida;

//    public void SetupTile(Sprite _sprite)
//    {
//        sprite = _sprite;
//        color = Random.ColorHSV();
//        colliderType = Tile.ColliderType.Sprite;
//    }
//}