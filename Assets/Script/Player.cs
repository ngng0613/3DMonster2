using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidBody;
    [SerializeField] Animator _animator;
    [SerializeField] float _moveSpeed = 3.0f;
    [SerializeField] bool _walkEffect = default;
    [SerializeField] FloorEffectGenerator _effectGenerator;

    public static bool CanMove = true;

    private void Start()
    {
        this._rigidBody = this.GetComponent<Rigidbody>();
        this._animator = GetComponent<Animator>();
        CanMove = true;
    }

    void Update()
    {
        if (CanMove == true)
        {
            Move();
        }
        else
        {
            _rigidBody.velocity = Vector3.zero;
            _animator.SetInteger("ActionState", 0);
        }
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 tempVector = new Vector3(horizontal, 0, vertical);

        if (tempVector.sqrMagnitude < 0.3f)
        {
            _animator.SetInteger("ActionState", 0);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(tempVector, transform.up);
            _rigidBody.velocity = tempVector * _moveSpeed;
            _animator.SetInteger("ActionState", 1);
            if (_walkEffect == true)
            {
                _effectGenerator.OnUpdate(this.transform);
            }

        }

    }
}
