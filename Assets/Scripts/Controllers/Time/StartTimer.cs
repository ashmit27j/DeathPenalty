
using UnityEngine;

public class StartTimer : MonoBehaviour
{
    public DeathTimer deathTimer;
    private bool isActivated = false;

    void Start()
    {
        if (deathTimer == null)
        {
            deathTimer = FindObjectOfType<DeathTimer>();

            if (deathTimer == null)
            {
                Debug.LogError("DeathTimer not found in the scene!");
            }
        }
    }

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
