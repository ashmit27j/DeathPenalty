using UnityEngine;

public class InstaKill : MonoBehaviour
{
    // Set player tag as "Player" in the inspector for your character

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the DeathTimer and call EndGame
            DeathTimer timer = FindObjectOfType<DeathTimer>();
            if (timer != null)
            {
                timer.EndGame();
            }
        }
    }
}
