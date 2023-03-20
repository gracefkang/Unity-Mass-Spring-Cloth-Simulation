using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mass 
{
    public Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);
    public float mass = 0.5f;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 force;
    public float c_d = 1.5f;
    public bool doesMove = true;

    // verlet
    public Vector3 prevPosition;

    public Mass(Vector3 pos)
    {
        this.velocity = Vector3.zero;
        this.force = Vector3.zero;
        this.position = pos;
        this.prevPosition = pos;
    }

    public void updateForces()
    {
        //gravity
        force += mass * gravity;

        //damping
        force -= c_d * mass * velocity;
    }
}
