using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tiles recolectables")]
public class Recolectables : ScriptableObject
{
    public TileBase tile;
    public int puntosDeVida;
    public string herramienta;
}
