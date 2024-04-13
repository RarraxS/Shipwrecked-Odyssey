using UnityEngine;

public class Toolbar : MonoBehaviour
{
    [SerializeField] private BotonInventario[] botonesToolbar;
    [SerializeField] private BotonInventario[] botonesInventario;


    private int herramientaActual;
    public BotonInventario herramientaSeleccionada;
    [SerializeField] private int numeroCasillasToolbar;


    void Start()
    {
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
        for(int i = 0; i<botonesToolbar.Length; i++)
        {
            botonesToolbar[i].item = botonesInventario[i].item;
            botonesToolbar[i].cantidad = botonesInventario[i].cantidad;
        }
    }

    private void ComprobarEstadoRaton()
    {
        float movimientoRaton = Input.mouseScrollDelta.y;

        if (movimientoRaton != 0)
        {
            botonesToolbar[herramientaActual].seleccionado.SetActive(false);

            if (movimientoRaton > 0)
            {
                herramientaActual++;
            }

            else
            {
                herramientaActual--;
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
