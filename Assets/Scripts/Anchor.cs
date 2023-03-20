using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    public Cloth cloth;
    Collider clothCollider;
    // Start is called before the first frame update
    void Start()
    {
        clothCollider = GetComponent<Collider>();
        foreach (Mass m in cloth.masses)
        {
            if (clothCollider.bounds.Contains(m.position))
            {
                m.doesMove = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
