//using TMPro;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class BotonInventarioOld : MonoBehaviour, IPointerClickHandler
//{
//    [SerializeField] Image icono;
//    [SerializeField] TMP_Text text;
//    [SerializeField] Image highlight;

//    int myIndex;

//    public void SetIndex(int index)
//    {
//        myIndex = index;
//    }

//    public void Set(ItemSlot slot)
//    {
//        icono.gameObject.SetActive(true);
//        icono.sprite = slot.item.sprite;

//        if (slot.item.stackeable == true)
//        {
//            if (slot.numItems == 1)
//            {
//                text.gameObject.SetActive(false);
//            }

//            else
//            {
//                text.gameObject.SetActive(true);
//                text.text = slot.numItems.ToString();
//            }
//        }
//        else 
//        {
//            text.gameObject.SetActive(false);
//        }
//    }

//    public void Clean()
//    {
//        icono.sprite = null;
//        icono.gameObject.SetActive(false);

//        text.gameObject.SetActive(false);
//    }

//    public void OnPointerClick(PointerEventData eventData)
//    {
//        ItemPanelOld itemPanel = transform.parent.GetComponent<ItemPanelOld>();
//        itemPanel.OnClick(myIndex);
//    }

//    public void Resaltar(bool b)
//    {
//        highlight.gameObject.SetActive(b);
//    }
//}
