using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class BallArea : MonoBehaviour
{
    [Tooltip("The agent inside the area")]
    public BallAgent ball_agent;
    [Tooltip("The small ball inside the area")]
    public GameObject small_ball;
    [Tooltip("The log text that shows the cumulative reward of the agent")]
    public Text cumulative_reward_text;
    [Tooltip("Prefab of a live collectable")]
    public Collectable collectable_prefab;

    private List<GameObject> collectables_list;

    void Start()
    {
        ResetArea();
    }

    void Update()
    {
        // Update the cumulative reward text
        cumulative_reward_text.text = ball_agent.GetCumulativeReward().ToString("0.00");
    }

    /* Resets the area including collectables and balls placement */
    public void ResetArea()
    {
        RemoveAllCollectables();
        PlaceBall();
        PlaceSmallBall();
        SpawnCollectables(4, Academy.Instance.EnvironmentParameters.GetWithDefault("collectable_speed", 0.5f));
    }

    /* Remove specific collectable from the area when it is collected */
    public void RemoveSpecificCollectable(GameObject collectableObj)
    {
        collectables_list.Remove(collectableObj);
        Destroy(collectableObj);
    }

    /* The number of collectables remaining */
    public int CollectablesRemaining()
    {
        return collectables_list.Count;
    }

    /* Choose a random position on the X-Z plane within a partial circle shape */
    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        float angle = minAngle;

        if (maxRadius > minRadius)
        {
            // pick a random radius
            radius = Random.Range(minRadius, maxRadius);
        }

        if (maxAngle > minAngle)
        {
            // pick a random angle
            angle = Random.Range(minAngle, maxAngle);
        }

        // Center position + forward vector rotated around the Y axis by "angle" degrees, multiplies by "radius"
        return center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
    }

    /* Remove all the collectables from the area */
    private void RemoveAllCollectables()
    {
        if (collectables_list != null)
        {
            for (int i = 0; i < collectables_list.Count; i++)
            {
                if (collectables_list[i] != null)
                {
                    Destroy(collectables_list[i]);
                }
            }
        }

        collectables_list = new List<GameObject>();
    }

    /* Place the ball in the area */
    private void PlaceBall()
    {
        Rigidbody rigidbody = ball_agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        ball_agent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * 0.5f;
        ball_agent.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    /* Place the small ball in the area */
    private void PlaceSmallBall()
    {
        Rigidbody rigidbody = small_ball.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        small_ball.transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * 0.5f;
        small_ball.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    /* Spawn a number of collectables in the area and set their swim speed */
    private void SpawnCollectables(int count, float collectableSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            // Spawn and place the collectable 
            GameObject collectable_object = Instantiate<GameObject>(collectable_prefab.gameObject);
            collectable_object.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * 0.5f;
            collectable_object.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // Set the collectable's parent to this area's transform
            collectable_object.transform.SetParent(transform);

            // Keep track of the collectable
            collectables_list.Add(collectable_object);

            // Set the collectable speed 
            collectable_object.GetComponent<Collectable>().collectableSpeed = collectableSpeed;
        }
    }
}
