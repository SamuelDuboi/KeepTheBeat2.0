using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesBehavior : MonoBehaviour
{
    public Material off;
    public Material on;
    private MeshRenderer mesh;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }


    public void On()
    {
        mesh.material = on;
    }
    public void Off()
    {
        mesh.material = off;
    }
}
