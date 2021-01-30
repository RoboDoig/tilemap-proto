using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float moveSpeed = 5f;

    Pathfinder pathfinder;
    public List<Pathfinder.Node> path;
    Vector3Int pathDestinationCell;
    Vector3Int destinationCell;
    public delegate void UpdateAction();
    public UpdateAction updateAction;

    void Start() {
        pathfinder = GetComponent<Pathfinder>();
        updateAction = Idle;
    }

    void Update() {
        updateAction();
    }

    public void SetPath(Vector3Int destinationCell)
    {
        FindPath(destinationCell);
        updateAction = GoToDestination;
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
