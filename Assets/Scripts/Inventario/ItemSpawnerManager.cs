using UnityEngine;

public class ItemSpawnerManager : MonoBehaviour
{
    public static ItemSpawnerManager instance;
    void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject pickUpItemPrefab;

    public void SpawnItem(Vector3 position, Item item, int count)
    {
        GameObject o = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        o.GetComponent<PickUpItem>().Set(item, count);
    }

}
