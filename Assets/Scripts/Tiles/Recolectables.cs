using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Datos/Tiles recolectables")]
public class Recolectables : ScriptableObject
{
    public TileBase tile;
    public int puntosDeVida;
    public GameObject objetoRecolectable;
    public int numeroRecolectables;
    public string herramienta;
    public int nivelMinimoHerramienta;
}
