using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private TileBase tile;

    public Vector3Int markedCellPosition;
    private Vector3Int oldCellPosition;
    private bool show;

    private void Update()
    {
        if (show == false)
        {
            return;
        }
        else
        {
            targetTilemap.SetTile(oldCellPosition, null); //Marca que el tile seleccionado en el frame anterior se vacíe
            targetTilemap.SetTile(markedCellPosition, tile); //Marca que el tile seleccionado en este frame se rellene con el tile que se le ha pasado por parámetro
            oldCellPosition = markedCellPosition; //El que ahora era el tile nuevo pasa a ser el tile viejo y el proceso se repite
        }
    }

    public void Show(bool seleccionado)
    {
        //Dependiendo de si la variable "seleccionado" es true o
        //false se muestra el tile al que está apuntando el ratón
        show = seleccionado;
        targetTilemap.gameObject.SetActive(show);

        //Permite interactuar solo cuando el tilemap de marcado esta activo (cuando se esta a la distancia necesaria para interactuar)
        GameManager.Instance.permitirUsarHerramineta = show;
    }
}
