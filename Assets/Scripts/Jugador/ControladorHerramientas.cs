using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Tilemaps;

public class ControladorHerramientas : MonoBehaviour, IObserver
{
    private Jugador personaje;
    private Rigidbody2D rb;

    // General -------------------------------------------------------------------

    [SerializeField] private Tilemap worldSinColisiones;

    // Marcar casilla actual -----------------------------------------------------

    [SerializeField] private MarkerManager markerManager;
    [SerializeField] private float distanciaMaximaMarcador;

    private Vector3Int posicionTileSeleccionado;
    private bool seleccionado;

    // Objetos Recolectables -----------------------------------------------------

    [SerializeField] private float distanciaEntreTiles;

    // Arar ----------------------------------------------------------------------

    [SerializeField] private string nombreHerramientaParaArar;
    [SerializeField] private Tilemap arado;
    [SerializeField] private TileBase piezaArada;
    [SerializeField] private Color colorTierra, colorArena;
    [SerializeField] private List<TileBase> tilesArablesTierra, tilesArablesArena;

    // Regar ---------------------------------------------------------------------

    [SerializeField] private string nombreHerramientaParaRegar;
    [SerializeField] private int energiaRegar;
    [SerializeField] private TileBase piezaRegar;
    [SerializeField] private Tilemap regar, mar;

    //----------------------------------------------------------------------------

    private void Start()
    {
        personaje = GetComponent<Jugador>();
        rb = GetComponent<Rigidbody2D>();

        ObserverManager.Instance.AddObserver(this);
    }

    private void Update()
    {
        Indicador();

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && GameManager.Instance.permitirUsarHerramineta == true)
        {
            //Coge la posición del tile que se está seleccionando y al tileset que se le pasa por parámetro 
            //se le apllica el tile paasado por referencia
            Vector3Int posicionMouse = GetMouseTilePosition();

            if (Input.GetMouseButtonDown(0))
            {
                AccederRecolectable(posicionMouse);
            }

            else if (Input.GetMouseButtonDown(1))
            {
                RecargarRegadera(posicionMouse);
            }
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


    #region Arar y regar

    // Arar ----------------------------------------------------------------

    private void Arar(Vector3Int posicion)
    {
        TileBase tileSinColision = worldSinColisiones.GetTile(posicion);

        TileBase tileEnPosicion = arado.GetTile(posicion);


        if (tileEnPosicion == null)
        {
            List<TileBase> tilesTotales = new List<TileBase>();
            tilesTotales.AddRange(tilesArablesTierra);
            tilesTotales.AddRange(tilesArablesArena);


            for (int i = 0; i < tilesTotales.Count; i++)
            {
                if (tilesTotales[i] == tileSinColision)
                {
                    arado.SetTile(posicion, piezaArada);

                    if (i < tilesArablesTierra.Count)
                    {
                        arado.SetColor(posicion, colorTierra);
                    }

                    else
                    {
                        arado.SetColor(posicion, colorArena);
                    }

                    Jugador.Instance.energia -= Toolbar.Instance.herramientaSeleccionada.item.energiaPorGolpe;

                    return;
                }
            }
        }
    }


    // Regar ------------------------------------------------------------------

    private void Regar(Vector3Int posicion, ObjetosRecolectables objetoRecolectable)
    {
        TileBase tileArado = arado.GetTile(posicion);

        TileBase tileRegar = regar.GetTile(posicion);


        if (tileArado != null && Toolbar.Instance.herramientaSeleccionada.cantidadBarraActual > 0 )
        {
            regar.SetTile(posicion, piezaRegar);
            objetoRecolectable.regado = true;
            ObserverManager.Instance.NotifyObserverNum("Restar en la barra de utilidad", Toolbar.Instance.herramientaActual);
            Jugador.Instance.energia -= Toolbar.Instance.herramientaSeleccionada.item.energiaPorGolpe;
        }
    }

    // Recargae la regadera ---------------------------------------------------

    private void RecargarRegadera(Vector3Int posicion)
    {
        if (Toolbar.Instance.herramientaSeleccionada.item.herramienta == nombreHerramientaParaRegar)
        {
            TileBase tileAgua = mar.GetTile(posicion);

            if (tileAgua != null)
            {
                ObserverManager.Instance.NotifyObserverNum("Recargar la barra de utilidad", Toolbar.Instance.herramientaActual);
            }
        }
    }

    #endregion

    //----------------------------------------------------------------------

    private void AccederRecolectable(Vector3Int posicion)
    {
        Vector3Int posicionObjeto = new Vector3Int(posicion.x + 1, posicion.y + 1, 0);


        Vector2 posicionMouse = new Vector2(posicionObjeto.x, posicionObjeto.y);


        Collider2D[] colliders = Physics2D.OverlapCircleAll(posicionMouse, distanciaEntreTiles);


        foreach (Collider2D hitbox in colliders)
        {
            if(hitbox != null && hitbox.gameObject.name != gameObject.name)
            {
                ObjetosRecolectables objetoRecolectable = hitbox.gameObject.GetComponent<ObjetosRecolectables>();

                if (objetoRecolectable != null)
                {
                    if (objetoRecolectable.componenteSpriteRenderer.enabled && posicionObjeto == objetoRecolectable.transform.position 
                        && objetoRecolectable.permitirGolpear == true )
                    {
                        objetoRecolectable.ClasificarGolpe();

                        if (Toolbar.Instance.herramientaSeleccionada.item != null && Toolbar.Instance.herramientaSeleccionada.item.herramienta == nombreHerramientaParaRegar &&
                            objetoRecolectable.regado == false)
                        {
                            Regar(posicion, objetoRecolectable);
                        }
                    }

                    else if (Toolbar.Instance.herramientaSeleccionada.item != null)
                    {
                        if (objetoRecolectable.componenteSpriteRenderer.enabled == false &&
                            Toolbar.Instance.herramientaSeleccionada.item.herramienta == nombreHerramientaParaArar)
                        {
                            Arar(posicion);
                        }

                        else if (Toolbar.Instance.herramientaSeleccionada.item.herramienta == nombreHerramientaParaRegar && 
                            objetoRecolectable.regado == false)
                        {
                            Regar(posicion, objetoRecolectable);
                        }

                        else if (objetoRecolectable.componenteSpriteRenderer.enabled == false &&
                            Toolbar.Instance.herramientaSeleccionada.item.semilla == true)
                        {
                            TileBase tileComprobarArado = arado.GetTile(posicion);

                            if (tileComprobarArado != null)
                            {
                                objetoRecolectable.CambiarAndAparecerObjeto(Toolbar.Instance.herramientaSeleccionada.item.worldItem);

                                ObserverManager.Instance.NotifyObserver("Restar en el inventario");
                            }
                        }
                    }
                }
            }
        }
    }

    //-------------------------------------------------------------------------

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "dia completado")
        {
            regar.ClearAllTiles();
        }
    }
}