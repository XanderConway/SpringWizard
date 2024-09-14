using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickDetect : MonoBehaviour
{
    //player rotation on x axis
    public bool isGrounded;
    public Vector3 rotationStart;
    public Vector3 rotationEnd;
    public float rotationDifference;
    public float rotationThreshold = 360.0f;

    // a magic to be spwaned when a trick is detected (sparkles, etc)
    public GameObject trickMagic;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        Debug.Log("rotationDifference: " + rotationDifference);
        if (rotationDifference > rotationThreshold)
        {
            Debug.Log("Trick!  " + rotationDifference);
            TrickDetected();
            rotationDifference = 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {

        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrounded)
        {
            rotationEnd = transform.up;
            float angle = Vector3.Angle(rotationStart, rotationEnd);
            rotationDifference += angle;
            rotationStart = rotationEnd;
        }

    }

    void TrickDetected()
    {
        Instantiate(trickMagic, transform.position, Quaternion.identity);
        //destroy the magic after 1 second
    }
}