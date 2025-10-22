
using UnityEngine;

public class StopTimer : MonoBehaviour
{
    public DeathTimer deathTimer;
    private bool isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            deathTimer.Victory();
            Debug.Log("Victory Triggered!");
        }
    }
}
