using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public bool hooked = false;
    Rigidbody2D rb2d;
    BoxCollider2D boxCollider;
    HingeJoint2D newHinge;

    Vector3 defaultRot = new Vector3( 0.0f, 0.0f, 45.0f );
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            Hooken(collision);
        }
    }

    public void ResetHook()
    {
        hooked = false;
        boxCollider.enabled = true;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localEulerAngles = defaultRot;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
    public void Hooken(Collision2D collision)
    {
        AudioManager.Play(AudioClipName.Turn);
        newHinge = gameObject.AddComponent < HingeJoint2D>();
        newHinge.anchor = transform.InverseTransformPoint(collision.GetContact(0).point);
        newHinge.connectedBody=collision.rigidbody;
        newHinge.connectedAnchor = transform.InverseTransformPoint(collision.GetContact(0).point);

        boxCollider.enabled = false;
        rb2d.constraints = RigidbodyConstraints2D.None;
        hooked = true;
    }
    public void Unhooken()
    {
        if(newHinge != null)
        {
            AudioManager.Play(AudioClipName.Jump);
            hooked = false;
            Destroy(newHinge);
            gameObject.GetComponentInParent<Rigidbody2D>().velocity *= 2f;
        }
    }
}
