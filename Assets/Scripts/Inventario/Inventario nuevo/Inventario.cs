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

    public void AnadirInventario(int posicion, string nombre, Sprite sprite, string descripcion, bool stackeable)
    {
        slotInventario[posicion].nombre = nombre;
        slotInventario[posicion].sprite = sprite;
        slotInventario[posicion].descripcion = descripcion;
        slotInventario[posicion].stackeable = stackeable;
        slotInventario[posicion].cantidad = 1;
    }

    public void Sumar(int posicion)
    {
        slotInventario[posicion].cantidad += 1;
    }
}
