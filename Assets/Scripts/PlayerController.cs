using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove;

    [Header("Moving")]
    public float walkingSpeed;
	public float runningSpeed;
	public float jumpForce;
    public float gravity;

    [Header("Rotation")]
    public Camera mainCamera;
    public float lookSpeed;
    public float lookXLimit;

    public Animator HandAnimator;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX;

	private void Start()
	{
		characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

	public void Update()
	{
		Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedZ = 0f;
        float curSpeedX = 0f;

        if (canMove)
        {
            curSpeedZ = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
            curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");
        }

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedZ) + (right * curSpeedX);

        if(Input.GetKey(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            mainCamera.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        HandAnimator.SetFloat("Velocity", characterController.velocity.magnitude);
	}
}
