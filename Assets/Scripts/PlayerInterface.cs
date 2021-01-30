using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{
    public float cameraMoveSpeed = 5f;

    private GameManager gameManager;
    private GameTiles gameTiles;

    // Start is called before the first frame update
    void Start()
    {
        gameTiles = GameTiles.instance;
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            // Get Location
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Get tile at that point
            Vector3Int selectedCell = gameTiles.tilemapFloor.WorldToCell(point);
            selectedCell.z = 0;

            gameManager.currentCharacter.SetPath(selectedCell);

            // Advance to next characters turn
            gameManager.AdvanceTurn();
        }

        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized * Time.deltaTime * cameraMoveSpeed;
    }
}
