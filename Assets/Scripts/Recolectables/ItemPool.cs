using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<GameObject> itemList;
    [SerializeField] private int poolSize;

    private List<Transform> transformList = new List<Transform>();
    private List<SpriteRenderer> spriteRendererList = new List<SpriteRenderer>();
    private List<PickUpItem> pickUpItemList = new List<PickUpItem>();

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
            item.transform.parent = transform;
            itemList.Add(item);

            Transform transformComponent = item.GetComponent<Transform>();
            transformList.Add(transformComponent);

            SpriteRenderer spriteRendererComponent = item.GetComponent<SpriteRenderer>();
            spriteRendererList.Add(spriteRendererComponent);

            PickUpItem pickUpItemComponent = item.GetComponent<PickUpItem>();
            pickUpItemList.Add(pickUpItemComponent);
        }
    }

    public GameObject RequestItem(Vector3 _position)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (!itemList[i].activeSelf)
            {
                transformList[i].position = _position;

                itemList[i].SetActive(true);
                return itemList[i];
            }
        }

        AddItemsToPool(1);

        transformList[itemList.Count - 1].position = _position;

        itemList[itemList.Count - 1].SetActive(true);
        return itemList[itemList.Count - 1];
    }

    //Queda programar el que se use la pool
}
