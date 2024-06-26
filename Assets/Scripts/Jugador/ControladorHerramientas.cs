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

    [SerializeField] private MarkerManager markerManager;
    [SerializeField] private float distanciaMaximaMarcador;

    private Vector3Int posicionTileSeleccionado;
    private bool seleccionado;

    // Objetos Recolectables ------------------------------------------------


    [SerializeField] private float distanciaEntreTiles;


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
        return worldRecolectables.WorldToCell(mouseWorldPos);
    }

    //------------------------------------------------------------------------------------------------------------
    #endregion
    

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

    private void AccederRecolectable(Vector3Int posicion)
    {
        //Actualizamos el Vector3 para acomodarlo al centro de cada tile y así detectar mejor los objetos
        posicion.x = posicion.x + 1;
        posicion.y = posicion.y + 1;
        posicion.z = 0;


        //Convertimos ese Vector3 a un Vector2 para poder usarlo para detectar las colisiones
        Vector2 posicionMouse = new Vector2(posicion.x, posicion.y);


        //Detectamos en un radio de una casilla (las casillas miden 1, asique el radio es 0.5, pero para evitar
        //solapamientos entre casillas lo ponemos un poco por debajo)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(posicionMouse, distanciaEntreTiles);


        //Para cada objeto con un collider que se encuentre en ese radio y que no sea el propio jugador se
        //obtiene el GameObject correspondiente a dicho objeto´compribamos si tiene un componente/script 
        //"ObjetosRecolectables", y en caso de que lo tenga hacemos cambios en las variables de ese objeto
        foreach (Collider2D hitbox in colliders)
        {
            if(hitbox != null && hitbox.gameObject.name != this.gameObject.name)
            {
                //Creamos una variable de tipo ObjetosRecolectables que acceda al objeto que tenemos y comprueba si
                //dicho objeto tiene el componente/script con el mismo nombre, en caso positivo continuamos con el
                //proceso, en caso negativo acaba ahí el proceso
                ObjetosRecolectables objetoRecolectable = hitbox.gameObject.GetComponent<ObjetosRecolectables>(); 


                //Si el objetoRecolectable no es nulo y la posición del mouse
                //y la del objeto coinciden entonces modificamos las variables
                if (objetoRecolectable != null && posicion == objetoRecolectable.transform.position)
                {
                    //Aqui llamamos a la funcion Golpear del objetoRecolectable en el que se ha clickado
                    objetoRecolectable.ClasificarGolpe();
                }
            }
        }
    }
    //-------------------------------------------------------------------------
}