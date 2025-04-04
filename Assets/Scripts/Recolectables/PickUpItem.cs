using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform jugador;//Es el objeto al que va a seguir el item

    public float distanciaMaximaInteraccion;//La distancia maxima a la que un objeto seguira al jugador
    public int velocidad;//La velocidad con la que el objeto persigue al jugador
    public Item item;//El item que se le sumara al inventario del jugador cuando recoja el objeto
    public int quantity;

    private bool permitirMovimiento;//Se encarga de hacer que un objeto no siga al jugador cuando este no cabe en el inventario
    private bool distanciaAceptada;//El objeto esta a una distancia menor a la distancia maxima por lo que se puede acercar al jugador
    private bool dentro = false;//Comprueba si el objeto esta lo suficientemente cerca del jugador como para anadirse al inventario

    public float cantidadBarraActual, cantidadBarraMaxima;

    void Start()
    {
        jugador = GameManager.Instance.Player.transform;
    }


    void Update()
    {
        //Seguir al jugador ---------------------------------------------------------------------------------------

        //Comprueba si la distancia la a la que esta el objeto del jugador esta
        //dentro de la siatancia maxima permitida para que se acerque al jugador
        float distance = Vector3.Distance(transform.position, jugador.position);

        if (distance > distanciaMaximaInteraccion)
        {
            distanciaAceptada = false;
        }

        else
        {
            distanciaAceptada = true;
        }

        

        if (distanciaAceptada == true)
        {
            Seguir(distance);
        }
    }

    private void Seguir(float distance)
    {
        //Esta parte es la que se encarga de que no siga al jugador cuando el item no cabe en el inventario
        for (int i = 0; i < Inventario.Instance.slotInventario.Length; i++)
        {
            permitirMovimiento = false;

            if (item == Inventario.Instance.slotInventario[i].item && item.stackable == true ||
                Inventario.Instance.slotInventario[i].item == null)
            {
                permitirMovimiento = true;
                break;
            }
        }

        //Si el objeto si cabe en el inventario entonces el objeto va a ir tras el jugador

        if (permitirMovimiento == true && distanciaAceptada == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        }

        //---------------------------------------------------------------------------------------------------------

        //Si se objeto "item" está a una distancia muy cercana del jugador y al jugador
        //le cabe en el inventario, el objeto se destruye y entre en dicho invenario
        if (distance < 0.1f && permitirMovimiento == true)
        {
            //Recorre todas las posiciones del inventario, si hay alguna posicion que ya tenga ese objeto y
            //el objeto es stackeable lo mete ahi, o si no hay ninguna posicion en la que haya acumulaciones
            //de ese objeto encuentra alguna posición vacia y entonces lo mete ahi. Si no tiene espacio,
            //entonces el objeto no segue al jugador

            for (int i = 0; i < Inventario.Instance.slotInventario.Length; i++)
            {
                if (item == Inventario.Instance.slotInventario[i].item && item.stackable == true)
                {
                    //Suma objetos a un espacio ya existente
                    Inventario.Instance.Sumar(i, quantity);
                    gameObject.SetActive(false);
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
                        Inventario.Instance.AnadirInventario(i, item, quantity, cantidadBarraActual, cantidadBarraMaxima);
                        
                        gameObject.SetActive(false);

                        break;
                    }
                }
            }
        }
    }
}
