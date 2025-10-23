
using UnityEngine;

public class StopTimer : MonoBehaviour
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
        Debug.Log("Triggered by: " + other.gameObject.name);
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            deathTimer.Victory();
            Debug.Log("Victory Triggered!");
        }
    }
}
