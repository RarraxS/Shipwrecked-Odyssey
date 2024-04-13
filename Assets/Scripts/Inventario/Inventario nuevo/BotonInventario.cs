using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


public class BotonInventario : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public Item item;
    public int cantidad;

    [SerializeField] private Image icono;
    [SerializeField] private TMP_Text textCantidad;
    [SerializeField] private int numeroClasificador;

    public GameObject seleccionado;

    [SerializeField] private DescripcionesController descripcionesController;

    private static BotonInventario instance;
    public static BotonInventario Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        DesactivarVisualizacion();
    }

    private void Update()
    {
        //Si una casilla está vacía se oculta tanto la imagen base de los objetos y también el texto
        if (item == null)
        {
            DesactivarVisualizacion();
        }

        //Si una casilla está ocupada se muestra tanto la imagen base de los objetos y también el texto
        else
        {
            icono.sprite = item.sprite;
            icono.gameObject.SetActive(true);

            if (cantidad == 1)
            {
                textCantidad.text = "";
            }

            else
            {
                textCantidad.text = cantidad.ToString("0");
            }
        }
    }

    private void DesactivarVisualizacion()
    {
        icono.gameObject.SetActive(false);

        textCantidad.text = "";
    }

    public void ClickIzquierdoInventario()
    {
        if (DragAndDropController.Instance.itemDnD == item)
        {
            DragAndDropController.Instance.Anadir(numeroClasificador);
        }

        else
        {
            DragAndDropController.Instance.Copiar(numeroClasificador);
        }
    }

    private void ClickDerechoInventario()
    {
        if (DragAndDropController.Instance.itemDnD == item)
        {
            DragAndDropController.Instance.AnadirIndividual(numeroClasificador);
        }

        else
        {
            DragAndDropController.Instance.CopiarIndividual(numeroClasificador);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Lógica para cuando pulsas el clic derecho del ratón
            ClickDerechoInventario();
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            descripcionesController.panelDescripciones.SetActive(false);
        }
        else
        {
            descripcionesController.textNombre.text = item.nombre;
            descripcionesController.textDescripciones.text = item.descripcion;
            descripcionesController.panelDescripciones.SetActive(true);
        }
    }
}