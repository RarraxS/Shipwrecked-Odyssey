//using UnityEngine;

//public class ItemToolbarPanelOld : ItemPanelOld
//{
//    [SerializeField] private ToolBarController toolbarController;
//    private int herramientaSeleccionadaActual;

//    //Es la encargada del resaltado inventario del objeto que se tiene seleccionado actualmente
//    private void Start()
//    {
//        Iniciar();//Cuenta cuantos botones hay en el inventario y actualiza la imagen y el número de objetos en cada casilla del inventario
//        toolbarController.onChange += Resaltar;//Cuando se mueve hacia alante o hacia atrás 
//        Resaltar(0);//Al iniciar el juego por primera vez se marca la primera casilla del inventario
//    }

//    public override void OnClick(int id)
//    {
//        //Se actualiza cual nueva herramienta seleccionada y se marca
//        toolbarController.Set(id);
//        Resaltar(id);
//    }

//    public void Resaltar(int id)
//    {
//        //Se resalta el hueco del inventario que se está seleccionando
//        botones[herramientaSeleccionadaActual].Resaltar(false);
//        herramientaSeleccionadaActual = id;
//        botones[herramientaSeleccionadaActual].Resaltar(true);
//    }
//}
