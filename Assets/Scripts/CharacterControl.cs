using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterControl : MonoBehaviour
{
    public string characterName;
    public float moveSpeed = 5f;
    public float maxMoveDistance = 5f;
    public bool isPlayer;
    public int turnPriority;

    public static List<CharacterControl> activeCharacters = new List<CharacterControl>();
    Pathfinder pathfinder;
    public List<Pathfinder.Node> path;
    private SpriteRenderer spriteRenderer;
    Vector3Int pathDestinationCell;
    Vector3Int destinationCell;
    Vector3Int interactionCell;
    public delegate void UpdateAction();
    public UpdateAction updateAction;

    void Awake() {
        activeCharacters.Add(this);

        activeCharacters = activeCharacters.OrderBy(x => x.turnPriority).ToList();
    }

    void Start() {
        pathfinder = GetComponent<Pathfinder>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        updateAction = Idle;
    }

    void Update() {
        updateAction();

        // Standard update checks
        Vector3Int currentCell = GetCurrentCell();
        if (!GameTiles.instance.worldTileData[currentCell.x, currentCell.y].playerVisible) {
            spriteRenderer.enabled = false;
        } else {
            spriteRenderer.enabled = true;
        }
    }

    public void SetPath(Vector3Int destinationCell)
    {
        interactionCell = destinationCell;

        // if the destination cell is non-traversable, find a surrounding cell that is
        if (!GameTiles.instance.worldTileData[destinationCell.x, destinationCell.y].traversable) {
            List<WorldTileData> traversablePerimeterTiles = new List<WorldTileData>();
            for (int x = -1; x < 2; x++) {
                for (int y = -1; y < 2; y++) {
                    if (GameTiles.instance.worldTileData[destinationCell.x+x, destinationCell.y+y].traversable) {
                        traversablePerimeterTiles.Add(GameTiles.instance.worldTileData[destinationCell.x+x, destinationCell.y+y]);
                    }
                }
            }
            traversablePerimeterTiles = traversablePerimeterTiles.OrderBy(x => Vector3Int.Distance(GetCurrentCell(), x.position)).ToList();
            if (traversablePerimeterTiles.Count > 0) {destinationCell = traversablePerimeterTiles[0].position;}
        }

        FindPath(destinationCell);
        updateAction = GoToDestination;
    }

    public void SetRandomPath() {
        List<Vector3Int> perimeterCells = GameTiles.instance.GetBresenhamCircleCells(GetCurrentCell(), 10);
        System.Random random = new System.Random();
        int index = random.Next(perimeterCells.Count);
        SetPath(perimeterCells[index]);
    }

    void GoToDestination()
    {
        if (path != null && path.Count > 0)
        {
            if (MoveOnPath())
            {
                updateAction = Idle;
            }
        }
        else
        {
            updateAction = Idle;
        }
    }

    void Idle() {

    }

    bool FindPath(Vector3Int _destinationCell)
    {
        path = pathfinder.FindPath(GetCurrentCell(), _destinationCell);

        if (path != null)
        {
            destinationCell = _destinationCell;
            pathDestinationCell = GetCurrentCell();
            return true;
        } else
        {
            return false;
        }
    }

    bool MoveOnPath()
    {
        if (path.Count <= 0 || path == null)
        {
            path = null;
            return true;
        }

        if(MoveToCell(pathDestinationCell))
        {
            path.RemoveAt(0);
            pathDestinationCell = path[0].position;
        }

        return false;
    }

    bool MoveToCell(Vector3Int destinationCell) {
        Vector3 worldDestination = GameTiles.instance.tilemapFloor.CellToWorld(destinationCell);

        // interact with the final destination cell tile if we are there
        if (destinationCell == path[path.Count-1].position) {
            DataTile tileFloor = (DataTile)GameTiles.instance.tilemapFloor.GetTile(interactionCell);
            if (tileFloor != null) {tileFloor.Interact(interactionCell, this);}

            DataTile tileObstacle = (DataTile)GameTiles.instance.tilemapObstacles.GetTile(interactionCell);
            if (tileObstacle != null) {tileObstacle.Interact(interactionCell, this);}
        }

        if((worldDestination - transform.position).magnitude > 0.1f)
        {
            Vector3 moveVector = (worldDestination - transform.position).normalized;
            transform.position += moveVector * Time.deltaTime * moveSpeed;

            return false;
        } else
        {
            return true;
        }
    }

    public Vector3Int GetCurrentCell()
    {
        return GameTiles.instance.tilemapFloor.WorldToCell(transform.position);
    }
}
