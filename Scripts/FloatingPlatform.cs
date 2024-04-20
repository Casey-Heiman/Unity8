using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float floatHeight = 1f;  // Height the platform will float from its initial position
    public float floatSpeed = 1f;   // Speed at which the platform floats
    public float pauseDuration = 2f;  // Duration of pause at the top and bottom positions

    private Vector3 startPos;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(PlatformMovement());
    }

    IEnumerator PlatformMovement()
    {
        while (true)
        {
            float targetY;
            if (movingUp)
            {
                targetY = startPos.y + floatHeight;
            }
            else
            {
                targetY = startPos.y - floatHeight;
            }

            float step = floatSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPos.x, targetY, startPos.z), step);

            if (transform.position.y >= startPos.y + floatHeight || transform.position.y <= startPos.y - floatHeight)
            {
                yield return new WaitForSeconds(pauseDuration);
                movingUp = !movingUp;
            }

            yield return null;
        }
    }
}
