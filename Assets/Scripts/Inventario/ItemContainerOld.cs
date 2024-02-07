//using System;
//using System.Collections.Generic;
//using UnityEngine;

//[Serializable]
//public class ItemSlot
//{
//    public Item item;
//    public int numItems;

//    public void Copy(ItemSlot slot)
//    {
//        item = slot.item;
//        numItems = slot.numItems;
//    }

//    public void Set(Item item, int count)
//    {
//        this.item = item;
//        this.numItems = count;
//    }

//    public void Clear()
//    {
//        item = null;
//        numItems = 0;
//    }
//}

//[CreateAssetMenu(menuName = "Data/Item Container")]
//public class ItemContainerOld : ScriptableObject
//{
//    public List<ItemSlot> slots;

//    public void Add(Item item, int count = 1)
//    {
//        if (item.Stackeable == true)
//        {
//            //Si ya tienes una unidad de ese objeto en el inventario
//            ItemSlot itemSlot = slots.Find(x => x.item == item);
//            if (itemSlot != null)
//            {
//                itemSlot.numItems += count;
//            }
//            else
//            {
//                itemSlot = slots.Find(x => x.item == null);
//                if (itemSlot != null)
//                {
//                    itemSlot.item = item;
//                    itemSlot.numItems = count;
//                }
//            }
//        }
//        else
//        {
//            //Añadir objeto no Stackeable al item container
//            ItemSlot itemSlot = slots.Find(x => x.item == null);
//            if (itemSlot != null)
//            {
//                itemSlot.item = item;
//            }
//        }
//    }

//    public ItemSlot Remove(Item item)
//    {
//        ItemSlot itemSlot = slots.Find(x => x.item == item);
//        if (itemSlot != null)
//        {
//            // Creamos una copia del ItemSlot antes de eliminarlo del inventario
//            ItemSlot itemRemoved = new ItemSlot();
//            itemRemoved.Copy(itemSlot);

//            // Eliminamos el objeto del inventario
//            itemSlot.Clear();

//            return itemRemoved;
//        }

//        return null;
//    }
//}