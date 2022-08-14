using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    public JugglingRobotV1 agent;
    public Rigidbody rig { get; set; }

    [Space]
    [Header("Random Initialization")]
    public Vector3 randomVelRange = new Vector3(0, 0, 0);
    public Bounds randomPosBounds;

    [Space]
    [Header("Death")]

    public Bounds ballBounds;
    public float outBoundsPunishment = 3;
    public float floorPunishment = 0;
    public float collisionReward = 1f;


    private void Awake() {
        rig = GetComponent<Rigidbody>();
        agent.OnRandomInit += new RandomInitDel(RandomInit);
    }

    void RandomInit() {
        rig.velocity = RandomLib.randomVec(-randomVelRange, randomVelRange);
        rig.angularVelocity = Vector3.zero;//TODO: randomize angular Vel too

        transform.localPosition = RandomLib.randomVec(randomPosBounds);
    }

    private bool AboveFloor(Vector3 pos) { return pos.y > ballBounds.min.y; }

    private void FixedUpdate() {
        if(!ballBounds.Contains(transform.localPosition)) {//if the ball hits the floor
            
            if (AboveFloor(transform.localPosition)) {//check, if the ball got out of bounds through the walls or the ceiling
                //Debug.Log("wall");
                agent.AddReward(-outBoundsPunishment);//punish
                StatMonitor.instance.wallOut++;
            }
            else {//ball hit the floor
                //Debug.Log("floor");
                agent.AddReward(-floorPunishment);//punish
                StatMonitor.instance.floorHits++;
            }
            
            agent.EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Player") {
            agent.AddReward(collisionReward);
            StatMonitor.instance.ballCol++;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.parent.TransformPoint(randomPosBounds.center), randomPosBounds.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.parent.TransformPoint(ballBounds.center), ballBounds.size);
    }
}
