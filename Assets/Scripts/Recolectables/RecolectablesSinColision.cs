using UnityEngine;

public class RecolectablesSinColision : MonoBehaviour
{
    [SerializeField] private GameObject mainItem;

    private CollectableObject item;

    private void Start()
    {
        item = mainItem.GetComponent<CollectableObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name
            && item.rotateOnCollisionEnter == true && item.spriteRendererComponent.enabled == true)
        {
            item.animatorComponent.SetTrigger("rotar");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name &&
            item.spriteRendererComponent.enabled == true)
        {
            item.TransparentarObjeto();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == Jugador.Instance.gameObject.name && 
            item.spriteRendererComponent.enabled == true)
        {
            item.RestablecerColor();
        }
    }
}
