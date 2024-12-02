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
            targetTilemap.SetTile(oldCellPosition, null);
            targetTilemap.SetTile(markedCellPosition, tile);
            oldCellPosition = markedCellPosition;

            return;
        }
    }

    public void Show(bool selected)
    {
        show = selected;
        targetTilemap.gameObject.SetActive(show);

        GameManager.Instance.permitirUsarHerramineta = show;
    }
}
