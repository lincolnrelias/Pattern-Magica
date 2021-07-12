using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class hittingPoint : MonoBehaviour
{
    [SerializeField]float size=10f;
    // Start is called before the first frame update
    private void OnDrawGizmos() {
        Gizmos.DrawCube(transform.position,new Vector3(size,size,size));
    }
}
