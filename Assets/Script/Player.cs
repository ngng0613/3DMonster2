using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] bool walkEffect = default;
    [SerializeField] FloorEffectGenerator effectGenerator;

    public static bool canMove = true;

    private void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody>();
        this.animator = GetComponent<Animator>();
        canMove = true;
    }

    void Update()
    {
        if (canMove == true)
        {
            Move();
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
            animator.SetInteger("ActionState", 0);
        }
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 tempVector = new Vector3(horizontal, 0, vertical);

        if (tempVector.sqrMagnitude < 0.3f)
        {
            animator.SetInteger("ActionState", 0);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(tempVector, transform.up);
            rigidBody.velocity = tempVector * moveSpeed;
            animator.SetInteger("ActionState", 1);
            if (walkEffect == true)
            {
                effectGenerator.OnUpdate(this.transform);
            }

        }

    }
}
