using UnityEngine;

public class TamagotchiMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5.0f;
    public float maxDistanceToCenter = 10.0f;
    public float minMovement = 2.0f;
    public float maxMovement = 4.0f;

    [Header("Timing")]
    public float minDelay = 3.0f;
    public float maxDelay = 5.0f;

    public bool isWalking { get; private set; } = false;

    private float timeToMove = 0.0f;
    private float destination = 0.0f;

    private void Update()
    {
        // Compute a random destination every now and then
        if (timeToMove <= Time.deltaTime)
            ComputeRandomDestination();
        else
            timeToMove -= Time.deltaTime;

        HandleMovement();
    }

    private void ComputeRandomDestination()
    {
        // Compute a point in [min, max] range
        destination = Random.Range(minMovement, maxMovement);
        // Randomly pick left or right
        destination *= (Random.value >= 0.5f) ? -1.0f : 1.0f;

        // If the target position is too far from the center, move in the other direction
        float position = transform.position.x;
        if (Mathf.Abs(position + destination) > maxDistanceToCenter)
            destination *= -1.0f;

        // Make the destination relative to the current position
        destination += position;

        // Set a random timer
        timeToMove = Random.Range(minDelay, maxDelay);
    }

    private void HandleMovement()
    {
        // Check if the current position is approximately equal to the destination
        float distanceToDestination = Mathf.Abs(destination - transform.position.x);
        // if not, the character needs to walk there
        isWalking = distanceToDestination > Mathf.Epsilon;

        // If the chicken is not walking, we're done here
        if (!isWalking)
            return;

        // Take the minimum between one frame of full-speed movement and the distance to the destination to avoid overshooting  
        float movement = Mathf.Min(distanceToDestination, speed * Time.deltaTime);
        // Compute the direction we're going
        var direction = (destination > transform.position.x) ? Vector3.right : Vector3.left;
        // Translate by movement units in the given direction
        transform.Translate(direction * movement);
    }
}
