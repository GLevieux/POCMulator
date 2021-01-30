using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holograme : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Material lineMat;
    Camera cam;


    public float holoHeight = 1;
    public float holoStart = 1;
    public float barHeight = 1;
    public float barWidth = 0.2f;

    public void DrawHolo()
    {
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Vertex(transform.position + transform.up * (holoStart));
        GL.Vertex(transform.position + transform.up * (holoStart + holoHeight));


        float realBarHeight = holoStart + (holoHeight * barHeight);
        GL.Vertex(transform.position - transform.right * barWidth * 0.5f + transform.up * realBarHeight);
        GL.Vertex(transform.position + transform.right * barWidth * 0.5f + transform.up * realBarHeight);
        GL.End();
    }

    void OnDrawGizmos()
    {
        DrawHolo();
    }

    void OnPostRender()
    {
        DrawHolo();


        
    }
}
