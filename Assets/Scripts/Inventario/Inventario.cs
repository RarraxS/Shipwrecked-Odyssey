using UnityEngine;

public class Inventario : MonoBehaviour, IObserverNum
{
    [SerializeField] public BotonInventario[] slotInventario;


    private static Inventario instance;
    public static Inventario Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        ObserverManager.Instance.AddObserverNum(this);
    }

    public void AnadirInventario(int posicion, Item item, int quantity, float cantBarraActual, float cantBarraMaxima)
    {
        //Cuando no tienes ese objeto en el inventario el objeto se establece al objeto en concreto con cantidad 1 
        slotInventario[posicion].item = item;
        slotInventario[posicion].cantidad = quantity;
        slotInventario[posicion].cantidadBarraActual= cantBarraActual;
        slotInventario[posicion].cantidadBarraMaxima= cantBarraMaxima;
        slotInventario[posicion].ActualizarInformacion();
        ObserverManager.Instance.NotifyObserver("Change on toolbar");
    }

    public void Sumar(int posicion, int quantity)
    {
        //Si el obejto si existe en el inventario entonces se le suma 1 a la cantidad
        slotInventario[posicion].cantidad += quantity;
        slotInventario[posicion].ActualizarInformacion();

        ObserverManager.Instance.NotifyObserver("Change on toolbar");
    }

    private void Restar(int posicion)
    {
        slotInventario[posicion].cantidad -= 1;

        if (slotInventario[posicion].cantidad <= 0)
        {
            slotInventario[posicion].item = null;
            slotInventario[posicion].cantidad = 0;
        }

        slotInventario[posicion].ActualizarInformacion();

        ObserverManager.Instance.NotifyObserver("Change on toolbar");
    }

    private void ReducirBarraInventario(int posicion)
    {
        slotInventario[posicion].cantidadBarraActual -= 1;

        slotInventario[posicion].ActualizarInformacion();

        ObserverManager.Instance.NotifyObserver("Change on toolbar");
    }

    private void RecargarBarraInventario(int posicion)
    {
        slotInventario[posicion].cantidadBarraActual = slotInventario[posicion].cantidadBarraMaxima;

        slotInventario[posicion].ActualizarInformacion();

        ObserverManager.Instance.NotifyObserver("Change on toolbar");
    }

    public void OnNotify(string eventInfo, int numInfo)
    {
        if (eventInfo == "Remove on inventory")
        {
            Restar(numInfo);
        }

        else if (eventInfo == "Remove on item uses")
        {
            ReducirBarraInventario(numInfo);
        }

        else if (eventInfo == "Reload item uses")
        {
            RecargarBarraInventario(numInfo);
        }
    }
}
