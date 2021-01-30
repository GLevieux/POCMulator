using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAllHolos : MonoBehaviour
{
    // Start is called before the first frame update
    Holograme[] holos;
    void Start()
    {
        holos = FindObjectsOfType<Holograme>();
    }

    // Update is called once per frame
    void OnPostRender()
    {
        foreach (Holograme h in holos)
            h.DrawHolo();
    }
}
