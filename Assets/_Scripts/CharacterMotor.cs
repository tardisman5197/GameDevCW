using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour 
{
	public bool IsGrounded { get {return characterController.isGrounded;}}

	public float speed = 10.0f;
	public float jumpSpeed = 10.0f;
	public float gravity = 20.0f;
	public float rotationSpeed = 100.0f;

	public float currentSpeed_v = 0.0f;
    public float currentSpeed_h = 0.0f;
    public float maxSpeed = 10.0f;
	public float acceleration = 10.0f;
	public float decceleration = 20.0f;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController characterController;

    private int animiatorWalkingSpeedFowardID;
    private int animiatorWalkingSpeedLeftID;
    private int animiatorStrikeID;
    private int animiatorJumpID;





    void Awake()
	{
		characterController = GetComponent<CharacterController>();
        animiatorWalkingSpeedFowardID = Animator.StringToHash("walkingSpeedFoward");
        animiatorWalkingSpeedLeftID = Animator.StringToHash("walkingSpeedLeft");
        animiatorStrikeID = Animator.StringToHash("strike");
        animiatorJumpID = Animator.StringToHash("jump");

    }

	void Update() 
	{

        if (Input.GetMouseButtonDown(0))
        {
            this.GetComponent<Animator>().SetTrigger(animiatorStrikeID);
        }
        else
        {


            // if we are on the ground then allow movement
            if (IsGrounded)
            {
                float input_v = Input.GetAxis("Vertical");
                float input_h = Input.GetAxis("Horizontal");

                bool isMoving_v = (input_v != 0);
                bool isMoving_h = (input_h != 0);

                moveDirection.x = transform.forward.x;
                moveDirection.z = -transform.right.z;

                if (isMoving_v)
                {
                    currentSpeed_v += (acceleration * input_v * Time.deltaTime);
                    currentSpeed_v = Mathf.Clamp(currentSpeed_v, -maxSpeed, maxSpeed);
                }
                else if (currentSpeed_v > 0)
                {
                    currentSpeed_v -= (decceleration * Time.deltaTime);
                    currentSpeed_v = Mathf.Clamp(currentSpeed_v, 0, maxSpeed);
                }
                else if (currentSpeed_v < 0)
                {
                    currentSpeed_v += (decceleration * Time.deltaTime);
                    currentSpeed_v = Mathf.Clamp(currentSpeed_v, -maxSpeed, 0);
                }

                if (isMoving_h)
                {
                    currentSpeed_h += (acceleration * input_h * Time.deltaTime);
                    currentSpeed_h = Mathf.Clamp(currentSpeed_h, -maxSpeed, maxSpeed);
                }
                else if (currentSpeed_h > 0)
                {
                    currentSpeed_h -= (decceleration * Time.deltaTime);
                    currentSpeed_h = Mathf.Clamp(currentSpeed_h, 0, maxSpeed);
                }
                else if (currentSpeed_h < 0)
                {
                    currentSpeed_h += (decceleration * Time.deltaTime);
                    currentSpeed_h = Mathf.Clamp(currentSpeed_h, -maxSpeed, 0);
                }

                moveDirection.x *= currentSpeed_v;
                moveDirection.z *= currentSpeed_h;

                moveDirection.y = Mathf.Max(0, moveDirection.y);


                if (Input.GetButton("Jump") && 
                    !(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jumping") ||
                    this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jump")))
                {
                    this.GetComponent<Animator>().SetTrigger(animiatorJumpID);
                    //if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                    //    StartCoroutine(DelayJump());
                    //}


                }
            }
            else
            {
                moveDirection.y -= (gravity * Time.deltaTime);
            }

            //float rotation = (Input.GetAxis("Horizontal") * rotationSpeed) * Time.deltaTime;

            //transform.Rotate(0, rotation, 0);

            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);

            characterController.Move(moveDirection * Time.deltaTime);

            this.GetComponent<Animator>().SetFloat(animiatorWalkingSpeedFowardID, currentSpeed_v);
            this.GetComponent<Animator>().SetFloat(animiatorWalkingSpeedLeftID, currentSpeed_h);
        }
    }

    IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(1);
        moveDirection.y = jumpSpeed;
    }


}
