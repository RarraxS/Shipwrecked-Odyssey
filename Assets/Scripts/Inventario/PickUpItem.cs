using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform jugador;//Es el objeto al que va a seguir el item

    [SerializeField] float distanciaMaximaInteraccion;//La distancia máxima a la que un objeto seguirá al jugador
    [SerializeField] int velocidad;//La velocidad con la que el objeto persigue al jugador
    [SerializeField] Item item;//El item que se le sumará al inventario del jugador cuando recoja el objeto

    private bool permitirMovimiento;
    private bool dentro = false;

    void Start()
    {
        jugador = GameManager.Instance.Player.transform;
    }

    
    void Update()
    {
        //Seguir al jugador ---------------------------------------------------------------------------------------
        float distance = Vector3.Distance(transform.position, jugador.position);
        if (distance > distanciaMaximaInteraccion)
        {
            return;
        }

            //Esta parte es la que se encarga de que no siga al jugador cuando el item no cabe  en el inventario
        
        for (int i = 0; i < Inventario.Instance.slotInventario.Length; i++)
        {
            permitirMovimiento = false;

            if (item == Inventario.Instance.slotInventario[i].item && item.stackeable == true ||
                Inventario.Instance.slotInventario[i].item == null)
            {
                permitirMovimiento = true;
                break;
            }
        }

        if(permitirMovimiento == true)
        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);

        //---------------------------------------------------------------------------------------------------------
        if (distance < 0.1f)
        {
            //Recorre todas las posiciones del inventario, si hay alguna posición que ya tenga ese objeto y
            //el objeto es stackeable lo mete ahí, o si no hay ninguna posición en la que haya acumulaciones
            //de ese objeto encuentra alguna posición vacía y entonces lo mete ahí. Si no tiene espacio,
            //entonces el objeto no segue al jugador
            for (int i = 0; i < Inventario.Instance.slotInventario.Length; i++)
            {
                if (item == Inventario.Instance.slotInventario[i].item && item.stackeable == true)
                {
                    //Suma objetos a un espacio ya existente
                    Debug.Log("Sumado");
                    Inventario.Instance.Sumar(i);
                    Destroy(gameObject);
                    dentro = true;
                    break;
                }
            }

            if (dentro == false)
            {
                for (int i = 0; i < Inventario.Instance.slotInventario.Length; i++)
                {
                    if (Inventario.Instance.slotInventario[i].item == null)
                    {
                        //Añade un nuevo espacio
                        Debug.Log("Añadido");
                        Inventario.Instance.AnadirInventario(i, item);
                        Destroy(gameObject);
                        break;
                    }
                }
            }
            dentro = false;
        }
    }
}
