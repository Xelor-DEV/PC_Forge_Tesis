using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

[RequireComponent(typeof(LineRenderer))]
public class RopeController : MonoBehaviour
{
    [Header("Referencias")]
    public Grabbable startGrabbable;
    public Grabbable endGrabbable;
    public LayerMask collisionLayers;

    [Header("Configuraci�n de la Soga")]
    public int segmentCount = 10;
    public float segmentRadius = 0.1f;
    public float maxLength = 5f;
    public float cooldownTime = 2f;
    public float ropeWidth = 0.05f;

    [Header("F�sica")]
    public float springForce = 100f;
    public float damper = 5f;
    public float massPerSegment = 0.1f;

    private LineRenderer lineRenderer;
    private List<Rigidbody> segments = new List<Rigidbody>();
    private bool isOverstretched = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        InitializeRope();
    }

    void InitializeRope()
    {
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        lineRenderer.positionCount = segmentCount;

        CreateSegments();
        ConnectSegments();
        AttachEnds();
    }

    void CreateSegments()
    {
        Vector3 startPos = startGrabbable.transform.position;
        Vector3 endPos = endGrabbable.transform.position;
        Vector3 segmentStep = (endPos - startPos) / (segmentCount - 1);

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = new GameObject($"Segment_{i}");
            segment.transform.position = startPos + segmentStep * i;
            segment.layer = LayerMask.NameToLayer("Rope");

            Rigidbody rb = segment.AddComponent<Rigidbody>();
            rb.mass = massPerSegment;
            rb.linearDamping = 0.5f;
            rb.angularDamping = 0.5f;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            SphereCollider collider = segment.AddComponent<SphereCollider>();
            collider.radius = segmentRadius;

            segments.Add(rb);
        }
    }

    void ConnectSegments()
    {
        for (int i = 1; i < segmentCount; i++)
        {
            SpringJoint joint = segments[i].gameObject.AddComponent<SpringJoint>();
            joint.connectedBody = segments[i - 1];
            joint.spring = springForce;
            joint.damper = damper;
            joint.minDistance = 0.1f;
            joint.maxDistance = 0.1f;
        }
    }

    void AttachEnds()
    {
        FixedJoint startJoint = segments[0].gameObject.AddComponent<FixedJoint>();
        startJoint.connectedBody = startGrabbable.GetComponent<Rigidbody>();

        FixedJoint endJoint = segments[segmentCount - 1].gameObject.AddComponent<FixedJoint>();
        endJoint.connectedBody = endGrabbable.GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateLineRenderer();
        CheckRopeLength();
    }

    void UpdateLineRenderer()
    {
        Vector3[] positions = new Vector3[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            positions[i] = segments[i].position;
        }
        lineRenderer.SetPositions(positions);
    }

    void CheckRopeLength()
    {
        float currentLength = Vector3.Distance(
            startGrabbable.transform.position,
            endGrabbable.transform.position
        );

        if (currentLength > maxLength && !isOverstretched)
        {
            DisableGrabbables();
            StartCoroutine(EnableGrabbablesAfterCooldown());
        }
    }

    void DisableGrabbables()
    {
        startGrabbable.enabled = false;
        endGrabbable.enabled = false;
        isOverstretched = true;
    }

    IEnumerator EnableGrabbablesAfterCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        startGrabbable.enabled = true;
        endGrabbable.enabled = true;
        isOverstretched = false;
    }
}