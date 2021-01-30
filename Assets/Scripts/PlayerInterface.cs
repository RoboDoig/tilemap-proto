using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{
    public float cameraMoveSpeed = 5f;
    public bool inControl = true;

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
        if (Input.GetMouseButtonDown(0)) {
            // Get Location
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
