using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceRockGenerator : MonoBehaviour
{
    public GameObject surfaceRockPrefab;
    public int maxRocks;

    private List<GameObject> currentRocks = new List<GameObject>();

    private float lastTemp;
    private float lastRadius;

    private Renderer rend;
    private ThermalBody thermals;

    void Awake()
    {
        rend = GetComponentInParent<Renderer>();
        thermals = GetComponentInParent<ThermalBody>();
       
    }

    void Update()
    {
        var radius = rend.bounds.extents.magnitude * 0.5f;
        var temp = thermals.Temperature;

        // if radius or temperature has changed, regenerate rocks
        // todo: make this check work properly - consist radii
        if ((temp != lastTemp) || (radius < lastRadius-2f) || (radius > lastRadius + 2f))
        {
            if (currentRocks.Count < maxRocks)
            {
                DebugExtension.DebugWireSphere(this.transform.position, radius, 1f, true);

                var surfacePoint = Random.onUnitSphere * radius; // not sure this is correct - objects are being created in very odd places?
                var newRock = Instantiate(surfaceRockPrefab, surfacePoint, Quaternion.identity, this.transform);
                currentRocks.Add(newRock);
            }

            lastRadius = radius;
            lastTemp = temp;
        }
    }
}
