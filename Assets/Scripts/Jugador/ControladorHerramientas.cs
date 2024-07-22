using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ControladorHerramientas : MonoBehaviour
{
    private Jugador personaje;
    private Rigidbody2D rb;

    // General ------------------------------------------------------------

    [SerializeField] private Tilemap worldSinColisiones;

    // Marcar casilla actual ----------------------------------------------

    [SerializeField] private MarkerManager markerManager;
    [SerializeField] private float distanciaMaximaMarcador;

    private Vector3Int posicionTileSeleccionado;
    private bool seleccionado;

    // Objetos Recolectables ------------------------------------------------

    [SerializeField] private float distanciaEntreTiles;

    // Arar ---------------------------------------------------------------

    [SerializeField] private int energiaArar;
    [SerializeField] private Tilemap arado;
    [SerializeField] private TileBase piezaAradaTierra;
    [SerializeField] private TileBase piezaAradaArena;
    [SerializeField] private List<TileBase> tilesArablesTierra;
    [SerializeField] private List<TileBase> tilesArablesArena;

    //----------------------------------------------------------------------

    

    private void Awake()
    {
        personaje = GetComponent<Jugador>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Indicador();

        
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.permitirUsarHerramineta == true)
        {
            //Coge la posición del tile que se está seleccionando y al tileset que se le pasa por parámetro 
            //se le apllica el tile paasado por referencia
            Vector3Int posicionMouse = GetMouseTilePosition();


            AccederRecolectable(posicionMouse);

            Arar(posicionMouse);
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
        Vector3Int gridPosition = worldSinColisiones.WorldToCell(worldPosition);

        posicionTileSeleccionado = gridPosition;
    }

    private void Mostrar()
    {
        //Mientras el ratón se encuentre a una cierta distancia del jugador permite interacruar con lo seleccionado

        //Guarda la posición tanto del jugador como del mouse en ese momento
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Si el ratón está más lejos de la distancia máxima permitida, el marcador tile del ratón no se mostrará
        seleccionado = Vector2.Distance(characterPosition, cameraPosition) < distanciaMaximaMarcador;
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
        return worldSinColisiones.WorldToCell(mouseWorldPos);
    }

    //------------------------------------------------------------------------------------------------------------
    #endregion
    

    // Arar ----------------------------------------------------------------

    private void Arar(Vector3Int posicion)
    {
        TileBase tileSinColision = worldSinColisiones.GetTile(posicion);

        TileBase tileEnPosicion = arado.GetTile(posicion);


        if (tileEnPosicion == null)
        {
            for (int i = 0; i < tilesArablesTierra.Count; i++)
            {
                if (tilesArablesTierra[i] == tileSinColision)
                {
                    arado.SetTile(posicion, piezaAradaTierra);
                    Jugador.Instance.energia -= energiaArar;
                    
                    return;
                }
            }

            for (int i = 0; i < tilesArablesArena.Count; i++)
            {
                if (tilesArablesArena[i] == tileSinColision)
                {
                    arado.SetTile(posicion, piezaAradaArena);
                    Jugador.Instance.energia -= energiaArar;

                    return;
                }
            }
        }
    }

    //----------------------------------------------------------------------

    private void AccederRecolectable(Vector3Int posicion)
    {
        Vector3Int posicionObjeto = new Vector3Int(posicion.x + 1, posicion.y + 1, 0);


        Vector2 posicionMouse = new Vector2(posicionObjeto.x, posicionObjeto.y);


        Collider2D[] colliders = Physics2D.OverlapCircleAll(posicionMouse, distanciaEntreTiles);


        foreach (Collider2D hitbox in colliders)
        {
            if(hitbox != null && hitbox.gameObject.name != this.gameObject.name)
            {
                ObjetosRecolectables objetoRecolectable = hitbox.gameObject.GetComponent<ObjetosRecolectables>();

                if (objetoRecolectable != null)
                {
                    if (objetoRecolectable.componenteSpriteRenderer.enabled && posicionObjeto == objetoRecolectable.transform.position)
                    {
                        objetoRecolectable.ClasificarGolpe();
                    }

                    else if (objetoRecolectable.componenteSpriteRenderer.enabled == false &&
                        Toolbar.Instance.herramientaSeleccionada.item.semilla == true)
                    {
                        TileBase tileComprobarArado = arado.GetTile(posicion);

                        if (tileComprobarArado != null)
                        {
                            objetoRecolectable.CambiarAndAparecerObjeto(Toolbar.Instance.herramientaSeleccionada.item.worldItem);

                            ObserverManager.Instance.NotifyObserver("Cambio en el inventario");
                        }
                    }
                }
            }
        }
    }
    //-------------------------------------------------------------------------
}