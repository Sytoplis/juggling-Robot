using UnityEngine;

public class StatMonitor : MonoBehaviour
{
    public static StatMonitor instance;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("There is more than one StatMonitor");
            return;
        }
        instance = this;

        count = -FindObjectsOfType<JugglingRobotV1>().Length;//uncount the first round -> one measurement to little
    }

    public int printCount = 500;

    //-------------- HIDDEN VALUES ------------------------
    private int count = 0;
    public int wallOut { get; set; } = 0;
    public int floorHits { get; set; } = 0;
    public int ballCol { get; set; } = 0;



    public void UpdateCount() {
        count++;
        if (count >= printCount) {
            PrintCount();
        }
    }
    
    private void PrintCount() {
        //Debug.Log("Count: " + count);

        Debug.Log($"Avrg wall: {(float)wallOut / count}");
        Debug.Log($"Avrg floor: {(float)floorHits / count}");
        Debug.Log($"Avrg col: {(float)ballCol / count}");

        //Debug.Log("Wall: " + wallOut);
        //Debug.Log("Floor: " + floorHits);
        //Debug.Log("Col: " + ballCol);

        count = 0;
        wallOut = 0;
        floorHits = 0;
        ballCol = 0;
    }
}
