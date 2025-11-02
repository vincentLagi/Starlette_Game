using System;
using UnityEngine;
using UnityEngine.Video;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 1;
    private Rigidbody2D characterBody;
    private Vector2 velocity;
    private Vector2 inputMovement;
    private Animator animator;
    private Vector2 lastDirection;
    private bool isMoving;
    public float interactRange = 1f;
    public InventoryManager inventoryManager;

    public bool canMove = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        velocity = new Vector2(speed, speed);
        characterBody = GetComponent<Rigidbody2D>();
        lastDirection = Vector2.down;
        isMoving = false;
    }

    public void HandleUpdate()
    {
        if (!canMove)
        {
            inputMovement = Vector2.zero;
            animator.SetBool("isMoving", false);
            return;
        }

        inputMovement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        isMoving = inputMovement != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            lastDirection = inputMovement.normalized;
            animator.SetFloat("x", inputMovement.x);
            animator.SetFloat("y", inputMovement.y);
        }

        animator.SetFloat("lastX", lastDirection.x);
        animator.SetFloat("lastY", lastDirection.y);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    public void StartMove()
    {
        canMove = true;
    }
    public void StopMove()
    {
        canMove = false;
        inputMovement = Vector2.zero;
        characterBody.linearVelocity = Vector2.zero;
        animator.SetBool("isMoving", false);
    }
    void Interact()
    {
        Vector2 interactPosition = (Vector2)transform.position + lastDirection * interactRange;
        Debug.DrawLine(transform.position, interactPosition, Color.red, 1f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastDirection, interactRange, LayerMask.GetMask("Interactable"));
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Interactable>().Interact();
        }
        else
        {
            Item item = inventoryManager.getSelectedItem();

            item.Use();
        }
    }

    private void FixedUpdate()
    {
        Vector2 delta = inputMovement * velocity * Time.deltaTime;
        characterBody.MovePosition(characterBody.position + delta);
    }

    public void SetFacingDirection(Vector2 faceDirection)
    {
        animator.SetFloat("x", faceDirection.x);
        animator.SetFloat("y", faceDirection.y);
    }
}