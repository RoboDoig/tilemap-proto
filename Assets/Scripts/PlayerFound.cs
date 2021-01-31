using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFound : MonoBehaviour
{
    private List<Vector3Int> cellCollection = new List<Vector3Int>();
    private GameTiles gameTiles;
    private CharacterControl characterControl;

    // Start is called before the first frame update
    void Start()
    {
        gameTiles = GameTiles.instance;
        characterControl = GetComponent<CharacterControl>();
    }

	private void Update () {
        if (GameManager.instance.loseState) {
            return;
        }

        // reset last cells
        if (cellCollection.Count > 0) {
            foreach (Vector3Int cell in cellCollection) {
                gameTiles.tilemapFloor.SetTileFlags(cell, TileFlags.None);
                gameTiles.tilemapFloor.SetColor(cell, Color.white);
            }
        }

        // Get Location
        Vector3Int selectedCell = characterControl.GetCurrentCell();

        cellCollection = new List<Vector3Int>();

        // Get circular perimeter around the point
        List<Vector3Int> perimeterCells = gameTiles.GetBresenhamCircleCells(selectedCell, 100);

        // for each point on the perimeter
        foreach (Vector3Int perimeterCell in perimeterCells) {

            // get line from cell center to perimeter
            List<Vector3Int> perimeterLine = gameTiles.GetBresenhamLineCells(new Vector2Int(selectedCell.x, selectedCell.y), new Vector2Int(perimeterCell.x, perimeterCell.y));
            // order line by distance from selected cell
            perimeterLine = perimeterLine.OrderBy(x => Vector3Int.Distance(selectedCell, x)).ToList();

            bool shouldBlock = false;

            // for each point in the line, colour the associate cells until vision blocked
            Color color = Color.white;
            bool playerVisible = true;
            foreach(Vector3Int lineCell in perimeterLine) {
                DataTile tile = (DataTile)gameTiles.tilemapFloor.GetTile(lineCell);
                DataTile tileObstacle = (DataTile)gameTiles.tilemapObstacles.GetTile(lineCell);
                if (tile != null) {
                    cellCollection.Add(lineCell);
                    if (shouldBlock) {
                        color = Color.black;
                        playerVisible = false;
                    }

                    if (gameTiles.worldTileData[lineCell.x, lineCell.y].blocksVision) {
                        shouldBlock = true;
                    }

                    gameTiles.tilemapFloor.SetTileFlags(lineCell, TileFlags.None);
                    gameTiles.tilemapFloor.SetColor(lineCell, color);
                    gameTiles.tilemapObstacles.SetTileFlags(lineCell, TileFlags.None);
                    gameTiles.tilemapObstacles.SetColor(lineCell, color);

                    // We also need to tell the tile data its hidden from view and implement any vision interactions
                    gameTiles.worldTileData[lineCell.x, lineCell.y].playerVisible = playerVisible;
                    if (tileObstacle != null) {
                        if (GameManager.instance.currentCharacter.characterName == "found" || GameManager.instance.currentCharacter.characterName == "guard") {
                            tileObstacle.VisionInteract(lineCell, true);
                        } else {
                            tileObstacle.VisionInteract(lineCell, playerVisible);
                        }
                        
                    }
                }
            }
        }
	}
}
