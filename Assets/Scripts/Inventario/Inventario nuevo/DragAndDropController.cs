using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DragAndDropController : MonoBehaviour
{
    //Drag and drop visual
    [SerializeField] private GameObject dragAndDrop;
    [SerializeField] private TMP_Text textDnD;
    private RectTransform iconTransform;
    private Image itemIconImage;
    [SerializeField] private Vector3 distancia;

    //Parámetros del item que se está mostrando actualmente
    public Item itemDnD;
    [SerializeField] private int cantidadDnD;

    //Parámetros del item a copiar
    private Item itemNuevo;
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
        textDnD = textDnD.GetComponent<TMP_Text>();
    }

    void Update()
    {
        //Actualiza la posición del objeto y su sprite
        iconTransform.position = Input.mousePosition + distancia;
        
        if (itemDnD == null)
        {
            dragAndDrop.SetActive(false);
        }
        else
        {
            dragAndDrop.SetActive(true);
            itemIconImage.sprite = itemDnD.sprite;
            textDnD.text = cantidadDnD.ToString();
        }

        //Cuando no hay ningún objeto en el DnD actual el sprite del
        //DnD se oculta, cuando si qye hay un objeto se vuelve visible
        //if (itemDnD == null)
        //{
        //    //itemIconImage.color = new Color(255, 255, 255, 0);
        //    itemIconImage.gameObject.SetActive(false);
        //}

        //else
        //{
        //    //itemIconImage.color = new Color(255, 255, 255, 255);
        //    itemIconImage.gameObject.SetActive(true);
        //}

    }

    public void Copiar(int numeroClasificatorio)
    {
        //Permite intercambiar el objeto que hay en el inventario con el que hay en el Drag and Drop

        //Rellena las variables "contenedoras" de los datos del objeto
        itemNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].item;
        cantidadNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].cantidad;

        //Actualiza los datos de las variables del inventario
        Inventario.Instance.slotInventario[numeroClasificatorio].item = itemDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad = cantidadDnD;

        //Pasa las variables del "contenedor" a las variables que realmente utiliza el Drag and Drop
        itemDnD = itemNuevo;
        cantidadDnD = cantidadNuevo;

        //Limpia el "contenedor" para que pueda acoger al próximo objeto
        itemNuevo = null;
        cantidadNuevo = 0;
    }

    public void CopiarIndividual(int numeroClasificatorio)
    {
        //Permite intercambiar el objeto que hay en el inventario con el que hay en el Drag and Drop

        if (Inventario.Instance.slotInventario[numeroClasificatorio].item != null)
        {
            //Rellena las variables "contenedoras" de los datos del objeto
            itemNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].item;
            cantidadNuevo = 1;
            Inventario.Instance.slotInventario[numeroClasificatorio].cantidad -= 1;

            if (Inventario.Instance.slotInventario[numeroClasificatorio].cantidad <= 0)
            {
                //Actualiza los datos de las variables del inventario
                Inventario.Instance.slotInventario[numeroClasificatorio].item = itemDnD;
                Inventario.Instance.slotInventario[numeroClasificatorio].cantidad = cantidadDnD;
            }

            //Pasa las variables del "contenedor" a las variables que realmente utiliza el Drag and Drop
            itemDnD = itemNuevo;
            cantidadDnD = cantidadNuevo;

            //Limpia el "contenedor" para que pueda acoger al próximo objeto
            itemNuevo = null;
            cantidadNuevo = 0;
        }

        else
        {
            if (cantidadDnD > 0)
            {
                //Rellena las variables "contenedoras" de los datos del objeto
                Inventario.Instance.slotInventario[numeroClasificatorio].item = itemDnD;
                Inventario.Instance.slotInventario[numeroClasificatorio].cantidad = 1;
                cantidadDnD -= 1;

                if (cantidadDnD <= 0)
                {
                    itemDnD = null;
                    cantidadDnD = 0;
                }
            }
        }

        //Debug.Log("Añadir");
    }

    public void Anadir(int numeroClasificatorio)
    {
        //Si el obeto que se está moviendo con el DnD se trata de colocar encima de una casilla
        //con el mismo objeto se suma a la casilla del inventario

        //Suma las cantidades
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad += cantidadDnD;

        //Limpia el los datos de las variables del DnD para que pueda acoger al próximo objeto
        itemDnD = null;
        cantidadDnD = 0;
    }

    public void AnadirIndividual(int numeroClasificatorio)
    {
        //Si el obeto que se está moviendo con el DnD se trata de colocar encima de una casilla
        //con el mismo objeto usando el click derecho, se suma uno a la casilla del inventario

        //Pasa la unidad del DnD al inventario
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad += 1;
        cantidadDnD -= 1;

        //Debug.Log("Añadir");

        if (cantidadDnD <= 0)
        {
            //Limpia el los datos de las variables del DnD para que pueda acoger al próximo objeto
            itemDnD= null;
            cantidadDnD = 0;
        }
    }
}