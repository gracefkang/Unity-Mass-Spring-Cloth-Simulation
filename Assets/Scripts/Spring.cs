using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring 
{
    public Mass p;
    public Mass q;

    public float k_s = 5000.0f;
    public float k_d = 50.0f;

    public float L0;

    public Spring(Mass p, Mass q)
    {
        this.p = p;
        this.q = q;
        L0 = Vector3.Distance(p.position, q.position);
    }

    public void updateForces()
    {
        float currentDistance = Vector3.Distance(p.position, q.position);
        Vector3 normalized = ((p.position - q.position) / currentDistance);
        float temp = (k_s * (currentDistance - L0)) + (k_d * Vector3.Dot((p.velocity - q.velocity), normalized));
        Vector3 force = -temp * normalized;
        p.force += force;
        q.force -= force;
    }
}
