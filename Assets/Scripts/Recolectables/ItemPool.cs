using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<GameObject> itemList;
    [SerializeField] private int poolSize;

    private static ItemPool instance;
    public static ItemPool Instance { get { return instance; } }

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
            itemList.Add(item);
            item.transform.parent = transform;
        }
    }

    public GameObject RequestItem()
    {
        for(int i = 0; i < itemList.Count; i++)
        {
            if (!itemList[i].activeSelf)
            {
                itemList[i].SetActive(true);
                return itemList[i];
            }
        }

        AddItemsToPool(1);
        itemList[itemList.Count - 1].SetActive(true);
        return itemList[itemList.Count - 1];
    }
}
