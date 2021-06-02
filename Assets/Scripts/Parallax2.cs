using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax2 : MonoBehaviour
{
    [Header("Layer config")]
    [Tooltip("Back to Front")] public GameObject[] Layers;
    [Tooltip("Back to Front")] public float[] ParallaxEffect;
    public float WidthOfLayer = 60;

    [Header("General")]
    public GameObject cam;


    private float[] length = new float[4];
    private float[] startposX = new float[4];
    private float[] startposY = new float[4];

    void Start()
    {
        for (int i = 0; i < Layers.Length; i++)
        {
            startposX[i] = Layers[i].transform.position.x;
            startposY[i] = Layers[i].transform.position.y;
            length[i] = WidthOfLayer;
        }
    }

    void Update()
    {
        for (int i = 0; i < Layers.Length; i++)
        {
            float distX = cam.transform.position.x * ParallaxEffect[i];
            float distY = cam.transform.position.y * 0.6F;
            Layers[i].transform.position = new Vector3(startposX[i] + distX, startposY[i] + distY, Layers[i].transform.position.z);
        }
    }
}
