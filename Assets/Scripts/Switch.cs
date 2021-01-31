using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public static List<Switch> switches = new List<Switch>();

    public Sprite onSprite;
    public DataTile onTile;
    public Sprite offSprite;
    public DataTile offTile;
    public bool isON = true;
    public Vector3Int targetCell;

    void Awake() {
        switches.Add(this);
    }

    public Vector3Int GetCurrentCell() {
        return GameTiles.instance.tilemapFloor.WorldToCell(transform.position);
    }

    public void Toggle() {
        if (isON) {
            isON = false;
            GetComponent<SpriteRenderer>().sprite = offSprite;
            GameTiles.instance.tilemapObstacles.SetTile(targetCell, offTile);
            GameTiles.instance.worldTileData[targetCell.x, targetCell.y].traversable = offTile.traversable;
            GameTiles.instance.worldTileData[targetCell.x, targetCell.y].blocksVision = offTile.blocksVision;
        } else {
            isON = true;
            GetComponent<SpriteRenderer>().sprite = onSprite;
            GameTiles.instance.tilemapObstacles.SetTile(targetCell, onTile);
            GameTiles.instance.worldTileData[targetCell.x, targetCell.y].traversable = onTile.traversable;
            GameTiles.instance.worldTileData[targetCell.x, targetCell.y].blocksVision = onTile.blocksVision;
        }
    }
}
