using UnityEngine;

public class RecolectablesSinColision : MonoBehaviour
{
    [SerializeField] private GameObject mainItem;

    private ObjetosRecolectables item;

    private void Start()
    {
        item = mainItem.GetComponent<ObjetosRecolectables>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name
            && item.rotarEntrarColision == true && item.componenteSpriteRenderer.enabled == true)
        {
            item.componenteAnimator.SetTrigger("rotar");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name &&
            item.componenteSpriteRenderer.enabled == true)
        {
            item.TransparentarObjeto();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name && 
            item.componenteSpriteRenderer.enabled == true)
        {
            item.RestablecerColor();
        }
    }
}
