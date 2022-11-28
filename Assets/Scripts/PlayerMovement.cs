using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _runSpeed = 10f;
    [SerializeField] float _jumpSpeed = 5f;
    [SerializeField] float _climbSpeed = 10f;
    
    Vector2 _moveInput;
    Rigidbody2D _rb;
    Animator _anim;
    CapsuleCollider2D _bodyCollider;
    BoxCollider2D _feetCollider;

    float _gravityScaleAtStart;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _feetCollider = GetComponent<BoxCollider2D>();
        _gravityScaleAtStart = _rb.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        isJumping();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if(value.isPressed)
        {
            _rb.velocity += new Vector2(0f, _jumpSpeed);
        }
    }

    void isJumping()
    {
        if(!_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _anim.SetBool("isJumping", true);
        }
        else
        {
            _anim.SetBool("isJumping", false);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (_moveInput.x * _runSpeed, _rb.velocity.y);
        _rb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon;
        _anim.SetBool("isRunning", playerHasHorizontalSpeed);

    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(_rb.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        
        if(!_feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            _rb.gravityScale = _gravityScaleAtStart;
            _anim.SetBool("isClimbing", true);
            return;
        }

        Vector2 climbVelocity = new Vector2 (_rb.velocity.x, _moveInput.y * _climbSpeed);
        _rb.velocity = climbVelocity;
        _rb.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(_rb.velocity.y) > Mathf.Epsilon;
        _anim.SetBool("isClimbing",playerHasVerticalSpeed);
    }
}

