//using System.Collections.Generic;
//using UnityEngine;

//public class ItemPanelOld : MonoBehaviour
//{
//    public ItemContainerOld inventario;
//    public List<BotonInventario> botones;

//    private void Start()
//    {
//        Iniciar();
//    }



//    void Update()
//    {
//        Show();
//    }

//    public void Iniciar()
//    {
//        //Cuenta cuantos botones hay en el inventario y actualiza la imagen y el número de objetos
//        //en cada casilla del inventario

//        SetIndex();//Cuenta los botones de inventario que hay
//        Show();//Actualiza la imagen y el numero de objetos en cada casilla del inventario
//    }

//    private void SetIndex()
//    {
//        for (int i = 0; i < inventario.slots.Count && i < botones.Count; i++)
//        {
//            botones[i].SetIndex(i);
//        }
//    }

//    public void Show()
//    {
//        //Actualiza la imagen y el numero de objetos en cada casilla del inventario
//        for (int i = 0; i < inventario.slots.Count && i < botones.Count; i++)
//        {
//            if (inventario.slots[i].item == null)
//            {
//                botones[i].Clean();
//            }
//            else
//            {
//                botones[i].Set(inventario.slots[i]);
//            }
//        }
//    }

//    public virtual void OnClick(int id)
//    {

//    }
//}
