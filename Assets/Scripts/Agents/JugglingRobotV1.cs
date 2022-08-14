using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public delegate void RandomInitDel();

public class JugglingRobotV1 : Agent { //Train with: mlagents-learn --run-id=jugglingRobotV1 --force

    public Rigidbody hand;
    public Ball ball;
    
    [Space]

    public Bounds randomPosBounds;
    public float speed = 5;

    public Bounds outerBounds;
    public float outBoundPunish = 3;
    

    public event RandomInitDel OnRandomInit;

    public Transform environtment;
 
    private Vector3 ToEnvPos(Vector3 worldPos) { return environtment.InverseTransformPoint(worldPos); }
    private Vector3 EnvToWorld(Vector3 locPos) { return environtment.TransformPoint(locPos); }
    private void SetEnvPos(Transform t, Vector3 pos) { t.position = environtment.TransformPoint(pos); }

    private void Reset() {
        environtment = transform;//fill the environment variable on script parameter reset
    }

    public override void OnEpisodeBegin() {
        base.OnEpisodeBegin();
        OnRandomInit.Invoke();

        StatMonitor.instance.UpdateCount();

        SetEnvPos(hand.transform, RandomLib.randomVec(randomPosBounds));
        hand.velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(hand.position);
        sensor.AddObservation(ball.rig.position);
        sensor.AddObservation(ball.rig.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        Vector3 vel;
        vel.x = actions.ContinuousActions[0];
        vel.y = actions.ContinuousActions[1];
        vel.z = actions.ContinuousActions[2];

        hand.velocity = vel * speed;

        if (!outerBounds.Contains(ToEnvPos(hand.transform.position))) {
            AddReward(-outBoundPunish);
            EndEpisode();
        }
    }

    //TESTING
    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> contrinuousActions = actionsOut.ContinuousActions;
        contrinuousActions[0] = Input.GetAxis("Horizontal");
        contrinuousActions[2] = Input.GetAxis("Vertical");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(EnvToWorld(randomPosBounds.center), randomPosBounds.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(EnvToWorld(outerBounds.center), outerBounds.size);
    }
}
