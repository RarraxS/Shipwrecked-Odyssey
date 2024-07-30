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

    public void AnadirInventario(int posicion, Item item)
    {
        //Cuando no tienes ese objeto en el inventario el objeto se establece al objeto en concreto con cantidad 1 
        slotInventario[posicion].item = item;
        slotInventario[posicion].cantidad = 1;
        slotInventario[posicion].ActualizarInformacion();
        ObserverManager.Instance.NotifyObserver("Cambio en la toolbar");
    }

    public void Sumar(int posicion)
    {
        //Si el obejto si existe en el inventario entonces se le suma 1 a la cantidad
        slotInventario[posicion].cantidad += 1;
        slotInventario[posicion].ActualizarInformacion();

        ObserverManager.Instance.NotifyObserver("Cambio en la toolbar");
    }

    public void Restar(int posicion)
    {
        slotInventario[posicion].cantidad -= 1;

        if (slotInventario[posicion].cantidad <= 0)
        {
            slotInventario[posicion].item = null;
            slotInventario[posicion].cantidad = 0;
        }

        slotInventario[posicion].ActualizarInformacion();

        ObserverManager.Instance.NotifyObserver("Cambio en la toolbar");
    }

    public void OnNotify(string eventInfo, int numInfo)
    {
        if (eventInfo == "Cambio en el inventario")
        {
            Restar(numInfo);
        }
    }
}
