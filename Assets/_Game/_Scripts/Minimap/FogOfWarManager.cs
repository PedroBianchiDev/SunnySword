using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWarManager : MonoBehaviour
{
    [Header("Configurações")]
    public Tilemap fogTilemap;
    public int revealRadius = 4;

    [SerializeField]
    private GameObject tileMapObj;

    private void Start()
    {
        tileMapObj.SetActive(true);
    }
    private void Update()
    {
        Vector3Int playerCellPos = fogTilemap.WorldToCell(transform.position);

        for (int x = -revealRadius; x <= revealRadius; x++)
        {
            for (int y = -revealRadius; y <= revealRadius; y++)
            {
                if (Vector3.Distance(new Vector3(x, y, 0), Vector3.zero) <= revealRadius)
                {
                    Vector3Int currentCell = new Vector3Int(playerCellPos.x + x, playerCellPos.y + y, 0);

                    if (fogTilemap.HasTile(currentCell))
                    {
                        fogTilemap.SetTile(currentCell, null);
                    }
                }
            }
        }
    }
}