
using UnityEngine;

public class TimePoint : MonoBehaviour
{
    public float timeToAdd = 5f;
    public DeathTimer deathTimer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            deathTimer.AddTime(timeToAdd);
            Debug.Log("Added " + timeToAdd + " seconds! ez");
            Destroy(gameObject);
        }
    }
}
