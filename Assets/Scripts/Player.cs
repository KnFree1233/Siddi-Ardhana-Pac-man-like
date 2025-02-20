using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] Transform mainCamera;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        HideLockCursor();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void HideLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Movement()
    {
        float horizontalValue = Input.GetAxis("Horizontal");
        float verticalValue = Input.GetAxis("Vertical");

        Vector3 horizontalDirection = horizontalValue * mainCamera.right;
        Vector3 verticalDirection = verticalValue * mainCamera.forward;

        horizontalDirection.y = verticalDirection.y = 0;

        Vector3 newMovement = horizontalDirection + verticalDirection;

        rb.velocity = newMovement * speed * Time.fixedDeltaTime;
    }
}
