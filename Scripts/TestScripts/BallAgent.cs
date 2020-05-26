using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class BallAgent : Agent
{
    [Tooltip("How fast the agent moves forward")]
    public float moveSpeed = 5f;
    [Tooltip("How fast the agent turns")]
    public float turnSpeed = 180f;
    [Tooltip("Prefab of the heart that appears when the small ball is fed")]
    public GameObject heartPrefab;
    [Tooltip("Prefab of the regurgitated collectable that appears when the small ball is fed")]
    public GameObject regurgitatedCollectablePrefab;

    public BallArea ball_area;
    new private Rigidbody rigidbody;
    public GameObject small_ball;
    private bool isFull;    // If true, the ball has enough collectables
    private float feedRadius = 0f;

    /* Initial setup, called when the agent is enabled */
    public override void Initialize()
    {
        base.Initialize();
        //ball_area = GetComponentInParent<BallArea>();
        //small_ball = ball_area.small_ball;
        rigidbody = GetComponent<Rigidbody>();
    }

    /* Perform actions based on a vector of numbers (param: vectorAction "The list of actions to take")*/
    public override void OnActionReceived(float[] vectorAction)
    {
        // Convert the first action to forward movement 
        float forwardAmount = vectorAction[0];

        // Convert the second action to turning left or right 
        float turnAmount = 0f;
        if (vectorAction[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            turnAmount = 1f;
        }

        // Apply movement 
        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        // Apply a tiny negative reward every step to encourage action 
        if (MaxStep > 0) AddReward(-1f / MaxStep);
    }

    /* Read inputs from the keyboard and convert them to a list of actions 
     * This is called only when the player wants to control the agent and 
     * hast set the behaviour type to "Heuristic Only" in the Behaviour Parameters inspector
     */
     public override void Heuristic(float[] actions_out)
    {
        float forward_action = 0f;
        float turn_action = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            forward_action = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // turn left 
            turn_action = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // turn right
            turn_action = 2f;
        }
        actions_out[0] = forward_action;
        actions_out[1] = turn_action;
    }
    /* Reset the agent and area */
    public void AgentReset()
    {
        isFull = false;
        ball_area.ResetArea();
        feedRadius = Academy.Instance.EnvironmentParameters.GetWithDefault("feed_radius", 0f);
    }
    /* Collect all non-raycast observations */
    public override void CollectObservations(VectorSensor sensor)
    {
        // Not Implemented 
    }
    void FixedUpdate()
    {
        // Request a decision every 5 steps. RequestDecision() automatically calls RequestAction(),
        // but for the steps in between, we need to call it explicitly to take action using the 
        // results from previous decision
        if (StepCount%5 == 0)
        {
            RequestDecision();
        }
        else
        {
            RequestAction();
        }

        // Test if the agent is close enough to feed the small ball 
        if (Vector3.Distance(transform.position, small_ball.transform.position) < feedRadius)
        {
            // Close enough, try to feed the baby 
            RegurgitateCollectable();
        }
    }
    // When the agent collides with something take action 
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Collectable"))
        {
            // try to eat the collectable 
            EatCollectable(collision.gameObject);
        }
        else if (collision.transform.CompareTag("SmallBall"))
        {
            // try to feed the small ball
            RegurgitateCollectable();
        }
    }
    // Check if the agent is full, if not, eat the collectable and get a reward
    private void EatCollectable(GameObject collectableObject)
    {
        if (isFull) return; // Can't eat another collectable when full
        isFull = true;

        ball_area.RemoveSpecificCollectable(collectableObject);

        AddReward(1f);
    }
    // Check if agent is full, if yes, feed the small ball
    private void RegurgitateCollectable()
    {
        if (!isFull) return;    // Nothing to regurgitate
        isFull = false;

        // Spawn the regurgitated collectable 
        GameObject regurgitatedCollectable = Instantiate<GameObject>(regurgitatedCollectablePrefab);
        regurgitatedCollectable.transform.parent = transform.parent;
        regurgitatedCollectable.transform.position = small_ball.transform.position;
        Destroy(regurgitatedCollectable, 4f);

        // Spawn Heart 
        GameObject heart = Instantiate<GameObject>(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.position = small_ball.transform.position + Vector3.up;
        Destroy(heart, 4f);

        AddReward(1f);

        if (ball_area.CollectablesRemaining() <= 0)
        {
            EndEpisode();
        }
    }
}
