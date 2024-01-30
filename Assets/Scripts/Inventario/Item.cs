using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    //Son las variables que posee un item de inventario
    public string Nombre;
    public bool Stackeable;
    public Sprite icono;
}