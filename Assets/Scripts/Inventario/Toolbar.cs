using UnityEngine;

public class Toolbar : MonoBehaviour
{
    //Las variables que van a conectar el inventario con la toolbar --------------
    [SerializeField] private BotonInventario[] botonesToolbar;
    [SerializeField] private BotonInventario[] botonesInventario;
    //----------------------------------------------------------------------------

    //Herramienta actual ---------------------------------------------------------
    private int herramientaActual;
    public BotonInventario herramientaSeleccionada;
    //----------------------------------------------------------------------------

    [SerializeField] private int numeroCasillasToolbar;


    private static Toolbar instance;
    public static Toolbar Instance
    {
        get { return instance; }
    }

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);


        //Al iniciar el juego la herramienta seleccionada es la primera casilla del inventario
        botonesToolbar[0].seleccionado.SetActive(true);
        herramientaSeleccionada = botonesToolbar[0];
    }

    void Update()
    {
        EnlazarToolbarInventario();

        ComprobarEstadoRaton();
    }

    private void EnlazarToolbarInventario()
    {
        //Copia los items de la primera barra del inventario en la toolbar
        for(int i = 0; i<botonesToolbar.Length; i++)
        {
            botonesToolbar[i].item = botonesInventario[i].item;
            botonesToolbar[i].cantidad = botonesInventario[i].cantidad;
        }
    }

    public void OnClick(int herramientaClick)
    {
        //Permite cambiar la herramienta seleccionada mediante click en la toolbar

        botonesToolbar[herramientaActual].seleccionado.SetActive(false);

        herramientaActual = herramientaClick;

        botonesToolbar[herramientaActual].seleccionado.SetActive(true);

        herramientaSeleccionada = botonesToolbar[herramientaActual];
    }

    private void ComprobarEstadoRaton()
    {
        //Permite cambiar la herramienta seleccionada mediante el uso de la rueda del mouse

        float movimientoRaton = Input.mouseScrollDelta.y;

        if (movimientoRaton != 0)
        {
            botonesToolbar[herramientaActual].seleccionado.SetActive(false);

            if (movimientoRaton > 0)
            {
                herramientaActual--;
            }

            else
            {
                herramientaActual++;
            }

            if (herramientaActual >= numeroCasillasToolbar)
            {
                herramientaActual = 0;
            }

            else if (herramientaActual < 0)
            {
                herramientaActual = numeroCasillasToolbar - 1;
            }

            botonesToolbar[herramientaActual].seleccionado.SetActive(true);

            herramientaSeleccionada = botonesToolbar[herramientaActual];
        }
    }
}
