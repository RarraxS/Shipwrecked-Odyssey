using UnityEngine;
using UnityEngine.Tilemaps;

public class ControladorHerramientas : MonoBehaviour
{
    private Jugador personaje;
    private Rigidbody2D rb;

    [SerializeField] private float offsetDistance = 1f;

    // Marcar casilla actual --------------------------------------------

    [SerializeField] private float tamañoAreaInteractuable = 1.2f;
    [SerializeField] private MarkerManager markerManager;
    [SerializeField] private TileMapReadController tileMapReadController;
    [SerializeField] private float distanciaMaxima;

    // Arar --------------------------------------------------------------

    [SerializeField] private TileBase piezaArada;
    [SerializeField] private Tilemap arado;
    [SerializeField] private int energiaArar;

    //--------------------------------------------------------------------


    private Vector3Int posicionTileSeleccionado;
    private bool seleccionado;


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
        TileSeleccionado();
        Mostrar();
        Marker();

        Arar();
        if (Input.GetMouseButtonDown(0))
        {
            UsarHerramientaWorld();
        }
    }

    private void TileSeleccionado()
    {
        //Guarda qué tile se está seleccionando, guarda su posición
        posicionTileSeleccionado = tileMapReadController.GetGridPosition(Input.mousePosition, true);
    }

    private void Mostrar()
    {
        //Mientras el ratón se encuentre a una cierta distancia del jugador permite interacruar con lo seleccionado
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
        return arado.WorldToCell(mouseWorldPos);
    }

    //--------------------------------------------------------------------

    private void Arar()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Coge la posición del tile que se está seleccionando y al tileset que se le pasa por parámetro 
            //se le apllica el tile paasado por referencia
            Vector3Int posicionMouse = GetMouseTilePosition();
            TileBase tileEnPosicion = arado.GetTile(posicionMouse);

            if (tileEnPosicion == null)
            {
                // No hay un tile en la posición actual, puedes hacer algo aquí
                arado.SetTile(posicionMouse, piezaArada); // Ejemplo: establecer un nuevo tile en la posición
                Jugador.Instance.energia -= energiaArar;
            }
        }
    }



    //--------------------------------------------------------------------


    private void UsarHerramientaWorld()
    {
        //Lo que hace es guardar la última posición hacia la que se
        //movió el jugador y permite picar para esa dirección 

        Vector2 posicion = rb.position + personaje.ultimoMotionVector * offsetDistance;

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
