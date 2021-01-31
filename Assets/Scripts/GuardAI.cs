using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour
{
    public List<Vector3Int> patrolPositions;
    public SightArea sightArea;
    public HearingArea hearingArea;
    public SpriteRenderer alarmAnimation;
    public GameObject pathIndicatorPrefab;
    public List<GameObject> currentPathIndicators = new List<GameObject>();
    public Vector3Int nextDestination;
    private CharacterControl characterControl;
    private int patrolPositionIndex;
    public bool incapacitated = false;
    private int incapacitateTurns = 3;

    void Start() {
        characterControl = GetComponent<CharacterControl>();

        for (int x = 0; x < patrolPositions.Count; x++) {
            patrolPositions[x] += characterControl.GetCurrentCell();
        }

        patrolPositions.Add(characterControl.GetCurrentCell());
        patrolPositionIndex = 0;

        nextDestination = patrolPositions[patrolPositionIndex];
        StartCoroutine(ShowDestination());
    }

    void Update() {
        CheckVision();
    }

    public void PlanNextMove() {
        patrolPositionIndex++;
        if (patrolPositionIndex > patrolPositions.Count-1) {
            patrolPositionIndex = 0;
        }

        nextDestination = patrolPositions[patrolPositionIndex];
    }

    IEnumerator ShowDestination() {
        foreach(GameObject obj in currentPathIndicators) {
            Destroy(obj);
        }

        Pathfinder pathfinder = GetComponent<Pathfinder>();
        List<Pathfinder.Node> path = pathfinder.FindPath(characterControl.GetCurrentCell(), nextDestination);
        foreach(Pathfinder.Node node in path) {
            if (GameTiles.instance.worldTileData[node.position.x, node.position.y].playerVisible) {
                Vector3 placePosition = GameTiles.instance.tilemapFloor.CellToWorld(node.position);
                GameObject obj = Instantiate(pathIndicatorPrefab, placePosition, Quaternion.identity);
                currentPathIndicators.Add(obj);
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShowDestination());
    }

    public void CheckVision() {
        if (sightArea.seenCharacters.Count > 0 && !incapacitated) {
            GameManager.instance.LoseState();
            StartCoroutine(ShowAlert());
        }
    }

    public void Incapacitate() {
        incapacitated = true;
        GetComponent<Animator>().SetTrigger("incapacitate"); // TODO - Yes I know this sucks
    }

    public void Recover() {
        incapacitateTurns -= 1;
        if (incapacitateTurns == 0) {
            incapacitated = false;
            incapacitateTurns = 2;
            GetComponent<Animator>().SetTrigger("recover"); // TODO - Yes I know this sucks
        }
    }

    public void Alert(Vector3Int suspectCell) {
        // Display alert symbol
        if (!incapacitated) {
            StartCoroutine(ShowAlert());
            nextDestination = suspectCell;
        }
    }

    IEnumerator ShowAlert() {
        alarmAnimation.enabled = true;
        yield return new WaitForSeconds(2f);
        alarmAnimation.enabled = false;
    }
}
