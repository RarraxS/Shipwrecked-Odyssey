using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<GameObject> itemList;
    [SerializeField] private int poolSize;

    private Transform tr;
    private List<Transform> transformList;
    private List<PickUpItem> pickUpItemList;

    private static ItemPool instance;
    public static ItemPool Instance { get { return instance; } }
    private ItemPool()
    {
        transformList = new List<Transform>();
        pickUpItemList = new List<PickUpItem>();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        AddItemsToPool(poolSize);
    }

    private void AddItemsToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject item = Instantiate(itemPrefab);

            item.SetActive(false);
            item.transform.parent = transform;
            itemList.Add(item);

            //tr = item.GetComponent<Transform>();
            //transformList.Add(tr);

            PickUpItem pickUpItem = item.GetComponent<PickUpItem>();

            //PickUpItem itemPU = item.gameObject.GetComponent<PickUpItem>();

            pickUpItemList.Add(pickUpItem);

            //PickUpItem pickUpItem = item.GetComponent<PickUpItem>();
            //if (pickUpItem != null)
            //{
            //    pickUpItemList.Add(pickUpItem);
            //}
            //else
            //{
            //    Debug.LogWarning($"El GameObject {item.name} no tiene un componente PickUpItem.");
            //}

            Debug.Log("Salida");
        }
    }

    public GameObject RequestItem(Vector3 position)
    {
        for(int i = 0; i < itemList.Count; i++)
        {
            if (!itemList[i].activeSelf)
            {
                //transformList[i].position = position;
                itemList[i].transform.position = position;
                itemList[i].SetActive(true);
                return itemList[i];
            }
        }

        AddItemsToPool(1);
        itemList[itemList.Count - 1].SetActive(true);
        return itemList[itemList.Count - 1];
    }

    //Queda programar el que se use la pool
}
