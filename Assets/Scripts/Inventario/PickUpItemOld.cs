//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PickUpItemOld : MonoBehaviour
//{
//    Transform jugador;
//    [SerializeField] float velocidad = 10f;
//    [SerializeField] float distancia = 8f;

//    [SerializeField] Item item;
//    int numObjetos = 1;

//    void Awake()
//    {
//        jugador = GameManager.Instance.Player.transform;
//    }

//    public void Set(Item item, int count)
//    {
//        this.item = item;
//        this.numObjetos = count;

//        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
//        renderer.sprite = item.sprite;
//    }

//    void Update()
//    {
//        float distance = Vector3.Distance(transform.position, jugador.position);
//        if(distance > distancia)
//        {
//            return;
//        }
//        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);

//        if(distance < 0.1f)
//        {
//            if(GameManager.Instance.InventoryContainer != null)
//            {
//                GameManager.Instance.InventoryContainer.Add(item, numObjetos);
//            }

//            else
//            {
//                Debug.LogWarning("No hay inventory container attached to game manager");
//            }

//            Destroy(gameObject);
//        }
//    }
//}
