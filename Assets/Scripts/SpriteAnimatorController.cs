using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimatorController : MonoBehaviour
{
    Vector3 lastPosition;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        Vector3 moveDirection = (transform.position - lastPosition).normalized;

        lastPosition = transform.position;

        animator.SetFloat("moveSpeed", moveSpeed);
        animator.SetFloat("horizontalSpeed", moveDirection.x);
        animator.SetFloat("verticalSpeed", moveDirection.y);

        if (moveDirection.x > 0.1) {
            FlipRight();
        } else if (moveDirection.x < -0.1) {
            FlipLeft();
        }
    }

    void FlipLeft() {
        Vector3 scale = transform.localScale;
        scale.x = -1;
        transform.localScale = scale;
    }

    void FlipRight() {
        Vector3 scale = transform.localScale;
        scale.x = 1;
        transform.localScale = scale;
    }
}
