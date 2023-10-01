using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    [SerializeField]
    private float topPointY;
    [SerializeField]
    private float moveSpeed = 5f;

    private float initPointY;
    private bool moving = false;
    private bool busy = false;

    private void Start()
    {
        initPointY = transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!moving && !busy && other.CompareTag("Player")) // Assuming the player is the trigger
        {
            moving = true;
        }
    }

    private void Update()
    {
        if (moving)
        {
            MoveToTopPoint();
        }
    }

    private void MoveToTopPoint()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, topPointY, transform.position.z), step);

        if (transform.position.y >= topPointY)
        {
            moving = false;
            StartCoroutine(ReturnAfterDelay(3));
        }
    }

    private IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float step = moveSpeed * Time.deltaTime;
        Vector3 startPoint = new Vector3(transform.position.x, initPointY, transform.position.z);

        while (transform.position.y > initPointY)
        {
            busy = true;
            transform.position = Vector3.MoveTowards(transform.position, startPoint, step);
            yield return null;
        }


        // Reset the elevator's position after returning
        transform.position = startPoint;
        busy= false;
    }

}
