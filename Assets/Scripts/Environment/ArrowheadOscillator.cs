using UnityEngine;

public class ArrowheadOscillator : MonoBehaviour
{
    [Header("References")]
    public Transform arrowheadLeft;
    public Transform arrowheadRight;

    [Header("Motion Settings")]
    public float moveDistance = 0.2f; // The distance to move each cycle
    public float moveTime = 0.3f;     // Seconds for each move (tweak for speed)

    private Vector3 leftStartPos;
    private Vector3 rightStartPos;
    private bool movingForward = true;

    void Start()
    {
        leftStartPos = arrowheadLeft.localPosition;
        rightStartPos = arrowheadRight.localPosition;

        StartCoroutine(Oscillate());
    }

    System.Collections.IEnumerator Oscillate()
    {
        while (true)
        {
            // Compute target positions for each direction
            Vector3 leftTarget = movingForward
                ? leftStartPos + new Vector3(moveDistance, 0, 0)
                : leftStartPos - new Vector3(moveDistance, 0, 0);

            Vector3 rightTarget = movingForward
                ? rightStartPos - new Vector3(moveDistance, 0, 0)
                : rightStartPos + new Vector3(moveDistance, 0, 0);

            // Lerp both arrowheads over moveTime
            float elapsed = 0;
            Vector3 l0 = arrowheadLeft.localPosition;
            Vector3 r0 = arrowheadRight.localPosition;
            while (elapsed < moveTime)
            {
                arrowheadLeft.localPosition = Vector3.Lerp(l0, leftTarget, elapsed / moveTime);
                arrowheadRight.localPosition = Vector3.Lerp(r0, rightTarget, elapsed / moveTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            // Ensure on target after lerp completion
            arrowheadLeft.localPosition = leftTarget;
            arrowheadRight.localPosition = rightTarget;

            // Swap direction for next frame
            movingForward = !movingForward;
        }
    }
}
