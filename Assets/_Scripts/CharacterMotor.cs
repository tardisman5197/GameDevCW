using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMotor : MonoBehaviour 
{
	public bool IsGrounded { get {return characterController.isGrounded;}}

    // Movement variables
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

    // Animation IDs
    private int animiatorWalkingSpeedFowardID;
    private int animiatorWalkingSpeedLeftID;
    private int animiatorStrikeID;
    private int animiatorJumpID;
    private int animiatorResetID;

    // Variables for storing actions
    public Vector3 startPosition;
    public Quaternion startRotation;
    public bool dead;
    public int currentTick;
    public enum Type { Move, Strike, Jump };
    public class Action
    {
        public Type actionType;
        public Vector3 position;
        public Quaternion rotation;
        public float speed_v;
        public float speed_h;

        public Action(Type type, Vector3 position, Quaternion rotation, float v, float h)
        {
            this.actionType = type;
            this.position = position;
            this.rotation = rotation;
            this.speed_v = v;
            this.speed_h = h;
        }
    }
    private Dictionary<int, Action> actions;


    void Awake()
	{
		characterController = GetComponent<CharacterController>();

        animiatorWalkingSpeedFowardID = Animator.StringToHash("walkingSpeedFoward");
        animiatorWalkingSpeedLeftID = Animator.StringToHash("walkingSpeedLeft");
        animiatorStrikeID = Animator.StringToHash("strike");
        animiatorJumpID = Animator.StringToHash("jump");
        animiatorResetID = Animator.StringToHash("reset");

        actions = new Dictionary<int, Action>();
        dead = false;
        currentTick = 0;
    }

    void Update()
    {
        currentTick++;

        // If alive and not paused the player should control the movement else replay movement
        if (!dead && !GameObject.Find("Main Camera").GetComponent<PauseController>().pauseMenu.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Strike();
            }
            else
            {

                if (Input.GetButton("Jump"))
                {
                    Jump();
                }
                Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), Input.GetAxis("Mouse X"));
            }
        }
        else
        {
            if (actions.ContainsKey(currentTick))
            {
                Action currentAction = actions[currentTick];
                switch (currentAction.actionType)
                {
                    case Type.Strike:
                        Strike();
                        break;
                    case Type.Move:
                        this.gameObject.transform.SetPositionAndRotation(currentAction.position, currentAction.rotation);
                        this.GetComponent<Animator>().SetFloat(animiatorWalkingSpeedFowardID, currentAction.speed_v);
                        this.GetComponent<Animator>().SetFloat(animiatorWalkingSpeedLeftID, currentAction.speed_h);
                        break;
                    case Type.Jump:
                        Jump();
                        break;
                }
            }
        }
    }

    // Jump playes the jump animation
    void Jump()
    {
        // Check if not already jumping
        if (!(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jumping") ||
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jump")))
        {
            // Start jump animation
            this.GetComponent<Animator>().SetTrigger(animiatorJumpID);

            // Check if the chracter is not a previous life
            if (!dead)
            {
                // Add the action to the list
                if (!actions.ContainsKey(currentTick))
                {
                    actions.Add(currentTick, new Action(Type.Jump, this.gameObject.transform.position, this.gameObject.transform.localRotation, 0, 0));
                }
            }
        }
    }

    // Strike plays the attack animation
    void Strike()
    {
        // Play the attack animation
        this.GetComponent<Animator>().SetTrigger(animiatorStrikeID);

        // Check if not a previous player life
        if (!dead)
        {
            // Add action to actions
            if (!actions.ContainsKey(currentTick))
            {
                actions.Add(currentTick, new Action(Type.Strike, this.gameObject.transform.position, this.gameObject.transform.localRotation, 0, 0));
            }
        }
    }

    // Move updates the players postion
    void Move(float input_v, float input_h, float input_x)
    {
        // if we are on the ground then allow movement
        if (IsGrounded)
        {

            bool isMoving_v = (input_v != 0);
            bool isMoving_h = (input_h != 0);

            moveDirection.x = transform.forward.x;
            moveDirection.z = transform.right.z;

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
                
        }
        else
        {
            moveDirection.y -= (gravity * Time.deltaTime);
        }

        transform.Rotate(new Vector3(0, input_x, 0) * Time.deltaTime * rotationSpeed);

        // Play the animation of walking
        this.GetComponent<Animator>().SetFloat(animiatorWalkingSpeedFowardID, currentSpeed_v);
        this.GetComponent<Animator>().SetFloat(animiatorWalkingSpeedLeftID, currentSpeed_h);

        // Add the action to the list
        if (!actions.ContainsKey(currentTick))
        {
            actions.Add(currentTick, new Action(Type.Move, this.gameObject.transform.position, this.gameObject.transform.localRotation, currentSpeed_v, currentSpeed_h));
        }
    }

    // Reset is called if the character is a replay of a past life
    public void Reset(Dictionary<int, Action> actions)
    {
        dead = true;
        this.actions = actions;
        this.currentTick = 0;

        this.GetComponent<Animator>().SetFloat(animiatorWalkingSpeedFowardID, 0);
        this.GetComponent<Animator>().SetFloat(animiatorWalkingSpeedLeftID, 0);
        this.GetComponent<Animator>().SetTrigger(animiatorResetID);
    }

    // GetActions retuns the list of actions that the chracter has taken
    public Dictionary<int, Action> GetActions()
    {
        return actions;
    }
}
