using UnityEngine;

[CreateAssetMenu(menuName = "Datos/Item")]
public class Item : ScriptableObject
{
    //Son las variables que posee un item de inventario
    public string nombre;
    public bool stackeable;
    public Sprite sprite;
    public string descripcion;
    public GameObject objetoSuelo;
    public string herramienta;
    public int nivel;
    public int damageHerramienta;
}