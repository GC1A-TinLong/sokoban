using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // seconds before finished moving
    public float duration = 0.4f;
    // seconds after started moving
    float elapsedTime;
    Vector3 destination;
    Vector3 origin;

    public void MoveTo(Vector3 newDestination)
    {
        //transform.position = destination;
        elapsedTime = 0;
        // if during moving, set position to previous target location
        origin = destination;
        transform.position = origin;
        destination = newDestination;
    }

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        origin = destination;
    }

    // Update is called once per frame
    void Update()
    {
        if (origin == destination) { return; }

        elapsedTime += Time.deltaTime;
        float timeRate = elapsedTime / duration;
        if (timeRate > 1.0f) { timeRate = 1.0f; }

        Vector3 currentPosition = Vector3.Lerp(origin, destination, timeRate);
        transform.position = currentPosition;
    }
}
