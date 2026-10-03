using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float turnSpeed = 150f;
    [SerializeField] float jumpForce = 6f;

    Rigidbody rb;
    bool isGrounded, jumpQueued;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            jumpQueued = true;
    }

    void FixedUpdate()
    {
        // Rotation via A and D keys
        float turnInput = 0f;
        if (Input.GetKey(KeyCode.D)) turnInput += 1f;
        if (Input.GetKey(KeyCode.A)) turnInput -= 1f;

        float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));

        // Strafe Left/Right via Arrow Keys only
        float strafeInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow)) strafeInput += 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) strafeInput -= 1f;

        // Forward/Backward input (Up/Down arrows or W/S)
        float forwardInput = Input.GetAxis("Vertical");

        // Combine relative forward and right movement vectors
        Vector3 moveDirection = (transform.forward * forwardInput) + (transform.right * strafeInput);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f) * speed;

        rb.linearVelocity = new Vector3(
            moveDirection.x, rb.linearVelocity.y, moveDirection.z);

        if (jumpQueued)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpQueued = false;
        }
    }

    void OnCollisionStay(Collision c)
    {
        if (c.GetContact(0).normal.y > 0.5f)
            isGrounded = true;
    }

    void OnCollisionExit(Collision c) => isGrounded = false;
}