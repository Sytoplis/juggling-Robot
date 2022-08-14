using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandMove : MonoBehaviour
{
    public Vector3 targetPos;
    public float posScale = 1;
    public float difScale = 1;
    private Rigidbody rig;

    private void Awake() {
        rig = GetComponent<Rigidbody>();
    }

    private Vector3 lastPos;
    private void FixedUpdate() {
        Vector3 localTarget = targetPos - rig.position;//position
        Vector3 vel = (rig.position - lastPos) / Time.fixedDeltaTime;

        Vector3 force = localTarget * posScale + vel * difScale;
        rig.AddForce(force, ForceMode.Force);//using a PID controller to control the force

        lastPos = rig.position;
    }
}
