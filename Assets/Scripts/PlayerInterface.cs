using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInterface : MonoBehaviour
{
    public float cameraMoveSpeed = 5f;
    public bool inControl = true;
    public GameObject mouseIndicator;

    private GameManager gameManager;
    private GameTiles gameTiles;

    public delegate void UpdateAction();
    public UpdateAction updateAction;

    // Start is called before the first frame update
    void Start()
    {
        gameTiles = GameTiles.instance;
        gameManager = GetComponent<GameManager>();
        updateAction = InControlUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        updateAction();
    }

    public void InControlUpdate() {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int hoverCell = gameTiles.tilemapFloor.WorldToCell(point);
        Vector3 hoverCellPosition = gameTiles.tilemapFloor.CellToWorld(hoverCell);
        hoverCellPosition.z = 0;
        mouseIndicator.transform.position = hoverCellPosition;

        if (Input.GetKeyDown(KeyCode.Space)) {
            gameManager.AdvanceTurn();
        }

        if (Input.GetMouseButtonDown(0)) {

            // Get tile at that point
            Vector3Int selectedCell = gameTiles.tilemapFloor.WorldToCell(point);
            selectedCell.z = 0;

            // Check that move is allowed
            float requestedMoveDistance = (gameManager.currentCharacter.GetCurrentCell() - selectedCell).magnitude;
            if (requestedMoveDistance > gameManager.currentCharacter.maxMoveDistance) {
                return;
            }

            gameManager.currentCharacter.SetPath(selectedCell);

            // Advance to next characters turn
            gameManager.AdvanceTurn();
        }

        // Camera control
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized * Time.deltaTime * cameraMoveSpeed;
    }

    public void OutControlUpdate() {
        // Camera control
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized * Time.deltaTime * cameraMoveSpeed;
    }
}
