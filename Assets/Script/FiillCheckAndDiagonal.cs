using UnityEngine;

public class FiillCheckAndDiagonal : MonoBehaviour
{
    public bool Fill = false;
    public bool Diagonal;

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.tag=="Tiger" || trigger.tag == "Goat")
        {
            Fill = true;
        }
    }

    void OnTriggerExit(Collider trigger)
    {
        if (trigger.tag == "Tiger" || trigger.tag == "Goat")
        {
            Fill = false;
        }
    }
}
