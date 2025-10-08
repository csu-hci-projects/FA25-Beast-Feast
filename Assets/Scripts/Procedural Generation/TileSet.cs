using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileSet", menuName = "Custom/Procedural Generation/TileSet")]
public class TileSet : ScriptableObject
{
    [SerializeField] Color wallColor;
    [SerializeField] TileVariant[] tiles = new TileVariant[16];

    public Color WallColor => wallColor;

    public GameObject GetTile(int tileIndex)
    {
        if (tileIndex >= tiles.Length)
        {
            return null;
        }
        return tiles[tileIndex].GetRandomTile();
    }
}
