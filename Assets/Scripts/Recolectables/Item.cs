using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public bool stackable;
    public Sprite sprite;
    public string description;
    public GameObject droppedItem;

    public string tool;
    public int level;
    public int toolDamage;
    public int energyPerHit;

    public bool semilla;
    public CollectableObject itemToPlaceOnWorld;
}