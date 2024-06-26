using System;
using UnityEngine;

public class RecolectablesSinColision : MonoBehaviour
{
    [SerializeField] private GameObject objetoPrincipal;

    private ObjetosRecolectables objeto;

    private void Start()
    {
        objeto = objetoPrincipal.GetComponent<ObjetosRecolectables>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name)
        {
            objeto.TransparentarObjeto();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name)
        {
            objeto.RestablecerColor();
        }
    }
}
