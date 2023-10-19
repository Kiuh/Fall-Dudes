using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform playerModel;

    [SerializeField]
    private float rotationSpeed;
    private bool lockModelRotation = false;
    public bool LockModelRotation
    {
        get => lockModelRotation;
        set => lockModelRotation = value;
    }

    private void Start()
    {
        LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        // rotate orientation
        Vector3 viewDir =
            player.position
            - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir =
            (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        if (inputDir != Vector3.zero && !lockModelRotation)
        {
            playerModel.forward = Vector3.Slerp(
                playerModel.forward,
                inputDir.normalized,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}
