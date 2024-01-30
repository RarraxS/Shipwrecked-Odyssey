using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tile Data")]
public class TileData : ScriptableObject
{
    //Se trata de una lista en la cual se establece si un scriptable object que le
    //pasamos con los sprites de los tiles arables y los no arables por otro lado

    public List<TileBase> tiles;
    
    public bool arable;
}