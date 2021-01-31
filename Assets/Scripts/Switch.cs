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

    private SpriteRenderer spriteRenderer;

    void Awake() {
        switches.Add(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    void Update() {
        // Standard update checks
        Vector3Int currentCell = GetCurrentCell();
        if (!GameTiles.instance.worldTileData[currentCell.x, currentCell.y].playerVisible) {
            spriteRenderer.enabled = false;
        } else {
            spriteRenderer.enabled = true;
        }

        if (GameManager.instance.loseState) {
            spriteRenderer.enabled = true;
        }
    }
}
