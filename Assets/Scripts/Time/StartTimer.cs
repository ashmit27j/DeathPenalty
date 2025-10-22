
using UnityEngine;

public class StartTimer : MonoBehaviour
{
    public DeathTimer deathTimer;
    private bool isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            deathTimer.StartTimer();
            Debug.Log("Timer Started!");
        }
    }
}
