using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    private bool isOnPlatform;
    private Transform platformTransform;
    private Vector3 lastPlatformPosition;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if(isOnPlatform)
        {
            Vector3 deltaPosition = platformTransform.position - lastPlatformPosition;
            transform.position = transform.position + deltaPosition;
            lastPlatformPosition = platformTransform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Moving")
        {
            platformTransform = collision.transform;
            lastPlatformPosition = platformTransform.position;
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Moving")
        {
            isOnPlatform = false;
            platformTransform = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
