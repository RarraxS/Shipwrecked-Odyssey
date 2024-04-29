using UnityEngine;

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
        //Cuando no tienes ese objeto en el inventario el objeto se establece al objeto en concreto con cantidad 1 
        slotInventario[posicion].item = item;
        slotInventario[posicion].cantidad = 1;
    }

    public void Sumar(int posicion)
    {
        //Si el obejto si existe en el inventario entonces se le suma 1 a la cantidad
        slotInventario[posicion].cantidad += 1;
    }
}
