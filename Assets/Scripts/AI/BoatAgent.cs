using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class BoatAgent : Agent
{
    public float speed = 5f;
    public Vector3 targetPosition = new Vector3(0, 0, 10);
    private Rigidbody rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 0.5f, 0);
        transform.localRotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Récupérer les actions continues
        float moveX = actionBuffers.ContinuousActions[0];
        float moveZ = actionBuffers.ContinuousActions[1];

        // Appliquer le mouvement
        Vector3 move = new Vector3(moveX, 0, moveZ) * speed;
        rb.AddForce(move);

        // Récompenses
        float distanceToTarget = Vector3.Distance(transform.localPosition, targetPosition);
        
        // Récompense si proche de l'objectif
        if(distanceToTarget < 1.0f)
        {
            SetReward(1.0f);
            EndEpisode();
        }        
    }
}
