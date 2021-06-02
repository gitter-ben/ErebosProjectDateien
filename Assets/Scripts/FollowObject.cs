using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [Header("General")]
    public GameObject ObjectToFollow;
    public bool ThreeDimensions;

    [Header("Clamp")]
    public bool useClamp;
    public bool useClampX;
    public bool useClampY;
    public bool useClampZ;

    public float ClampXMin;
    public float ClampXMax;
    public float ClampYMin;
    public float ClampYMax;
    public float ClampZMin;
    public float ClampZMax;

    [Header("Offset")]
    public bool useOffset;
    public bool useOffsetX;
    public bool useOffsetY;
    public bool useOffsetZ;

    public float OffsetX;
    public float OffsetY;
    public float OffsetZ;

    [Header("Smooth")]
    public bool useSmooth;
    [Range(0, 1)]public float moveSmoothTime = 0.3F;

    private Vector3 position;
    private Vector3 startpos;
    private Vector3 x;
    private Vector3 currentPos;
    private Vector3 targetPos;

    void Start()
    {
        startpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    position = new Vector3(ObjectToFollow.transform.position.x, ObjectToFollow.transform.position.y, (ThreeDimensions ? ObjectToFollow.transform.position.z : startpos.z));

    if (useClamp) {
        if (useClampX) {
            position.x = Mathf.Clamp(position.x, ClampXMin, ClampXMax);
        }
        if (useClampY) {
            position.y = Mathf.Clamp(position.y, ClampYMin, ClampYMax);
        }
        if (ThreeDimensions && useClampZ) {
            position.z = Mathf.Clamp(position.z, ClampZMin, ClampZMax);
        }
    }
    targetPos = startpos + position + OffsetY * Vector3.up;
    if (useSmooth) {
        currentPos = Vector3.SmoothDamp(targetPos, currentPos, ref x, moveSmoothTime);
    } else {
        currentPos = targetPos;
    }

    transform.position = currentPos;
    }
}
