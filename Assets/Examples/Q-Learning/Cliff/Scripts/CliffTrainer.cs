using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CliffTrainer : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile[] tiles;

    int[,] map = {
        { -1, -1, -1, -1, -1, -1, -1 },
        { -1,  0,  0,  0,  0,  0, -1 },
        { -1,  0,  0,  0,  0,  0, -1 },
        { -1,  0,  0,  0,  0,  0, -1 },
        { -1,  0, -1, -1, -1,  0, -1 },
        { -1, -1, -1, -1, -1, -1, -1 }
    };
    Vector2Int startPos = new Vector2Int(1, 1);
    Vector2Int endPos = new Vector2Int(5, 1);

    int[,] mapScores;

    void Start()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[map.GetLength(0) - y - 1, x] == -1)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[0]);
                else if (map[map.GetLength(0) - y - 1, x] == 0)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[1]);
            }
        }

        Camera.main.transform.position = new Vector3(map.GetLength(0) * .5f + .5f, map.GetLength(1) * .5f - .5f, -10);
        Camera.main.orthographicSize = map.GetLength(1) * 0.5f + 1f;
    }
}
