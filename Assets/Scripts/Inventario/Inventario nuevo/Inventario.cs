using UnityEngine;
using UnityEngine.EventSystems;

public class Inventario : MonoBehaviour
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
    }

    public void AnadirInventario(int posicion, Item item)
    {
        slotInventario[posicion].item = item;
        slotInventario[posicion].cantidad = 1;
    }

    public void Sumar(int posicion)
    {
        slotInventario[posicion].cantidad += 1;
    }
}
