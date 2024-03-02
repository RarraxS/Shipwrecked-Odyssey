using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropController : MonoBehaviour
{
    //Drag and drop visual
    [SerializeField] private GameObject dragAndDrop;
    private RectTransform iconTransform;
    [SerializeField] private Vector3 distancia;
    private Image itemIconImage;

    //Parámetros del item que se está mostrando actualmente
    public  string nombreDnD;
    [SerializeField] private Sprite spriteDnD;
    [SerializeField] private string descripcionDnD;
    [SerializeField] private bool stackeableDnD;
    [SerializeField] private int cantidadDnD;

    //Parámetros del item a copiar
    [SerializeField] private string nombreNuevo;
    [SerializeField] private Sprite spriteNuevo;
    [SerializeField] private string descripcionNuevo;
    [SerializeField] private bool stackeableNuevo;
    [SerializeField] private int cantidadNuevo;


    private static DragAndDropController instance;
    public static DragAndDropController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    
        iconTransform = dragAndDrop.GetComponent<RectTransform>();
        itemIconImage = dragAndDrop.GetComponent<Image>();
    }

    void Update()
    {
        //Actualiza la posición del objeto y su sprite
        iconTransform.position = Input.mousePosition + distancia;
        itemIconImage.sprite = spriteDnD;

        //Cuando no hay ningún objeto en el DnD actual el sprite del
        //DnD se oculta, cuando si qye hay un objeto se vuelve visible
        if (nombreDnD != "")
        {
            itemIconImage.color = new Color(255, 255, 255, 255);
        }

        else
        {
            itemIconImage.color = new Color(255, 255, 255, 0);
        }

    }

    public void Copiar(int numeroClasificatorio)
    {
        //Permite intercambiar el objeto que hay en el inventario con el que hay en el Drag and Drop

        //Rellena las variables "contenedoras" de los datos del objeto
        nombreNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].nombre;
        spriteNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].sprite;
        descripcionNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].descripcion;
        stackeableNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].stackeable;
        cantidadNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].cantidad;

        //Actualiza los datos de las variables del inventario
        Inventario.Instance.slotInventario[numeroClasificatorio].nombre = nombreDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].sprite = spriteDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].descripcion = descripcionDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].stackeable = stackeableDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad = cantidadDnD;

        //Pasa las variables del "contenedor" a las variables que realmente utiliza el Drag and Drop
        nombreDnD = nombreNuevo;
        spriteDnD = spriteNuevo;
        descripcionDnD = descripcionNuevo;
        stackeableDnD = stackeableNuevo;
        cantidadDnD = cantidadNuevo;

        //Limpia el "contenedor" para que pueda acoger al próximo objeto
        nombreNuevo = "";
        spriteNuevo = null;
        descripcionNuevo = null;
        stackeableNuevo = false;
        cantidadNuevo = 0;
    }

    public void CopiarIndividual(int numeroClasificatorio)
    {
        //Permite intercambiar el objeto que hay en el inventario con el que hay en el Drag and Drop

        //Rellena las variables "contenedoras" de los datos del objeto
        nombreNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].nombre;
        spriteNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].sprite;
        descripcionNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].descripcion;
        stackeableNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].stackeable;
        cantidadNuevo = 1;
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad -= 1;

        if (Inventario.Instance.slotInventario[numeroClasificatorio].cantidad <= 0)
        {
            //Actualiza los datos de las variables del inventario
            Inventario.Instance.slotInventario[numeroClasificatorio].nombre = nombreDnD;
            Inventario.Instance.slotInventario[numeroClasificatorio].sprite = spriteDnD;
            Inventario.Instance.slotInventario[numeroClasificatorio].descripcion = descripcionDnD;
            Inventario.Instance.slotInventario[numeroClasificatorio].stackeable = stackeableDnD;
            Inventario.Instance.slotInventario[numeroClasificatorio].cantidad = cantidadDnD;
        }

        //Pasa las variables del "contenedor" a las variables que realmente utiliza el Drag and Drop
        nombreDnD = nombreNuevo;
        spriteDnD = spriteNuevo;
        descripcionDnD = descripcionNuevo;
        stackeableDnD = stackeableNuevo;
        cantidadDnD = cantidadNuevo;

        //Limpia el "contenedor" para que pueda acoger al próximo objeto
        nombreNuevo = "";
        spriteNuevo = null;
        descripcionNuevo = null;
        stackeableNuevo = false;
        cantidadNuevo = 0;
    }

    public void Anadir(int numeroClasificatorio)
    {
        //Si el obeto que se está moviendo con el DnD se trata de colocar encima de una casilla
        //con el mismo objeto se suma a la casilla del inventario

        //Suma las cantidades
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad += cantidadDnD;

        //Limpia el los datos de las variables del DnD para que pueda acoger al próximo objeto
        nombreDnD = null;
        spriteDnD = null;
        descripcionDnD = null;
        stackeableDnD = false;
        cantidadDnD = 0;
    }

    public void AnadirIndividual(int numeroClasificatorio)
    {
        //Si el obeto que se está moviendo con el DnD se trata de colocar encima de una casilla
        //con el mismo objeto usando el click derecho, se suma uno a la casilla del inventario

        //Pasa la unidad del DnD al inventario
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad += 1;
        cantidadDnD -= 1;

        if (cantidadDnD <= 0)
        {
            //Limpia el los datos de las variables del DnD para que pueda acoger al próximo objeto
            nombreDnD = "";
            spriteDnD = null;
            descripcionDnD = null;
            stackeableDnD = false;
            cantidadDnD = 0;
        }
    }
}