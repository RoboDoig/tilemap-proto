using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTiles : MonoBehaviour
{
    public static GameTiles instance;
    public Tilemap tilemapFloor;
    public Tilemap tilemapObstacles;
    public WorldTileData[,] worldTileData;
    public List<Vector3Int> goalTileLocations;

    private void Awake() 
	{
		if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		GetWorldTiles();
	}

    private void GetWorldTiles () 
	{
        BoundsInt bounds = tilemapFloor.cellBounds;
        worldTileData = new WorldTileData[bounds.size.x, bounds.size.y];
        
        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                DataTile tileFloor = (DataTile)tilemapFloor.GetTile(new Vector3Int(x, y, 0));
                DataTile tileObstacle = (DataTile)tilemapObstacles.GetTile(new Vector3Int(x, y, 0));
                if (tileFloor == null) {
                    worldTileData[x, y] = new WorldTileData(false, false, new Vector3Int(x, y, 0));
                } else {
                    if (tileObstacle != null) {
                        worldTileData[x, y] = new WorldTileData(tileObstacle.traversable, tileObstacle.blocksVision, new Vector3Int(x, y, 0));
                        
                        if (tileObstacle.isWinLocation) {
                            worldTileData[x, y].isWinLocation = true;
                            goalTileLocations.Add(worldTileData[x, y].position);
                        }
                    } else {
                        worldTileData[x, y] = new WorldTileData(tileFloor.traversable, tileFloor.blocksVision, new Vector3Int(x, y, 0));
                    }
                }
            }
        }
	}

    // Function for circle-generation 
    // using Bresenham's algorithm 
    public List<Vector3Int> GetBresenhamCircleCells(Vector3Int origin, int radius) {
        List<Vector3Int> coords = new List<Vector3Int>();
        int x = 0;
        int y = radius;
        int d = 3 - 2 * radius;
        coords.Add(new Vector3Int(origin.x+x,origin.y+y,0));
        coords.Add(new Vector3Int(origin.x-x,origin.y+y,0));
        coords.Add(new Vector3Int(origin.x+x,origin.y-y,0));
        coords.Add(new Vector3Int(origin.x-x,origin.y-y,0));
        coords.Add(new Vector3Int(origin.x+y,origin.y+x,0));
        coords.Add(new Vector3Int(origin.x-y,origin.y+x,0));
        coords.Add(new Vector3Int(origin.x+y,origin.y-x,0));
        coords.Add(new Vector3Int(origin.x-y,origin.y-x,0));
        while (y >= x) {
            x++;

            if (d > 0) {
                y--;
                d = d + 4 * (x - y) + 10; 
            } else {
                d = d + 4 * x + 6;
            }

            coords.Add(new Vector3Int(origin.x+x,origin.y+y,0));
            coords.Add(new Vector3Int(origin.x-x,origin.y+y,0));
            coords.Add(new Vector3Int(origin.x+x,origin.y-y,0));
            coords.Add(new Vector3Int(origin.x-x,origin.y-y,0));
            coords.Add(new Vector3Int(origin.x+y,origin.y+x,0));
            coords.Add(new Vector3Int(origin.x-y,origin.y+x,0));
            coords.Add(new Vector3Int(origin.x+y,origin.y-x,0));
            coords.Add(new Vector3Int(origin.x-y,origin.y-x,0));
        }

        return coords;
    }

        // Swap the values of A and B
    private void Swap<T>(ref T a, ref T b) {
        T c = a;
        a = b;
        b = c;
    }

    // Returns the list of points from p0 to p1 
    public List<Vector3Int> GetBresenhamLineCells(Vector2Int p0, Vector2Int p1) {
        return BresenhamLine(p0.x, p0.y, p1.x, p1.y);
    }

    // Returns the list of points from (x0, y0) to (x1, y1)
    private List<Vector3Int> BresenhamLine(int x0, int y0, int x1, int y1) {
        // Optimization: it would be preferable to calculate in
        // advance the size of "result" and to use a fixed-size array
        // instead of a list.
        List<Vector3Int> result = new List<Vector3Int>();

        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep) {
            Swap(ref x0, ref y0);
            Swap(ref x1, ref y1);
        }
        if (x0 > x1) {
            Swap(ref x0, ref x1);
            Swap(ref y0, ref y1);
        }

        int deltax = x1 - x0;
        int deltay = Math.Abs(y1 - y0);
        int error = 0;
        int ystep;
        int y = y0;
        if (y0 < y1) ystep = 1; else ystep = -1;
        for (int x = x0; x <= x1; x++) {
            if (steep) result.Add(new Vector3Int(y, x, 0));
            else result.Add(new Vector3Int(x, y, 0));
            error += deltay;
            if (2 * error >= deltax) {
                y += ystep;
                error -= deltax;
            }
        }

        return result;
    }
}
