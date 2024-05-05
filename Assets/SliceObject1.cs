using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using JetBrains.Annotations;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class SliceObject1 : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;

    public Material crossSection;
    public float cutForce = 2000;
    // Start is called before the first frame update
    void Start()
    {
        velocityEstimator = new VelocityEstimator();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if(hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();
        

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if(hull !=  null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, crossSection);
            SetupSlicedComponent(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSection);
            SetupSlicedComponent(lowerHull);
            
            Destroy(target);

            
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}
