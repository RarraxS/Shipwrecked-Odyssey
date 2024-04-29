using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapReadController : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileData tileDataArable, tileDataNoArable;

    private void Start()
    {
        //La idea es que vea el sprite que hay en ese tile, y compruebe si ese es arable y no
        //tiene uno no arable encima entonces te permite arar y asi con el resto de herramientas
    }

    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition)
    {
        //Se encarga de convertir el cursor del ratón en una posición en el propio juego para que
        //luego a la hora de hacer cosas con los tilemaps sel juego sepa que tile tiene que modificar


        //Guarda la posicion del mouse en el mundo
        Vector3 worldPosition;

        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        //Convierte esa posicion del mundo en una posicion de tile
        Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);

        return gridPosition;
    }
}
