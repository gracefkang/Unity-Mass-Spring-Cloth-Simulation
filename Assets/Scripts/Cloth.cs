using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cloth : MonoBehaviour
{
    public enum IntegrationType
    {
        EulerExplicit,
        VerletExplicit,
        Symplectic
    }
    public IntegrationType integrator;

    public bool detectCollisions;
    public GameObject obstacle;
    private SphereCollider obstacleCollider;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    public List<Mass> masses = new List<Mass>();
    List<Spring> springs = new List<Spring>();

    float timestep = 0.001f;
    float speed = 0.01f;

    private void Awake()
    {
        obstacleCollider = obstacle.GetComponent<SphereCollider>();

        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        triangles = mesh.triangles;

        //initialize masses
        foreach (Vector3 vert in vertices)
        {
            Mass newMass = new Mass(vert);
            masses.Add(newMass);
        }

        //initialize and catalogue springs
        var seenSprings = new List<(int, int)>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Spring AB = new Spring(masses[triangles[i]], masses[triangles[i + 1]]);
            Spring BC = new Spring(masses[triangles[i+1]], masses[triangles[i + 2]]);
            Spring AC = new Spring(masses[triangles[i]], masses[triangles[i + 2]]);

            if (!(seenSprings.Contains((i, i + 1))))
            {
                springs.Add(AB);
                seenSprings.Add((i, i + 1));
                seenSprings.Add((i + 1, i));
            }

            if (!(seenSprings.Contains((i + 1, i + 2))))
            {
                springs.Add(BC);
                seenSprings.Add((i + 1, i + 2));
                seenSprings.Add((i + 2, i + 1));
            }

            if (!(seenSprings.Contains((i, i + 2))))
            {
                springs.Add(AC);
                seenSprings.Add((i, i + 2));
                seenSprings.Add((i + 2, i));
            }
        }

        //couldn't figure out how to add bending springs lol
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < masses.Count; i++)
        {
            vertices[i] = (masses[i].position);
        }
        mesh.vertices = vertices;
    }

    void IntegrateEulerExplicit(Mass m)
    {
        m.position += timestep * m.velocity;
        m.velocity += (m.force / m.mass) * timestep;
    }

    void IntegrateVerletExplicit(Mass m)
    {
        m.position = (2 * m.position) - m.prevPosition + ((m.force / m.mass) * timestep * timestep);
    }

    void IntegrateSymplectic(Mass m)
    {
        m.velocity += (m.force / m.mass) * timestep;
        m.position += timestep * m.velocity;
    }

    private void FixedUpdate()
    {

        for (int i = 0; i < (1/timestep * speed); i++)
        {
            foreach (Mass m in masses)
            {
                m.force = Vector3.zero;
                m.updateForces();
            }
            foreach (Spring s in springs)
            {
                s.updateForces();
            }
            for (int j = 0; j < masses.Count; j++)
            {
                if (masses[j].doesMove) {
                    var prevTemp = masses[j].position;
                    switch (integrator)
                    {
                        case IntegrationType.EulerExplicit:
                            IntegrateEulerExplicit(masses[j]);
                            break;
                        case IntegrationType.VerletExplicit:
                            IntegrateVerletExplicit(masses[j]);
                            break;
                        case IntegrationType.Symplectic:
                            IntegrateSymplectic(masses[j]);
                            break;
                        default:
                            Debug.Log("Error: integration method not specified.");
                            break;
                    }
                    masses[j].prevPosition = prevTemp;
                }

                if (detectCollisions)
                {
                    if (Vector3.Distance(masses[j].position, obstacle.transform.position) < 2.1f)
                    {
                        Vector3 sphereLine = Vector3.Normalize(masses[j].position - obstacle.transform.position);
                        while (Vector3.Distance(masses[j].position, obstacle.transform.position) < 2.1f)
                        {
                            masses[j].position += 0.01f * sphereLine;
                        }
                    }
                }

            }
        }
    } 

}
