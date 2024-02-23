using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BotonInventario : MonoBehaviour
{
    public string nombre;
    public Sprite sprite;
    public string descripcion;
    public bool stackeable;
    public int cantidad;

    [SerializeField] private Image icono;
    [SerializeField] private TMP_Text textCantidad;
    [SerializeField] private int numeroClasificador;
    
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

    public void ClickInventario()
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
}