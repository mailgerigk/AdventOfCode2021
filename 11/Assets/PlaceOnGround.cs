using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOnGround : MonoBehaviour
{
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        if(Physics.Raycast(new Ray(transform.position, -transform.up), out var hit, 9000, mask.value))
        {
            transform.position = hit.point;
        }
    }

}
