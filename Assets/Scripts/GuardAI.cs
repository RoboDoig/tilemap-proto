using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour
{
    public List<Vector3Int> patrolPositions;
    public SightArea sightArea;
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

    public void CheckVision() {
        if (sightArea.seenCharacters.Count > 0 && !incapacitated) {
            GameManager.instance.LoseState();
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
        // Display some alert symbol
    }
}
