using UnityEngine;

public class RecolectablesSinColision : MonoBehaviour
{
    [SerializeField] private GameObject objetoPrincipal;

    private ObjetosRecolectables objeto;

    private void Start()
    {
        objeto = objetoPrincipal.GetComponent<ObjetosRecolectables>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name
            && objeto.rotarEntrarColision == true && objeto.componenteSpriteRenderer.enabled == true)
        {
            objeto.componenteAnimator.SetTrigger("rotar");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name &&
            objeto.componenteSpriteRenderer.enabled == true)
        {
            objeto.TransparentarObjeto();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name && 
            objeto.componenteSpriteRenderer.enabled == true)
        {
            objeto.RestablecerColor();
        }
    }
}
