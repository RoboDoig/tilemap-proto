using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class InteractionTest : MonoBehaviour
{
    private GameTiles gameTiles;
    private Vector3Int lastSelection;
    private List<Vector3Int> cellCollection = new List<Vector3Int>();
    public CharacterControl characterControl;

    void Start() {
        gameTiles = GameTiles.instance;
    }

    // Update for pathfinding
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            // Get Location
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Get tile at that point
            Vector3Int selectedCell = gameTiles.tilemapFloor.WorldToCell(point);
            selectedCell.z = 0;
            Debug.Log(selectedCell);
            lastSelection = selectedCell;

            characterControl.SetPath(selectedCell);
        }
    }
	
	// Update for vision cone
	// private void Update () {
	// 	if (Input.GetMouseButtonDown(0))
	// 	{
    //         // reset last cells
    //         if (cellCollection.Count > 0) {
    //             foreach (Vector3Int cell in cellCollection) {
    //                 gameTiles.tilemap.SetTileFlags(cell, TileFlags.None);
    //                 gameTiles.tilemap.SetColor(cell, Color.white);
    //             }
    //         }

    //         // if (lastSelection != null) {
    //         //     gameTiles.tilemap.SetTileFlags(lastSelection, TileFlags.None);
    //         //     gameTiles.tilemap.SetColor(lastSelection, Color.white);
    //         // }

    //         // Get location
	// 		Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //         // Get tile at that point
    //         Vector3Int selectedCell = gameTiles.tilemap.WorldToCell(point);
    //         selectedCell.z = 0;
    //         lastSelection = selectedCell;

    //         cellCollection = new List<Vector3Int>();

    //         // Get circular perimeter around the point
    //         List<Vector3Int> perimeterCells = gameTiles.GetBresenhamCircleCells(selectedCell, 100);

    //         // for each point on the perimeter
    //         foreach (Vector3Int perimeterCell in perimeterCells) {

    //             // get line from cell center to perimeter
    //             List<Vector3Int> perimeterLine = gameTiles.GetBresenhamLineCells(new Vector2Int(selectedCell.x, selectedCell.y), new Vector2Int(perimeterCell.x, perimeterCell.y));
    //             // order line by distance from selected cell
    //             perimeterLine = perimeterLine.OrderBy(x => Vector3Int.Distance(selectedCell, x)).ToList();

    //             bool shouldBlock = false;

    //             // for each point in the line, colour the associate cells until vision blocked
    //             Color color = Color.red;
    //             foreach(Vector3Int lineCell in perimeterLine) {
    //                 DataTile tile = (DataTile)gameTiles.tilemap.GetTile(lineCell);
    //                 if (tile != null) {
    //                     cellCollection.Add(lineCell);
    //                     if (shouldBlock) {
    //                         color = Color.black;
    //                     }

    //                     if (tile.blocksVision) {
    //                         shouldBlock = true;
    //                     }

    //                     gameTiles.tilemap.SetTileFlags(lineCell, TileFlags.None);
    //                     gameTiles.tilemap.SetColor(lineCell, color);
    //                 }

    //             }
    //         }
	// 	}
	// }
}
