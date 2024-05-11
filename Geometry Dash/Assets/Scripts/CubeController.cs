using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CubeController : MonoBehaviour
{
    [SerializeField] private float cubeSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private float rotationSpeed = 5f;
    private Rigidbody2D cubeRB;
    private float targetAngle = 0f;
    private bool isGround = false;
    private bool isMovement = true;
    public UnityEvent OnJumping;
    public UnityEvent OnUpheaval;
    public UnityEvent OnExit;

    private void Awake()
    {
        cubeRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isMovement == true)
        {
            cubeRB.velocity = new Vector2(cubeSpeed, cubeRB.velocity.y);

            if (isGround)
            {
                if (Input.GetButton("Jump") || Input.touchCount != 0 || Input.GetKey(KeyCode.Mouse0))
                {
                    Jump();
                }
            }
            float rotation = Mathf.LerpAngle(cubeRB.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
 
            cubeRB.MoveRotation(rotation);
        }
    }

    private void Jump()
    {
        cubeRB.velocity = new Vector2(cubeRB.velocity.x, jumpForce);
        isGround = false;
        
        if (cubeRB.gravityScale > 0)
        {
            targetAngle -= 90f;
        }
        else
        {
            targetAngle += 90f;
        }
        
        OnJumping.Invoke();
    }
    
    private void Upheaval()
    {
        cubeRB.gravityScale *= -1;
        jumpForce *= -1;
        
        OnUpheaval.Invoke();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGround = true; 
        
        if (collision.gameObject.CompareTag("Danger"))
        {
            SceneController.RestartScene();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DoubleJump"))
        {
            isGround = true;
        }
        
        if (collision.CompareTag("BlackHole"))
        {
            isMovement = false;
        }

        if (collision.CompareTag("Finish"))
        {
            SceneController.LoadNextScene();
            OnExit.Invoke();
        }
        
        if (collision.CompareTag("Upheavel"))
        {
            Upheaval();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
    }

}
