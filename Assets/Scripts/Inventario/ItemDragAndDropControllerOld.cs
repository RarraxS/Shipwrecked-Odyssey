//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class ItemDragAndDropControllerOld : MonoBehaviour
//{
//    [SerializeField] ItemSlot itemSlot;
//    [SerializeField] GameObject itemIcon;
//    RectTransform iconTransform;
//    Image itemIconImage;

//    private static ItemDragAndDropControllerOld instance;
//    public static ItemDragAndDropControllerOld Instance
//    {
//        get { return instance; }
//    }

//    private void Start()
//    {
//        itemSlot = new ItemSlot();
//        iconTransform = itemIcon.GetComponent<RectTransform>();
//        itemIconImage = itemIcon.GetComponent<Image>();
//    }

//    void Update()
//    {
//        DevolverItem();
//    }

//    internal void OnClick(ItemSlot itemSlot)
//    {
//        if (this.itemSlot.item == null)
//        {
//            this.itemSlot.Copy(itemSlot);
//            itemSlot.Clear();
//        }
//        else
//        {
//            Item item = itemSlot.item;
//            int count = itemSlot.numItems;

//            itemSlot.Copy(this.itemSlot);
//            this.itemSlot.Set(item, count);
//        }

//        ActualizarIcono();
//    }

//    void ActualizarIcono()
//    {
//        if (itemSlot.item == null)
//        {
//            itemIcon.SetActive(false);
//        }
//        else
//        {
//            itemIcon.SetActive(true);
//            itemIconImage.sprite = itemSlot.item.icono;
//        }
//    }

//    void DevolverItem()
//    {
//        if (itemIcon.activeInHierarchy == true)
//        {
//            iconTransform.position = Input.mousePosition;

//            if (Input.GetMouseButtonDown(0))
//            {
//                if (EventSystem.current.IsPointerOverGameObject() == false)
//                {
//                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                    worldPosition.z = -5;

//                    ItemSpawnerManager.instance.SpawnItem(worldPosition, itemSlot.item, itemSlot.numItems);
//                    itemSlot.Clear();
//                    ActualizarIcono();
//                }
//            }
//        }
//    }
//}
