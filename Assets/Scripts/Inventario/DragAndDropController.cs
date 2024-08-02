using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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
    [SerializeField] private float cantBarraActualDnD, cantBarraMaxDnD;    

    //Parámetros del item a copiar
    private Item itemNuevo;
    [SerializeField] private int cantidadNuevo;
    [SerializeField] private float cantBarraActualNuevo, cantBarraMaxNuevo;


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

        //Si el DnD no tiene nada se oculta, sino, se muestra
        if (itemDnD == null)
        {
            dragAndDrop.SetActive(false);
        }
        else
        {
            dragAndDrop.SetActive(true);
            itemIconImage.sprite = itemDnD.sprite;

            //Si el objeto del DnD es stackeable y la cantidad es mayor a 1 entonces se muestra
            //el texto con el número de unidades que se tiene, sino, el texto no se muestra
            if (cantidadDnD > 1 && itemDnD.stackeable == true)
            {
                textDnD.text = cantidadDnD.ToString();
            }
            else
            {
                textDnD.text = "";
            }
        }

        Dropeos();
    }

    public void Copiar(int numeroClasificatorio)
    {
        //Permite intercambiar el objeto que hay en el inventario con el que hay en el Drag and Drop

        //Rellena las variables "contenedoras" de los datos del objeto
        itemNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].item;
        cantidadNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].cantidad;
        cantBarraActualNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraActual;
        cantBarraMaxNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraMaxima;

        //Actualiza los datos de las variables del inventario
        Inventario.Instance.slotInventario[numeroClasificatorio].item = itemDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad = cantidadDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraActual = cantBarraActualDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraMaxima = cantBarraMaxDnD;

        //Pasa las variables del "contenedor" a las variables que realmente utiliza el Drag and Drop
        itemDnD = itemNuevo;
        cantidadDnD = cantidadNuevo;
        cantBarraActualDnD = cantBarraActualNuevo;
        cantBarraMaxDnD = cantBarraMaxNuevo;

        //Limpia el "contenedor" para que pueda acoger al próximo objeto
        itemNuevo = null;
        cantidadNuevo = 0;
        cantBarraActualNuevo = 0;
        cantBarraMaxNuevo = 0;
    }

    public void CopiarIndividual(int numeroClasificatorio)
    {
        //Permite intercambiar el objeto que hay en el inventario con el que hay en el Drag and Drop


        //Permite anadir el objeto del DnD a la casilla del inventario
        //siempre y cuando esta posea el mismo tipo de objeto que el DnD
        if (Inventario.Instance.slotInventario[numeroClasificatorio].item != null)
        {
            if (itemDnD == null)
            {
                //Rellena las variables "contenedoras" de los datos del objeto
                itemNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].item;
                cantidadNuevo = 1;
                Inventario.Instance.slotInventario[numeroClasificatorio].cantidad -= 1;
                cantBarraActualNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraActual;
                cantBarraMaxNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraMaxima;

                if (Inventario.Instance.slotInventario[numeroClasificatorio].cantidad <= 0)
                {
                    //Actualiza los datos de las variables del inventario
                    Inventario.Instance.slotInventario[numeroClasificatorio].item = itemDnD;
                    Inventario.Instance.slotInventario[numeroClasificatorio].cantidad = cantidadDnD;
                    Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraActual = cantBarraActualDnD;
                    Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraMaxima = cantBarraMaxDnD;
                }

                //Pasa las variables del "contenedor" a las variables que realmente utiliza el Drag and Drop
                itemDnD = itemNuevo;
                cantidadDnD = cantidadNuevo;
                cantBarraActualDnD = cantBarraActualNuevo;
                cantBarraMaxDnD = cantBarraMaxNuevo;

                //Limpia el "contenedor" para que pueda acoger al próximo objeto
                itemNuevo = null;
                cantidadNuevo = 0;
                cantBarraActualNuevo = 0;
                cantBarraMaxNuevo = 0;
            }

            else
            {
                Copiar(numeroClasificatorio);
            }
        }

        //Esta parte se encarga de cuando no hay ningún objeto en la casilla de poner una del objeto seleccionado
        else
        {
            if (cantidadDnD > 0)
            {
                //Rellena las variables "contenedoras" de los datos del objeto
                Inventario.Instance.slotInventario[numeroClasificatorio].item = itemDnD;
                Inventario.Instance.slotInventario[numeroClasificatorio].cantidad = 1;
                cantidadDnD -= 1;
                Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraActual = cantBarraActualDnD;
                Inventario.Instance.slotInventario[numeroClasificatorio].cantidadBarraMaxima = cantBarraMaxDnD;


                if (cantidadDnD <= 0)
                {
                    ReiniciarContenedorDnD(); ;
                }
            }
        }
    }

    public void Anadir(int numeroClasificatorio)
    {
        //Si el obeto que se está moviendo con el DnD se trata de colocar encima de una casilla
        //con el mismo objeto se suma a la casilla del inventario


        //Suma las cantidades
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad += cantidadDnD;

        //Limpia el los datos de las variables del DnD para que pueda acoger al próximo objeto
        ReiniciarContenedorDnD();
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
            ReiniciarContenedorDnD();
        }
    }

    private void Dropeos()
    {
        //Agrupa todos los dropeos para que quede más limpio
        Dropear();
        DropearIndividual();
    }

    public void Dropear()
    {
        //Dropea todos los objetos que tiene almacenado el DnD en ese momento
        if (dragAndDrop.activeInHierarchy == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() == false)
                {
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPosition.z = 0;

                    for (int i = 0; i < cantidadDnD; i++)
                    {
                        if (cantBarraMaxDnD > 0)
                        {
                            PickUpItem pickUpDrop = itemDnD.objetoSuelo.gameObject.GetComponent<PickUpItem>();

                            pickUpDrop.cantidadBarraActual = cantBarraActualDnD;
                            pickUpDrop.cantidadBarraMaxima = cantBarraMaxDnD;
                        }

                        GameObject itemSuelo = Instantiate(itemDnD.objetoSuelo, worldPosition, Quaternion.identity);
                    }

                    ReiniciarContenedorDnD();
                }
            }
        }       
    }

    public void DropearIndividual()
    {
        //Dropea unicamente una unidad los objetos que tiene almacenado el DnD en ese momento
        if (dragAndDrop.activeInHierarchy == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (EventSystem.current.IsPointerOverGameObject() == false)
                {
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPosition.z = 0;

                    if (cantBarraMaxDnD > 0)
                    {
                        PickUpItem pickUpDrop = itemDnD.objetoSuelo.gameObject.GetComponent<PickUpItem>();

                        pickUpDrop.cantidadBarraActual = cantBarraActualDnD;
                        pickUpDrop.cantidadBarraMaxima = cantBarraMaxDnD;
                    }

                    GameObject itemSuelo = Instantiate(itemDnD.objetoSuelo, worldPosition, Quaternion.identity);
                    cantidadDnD -= 1;

                    if (cantidadDnD <= 0)
                    {
                        ReiniciarContenedorDnD();
                    }
                }
            }
        }
    }

    private void ReiniciarContenedorDnD()
    {
        itemDnD = null;
        cantidadDnD = 0;
        cantBarraActualDnD = 0;
        cantBarraMaxDnD = 0;
    }
}