using ArionDigital;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchBehavior : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;

    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        m_Started = true;
    }

    public void Punch()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        int i = 0;
        //Check when there is a new collider coming into contact with the box
        foreach (var box in hitColliders)
        {
            Rigidbody rb = box.GetComponent<Rigidbody>();
            Vector3 breakForce = (rb.transform.position - transform.position).normalized * Random.Range(2, 20);
            rb.AddForce(breakForce, ForceMode.Impulse);
            if (box.GetComponent<CrashCrate>() != null)
            {
                box.GetComponent<CrashCrate>().Break();
            }
        }
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
