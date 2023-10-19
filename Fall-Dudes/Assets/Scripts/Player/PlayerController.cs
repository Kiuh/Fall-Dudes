using Common;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private float allSpeedMultiplier;

    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private Rigidbody rigidBody;

    [SerializeField]
    private Animator animator;

    [Header("Movement")]
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float airMinSpeed;

    [SerializeField]
    private float speedIncreaseMultiplier;

    // Local
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [Header("Jumping")]
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float jumpCooldown;

    [SerializeField]
    private float airMultiplier;

    // Local
    private bool readyToJump = true;

    [Header("Key binds")]
    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField]
    private float groundCheckHeight;

    [SerializeField]
    [InspectorReadOnly]
    private bool grounded;

    // Local
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private bool lockInteraction;
    public bool LockInteraction
    {
        get => lockInteraction;
        set => lockInteraction = value;
    }

    public void ResetForces()
    {
        rigidBody.velocity = Vector3.zero;
    }

    public void PlayDeathAnimation()
    {
        animator.Play("Die");
    }

    public void PlayVictoryAnimation()
    {
        animator.Play("Victory");
    }

    public void ReturnToNormalAnimations()
    {
        animator.Play("Idle");
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckHeight);

        MyInput();
        SpeedControl();
        StateHandler();

        animator.SetFloat("Velocity", rigidBody.velocity.magnitude);
        animator.SetBool("IsGrounded", grounded);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if (lockInteraction)
        {
            horizontalInput = 0;
            verticalInput = 0;
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private bool keepMomentum;

    private void StateHandler()
    {
        // Mode - Walking
        if (grounded)
        {
            desiredMoveSpeed = walkSpeed;
        }
        else // Mode - Air
        {
            if (moveSpeed < airMinSpeed)
            {
                desiredMoveSpeed = airMinSpeed;
            }
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                _ = StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

        // deactivate keepMomentum
        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f)
        {
            keepMomentum = false;
        }
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * speedIncreaseMultiplier;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection =
            (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        if (grounded)
        {
            rigidBody.AddForce(
                allSpeedMultiplier * moveSpeed * moveDirection.normalized,
                ForceMode.Force
            );
        }
        else // in air
        {
            rigidBody.AddForce(
                allSpeedMultiplier * airMultiplier * moveSpeed * moveDirection.normalized,
                ForceMode.Force
            );
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        // limit velocity if needed
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVelocity.normalized * moveSpeed;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
