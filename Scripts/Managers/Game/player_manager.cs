using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Base class for all Characters */
public class player_manager : MonoBehaviour
{
    [Tooltip("The list that holds the collectables names (tags)")]
    public List<string> collectableNames;

    [Tooltip("The refernce for inventory controllers")]
    public inventory_controller inventoryController;

    [Tooltip("The gravity opposite scale factor")]
    public float gravityScaleFactor;

    [Tooltip("The object that holds the virtual joystick prefab")]
    public Joystick joystick;

    [Tooltip("The climbing panel that contains controls for climbing")]
    public GameObject climbingPanel;

    [Tooltip("The Button that hanges the player on the wall")]
    public GameObject hangOnWallBtn;

    Rigidbody rb;

    [Tooltip("Animator component of the player")]
    Animator animator;

    [Tooltip("Value for the player speed")]
    public float speed;
    [Tooltip("Value for the speed that player turns around with")]
    public float rotationSpeed;
    [Tooltip("Value for the jump force for the player")]
    public float jumpForce;

    [Tooltip("The flag for climbing up")]
    public bool isClimbingUp;
    [Tooltip("The flag for climbing down")]
    public bool isClimbingDown;
    [Tooltip("The flag for hanging")]
    public bool isHanging;
    [Tooltip("Is the player lerping when climbing or not?")]
    bool isLerping;
    [Tooltip("The player in position for climbing or not")]
    bool inPosition;
    [Tooltip("The t pos of the player when climbing")]
    float t;
    [Tooltip("The position where the climbing has started")]
    Vector3 startPosition;
    [Tooltip("The target position for the climbing")]
    Vector3 targetPosition;
    [Tooltip("The start rotation of the player in the climbing")]
    Quaternion startRot;
    [Tooltip("The target rotation of the player when climbing")]
    Quaternion targetRot;
    [Tooltip("The position offset of the player when climbing")]
    public float positionOffset;
    [Tooltip("How far away will the player be from the wall")]
    public float offsetFromWall = 0.3f;
    [Tooltip("The speed that the player will be climbing up with")]
    public float speedMultiplier = 0.2f;

    [Tooltip("Helper transform for the climbing movement")]
    Transform helper;

    float delta;

    void ShowHangOnWallBtn()
    {
        if (hangOnWallBtn)
        {
            hangOnWallBtn.SetActive(true);
        }
    }
    void HideHangOnWallBtn()
    {
        if (hangOnWallBtn)
        {
            hangOnWallBtn.SetActive(false);
        }
    }
    void ShowClimbingPanel()
    {
        if (climbingPanel)
        {
            climbingPanel.SetActive(true);
        }
    }
    void HideClimbingPanel()
    {
        if (climbingPanel)
        {
            climbingPanel.SetActive(false);
        }
    }

    public void HangOnWallBtnCallback()
    {
        HideHangOnWallBtn();
        ShowClimbingPanel();
        isHanging = true;
        isClimbingDown = false;
        isClimbingUp = false;
    }
    public void DropFromWallBtnCallback()
    {
        HideClimbingPanel();
        isHanging = false;
        isClimbingDown = false;
        isClimbingUp = false;
    }
    public void ClimbUpWallBtnCallback()
    {
        isClimbingUp = true;
        isHanging = false;
        isClimbingDown = false;
    }
    public void ClimbDownWallBtnCallback()
    {
        isClimbingDown = true;
        isHanging = false;
        isClimbingUp = false;
    }

    // Gets called whenever the object is enabled 
    void OnEnable()
    {
        rb = this.GetComponentInParent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        // player animation is idle initially 
        if (animator)
        { 
            animator.SetFloat("speed", 0.0f);
        }
        // Hide climbing controls initially 
        HideClimbingPanel();
        HideHangOnWallBtn();
        // Initializing climbing transform helper
        helper = new GameObject().transform;
        helper.name = "climb helper";
        // Climbing check
        //CheckForClimb(1.5f);
    }

    void GetForwardInfo()
    {
        Vector3 origin = this.transform.position;
        origin.y += 1.0f;
        Vector3 dir = this.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, 3))
        {
            if ((hit.collider.gameObject.tag == "RockWall" || hit.collider.gameObject.tag == "Wall") && (!(isHanging) || !(isClimbingUp)))
            {
                ShowHangOnWallBtn();
            }
            else
            {
                HideHangOnWallBtn();
            }
        }
    }

    void CheckForClimb(float speedParam)
    {
        Vector3 origin = this.transform.position;
        origin.y += 1.0f;
        Vector3 dir = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, 3))
        {
            //Debug.Log("Seeing: " + hit.collider.gameObject.tag);
            InitForClimb(hit, speedParam);
        }
    }

    void InitForClimb(RaycastHit hit, float speedParam)
    {
        //isClimbing = true;
        // specify the player look rotation to be always looking at the obstacle it will be climbing 
        helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
        // the start position of the climbing is the initial position of the player 
        startPosition = this.transform.position;
        // the target position will be the point the raycast hit + offset from wall in the direction of the the obstacle 
        targetPosition = hit.point + (hit.normal * offsetFromWall);
        // Just in case this is not the first time the player is climbing 
        t = 0;
        inPosition = false;
        // Play the animation for climbing hanging idle 
        if (animator)
        {
            animator.SetFloat("speed", speedParam);
        }
    }


    // Joystick player movement
    void TouchInput()
    {
        if (joystick && rb)
        {
            float hor = joystick.Horizontal;
            float ver = joystick.Vertical;
            if (hor != 0 && ver != 0)
            {
                Vector3 playerMovement = new Vector3(hor, 0f, ver) * speed * Time.deltaTime;
                //rb.AddForce(playerMovement * speed * Time.deltaTime);
                //rb.velocity = playerMovement * speed * Time.deltaTime;
                this.transform.position += playerMovement;
                Quaternion lookRotation = Quaternion.LookRotation(playerMovement, Vector3.up);
                float step = rotationSpeed * Time.deltaTime;
                this.transform.rotation = Quaternion.RotateTowards(lookRotation, this.transform.rotation, step);
                if (hor > 0.4f || hor < -0.4f || ver > 0.4f || ver < 0.4f)
                {
                    // play running animation 
                    animator.SetFloat("speed", 0.9f);
                }
                else
                {
                    // play walking animation 
                    animator.SetFloat("speed", 0.5f);
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
                // play idle animation 
                animator.SetFloat("speed", 0.0f);
            }
        }
    }

    // Handle Jump movement
    public void JumpMovement()
    {
        if (rb)
        {
            rb.AddForce(Vector3.up * jumpForce);
            if (animator)
            {
                // play jump over animation 
                //animator.SetFloat("speed", 3.0f);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Rock")
        {
            // CheckForClimb();
            //ShowHangOnWallBtn();
        }
        if (collision.gameObject.tag == "SoulWell")
        {
            // Show missions list for the player 
        }
        if (collectableNames != null)
        {
            for (int i = 0; i < collectableNames.Count; i++)
            {
                if (collision.gameObject.tag == collectableNames[i])
                {
                    // Add collectable to Inventory 
                    if (inventoryController)
                    {
                        if (game_globals.itemTypes != null)
                        {
                            for (int j = 0; j < game_globals.itemTypes.Count; j++)
                            {
                                if (collision.gameObject.tag == game_globals.itemTypes[j].name)
                                {
                                    // Assign the corresponding item for the collectable
                                    inventoryController.AddItemToInventory(game_globals.itemTypes[j]);
                                    // Add Item to the database 
                                    game_db_manager.InsertItem(game_globals.playerUsername, game_globals.itemTypes[j].name, game_globals.itemTypes[j].category);
                                    // Remove object from the scene 
                                    Destroy(collision.gameObject);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Globals' itemTypes list is null ... ");
                        }
                    }
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RockWall")
        {
            HideHangOnWallBtn();
            DropFromWallBtnCallback();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "RockWall")
        {
            // Player is just hanging on the wall 
            if (isHanging && !(isClimbingDown) && !(isClimbingUp))
            {
                // Play Hang animation 
                CheckForClimb(1.5f);
            }
            // Player is just climbing up the wall 
            else if (!(isHanging) && !(isClimbingDown) && isClimbingUp)
            {
                // Play climb up animation 
                CheckForClimb(2.0f);
            }
            // Player is just climbing down the wall 
            else if (!(isHanging) && isClimbingDown && !(isClimbingUp))
            {
                // Play climb down animation 
                CheckForClimb(2.5f);
            }
        }
    }

    void FixedUpdate()
    {
        TouchInput();
    }

    void Update()
    {
        delta = Time.deltaTime;
        Tick(delta);
        GetForwardInfo();
    }

    // Holds some climbing controls 
    void Tick(float delta)
    {
        // is the player is not ready to start climbing 
        if (!(inPosition))
        {
            GetInPosition();
            return;
        }
    }

    void GetInPosition()
    {
        t += delta;

        if (t > 1)
        {
            t = 1;
            inPosition = true;

            // may be enable the IK 
        }

        if (isClimbingUp)
        {
            Vector3 tp = Vector3.Lerp(startPosition, targetPosition, t*gravityScaleFactor);
            this.transform.position = tp;
        }
    }

    Vector3 PosWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * offsetFromWall;
        return target + offset;
    }
}
