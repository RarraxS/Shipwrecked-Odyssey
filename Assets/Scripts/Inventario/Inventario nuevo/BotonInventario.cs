using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BotonInventario : MonoBehaviour, IPointerClickHandler
{
    public string nombre;
    public Sprite sprite;
    public string descripcion;
    public bool stackeable;
    public int cantidad;

    [SerializeField] private Image icono;
    [SerializeField] private TMP_Text textCantidad;
    [SerializeField] private int numeroClasificador;

    private static BotonInventario instance;
    public static BotonInventario Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        //Si una casilla está vacía se oculta tanto la imagen base de los objetos y también el texto
        if (nombre == "")
        {
            icono.gameObject.SetActive(false);

            textCantidad.text = "";
        }

        //Si una casilla está ocupada se muestra tanto la imagen base de los objetos y también el texto
        else
        {
            icono.sprite = sprite;
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

    public void ClickIzquierdoInventario()
    {
        if (DragAndDropController.Instance.nombreDnD == nombre)
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
        if (DragAndDropController.Instance.nombreDnD == nombre)
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
            //Debug.Log("Clic derecho");
        }
    }
}