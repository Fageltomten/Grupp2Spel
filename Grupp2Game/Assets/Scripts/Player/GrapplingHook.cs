using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> grapplePoints = new List<Vector3>();
    private Vector3 grapplingLastPoint;
    private float ropeLength;

    [SerializeField] private int PhysicsIterations = 10;
    [SerializeField] float ropeWidth = 0.1f;
    [SerializeField] float ropeOffset = 0.03f;
    [SerializeField] float checkDistance = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grapplePoints = new List<Vector3>(2);
        grapplePoints.Add(transform.position);
        grapplePoints.Add(new Vector3(4, 4, 4));
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        //grapplePoints.Add(transform.position);
        grapplingLastPoint = grapplePoints[0];
        lineRenderer.SetPosition(0, grapplePoints[0]);
        lineRenderer.SetPosition(1, grapplePoints[1]);
        ropeLength = (grapplePoints[0] - grapplePoints[1]).magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grapplePoints[1] = transform.position;
        for (int i = 0; i < PhysicsIterations; i++)
        {
            CheckCollisionPoints();
        }
        UpdatePhysics();
    }

    private void LateUpdate()
    {
        lineRenderer.positionCount = grapplePoints.Count;
        lineRenderer.SetPositions(grapplePoints.ToArray());
    }

    private void UpdatePhysics()
    {
        Vector3 velocity = grapplePoints[0] - grapplingLastPoint;
        Vector3 gravity = Vector3.down * 9.8f * Time.fixedDeltaTime;
        Vector3 toAdd = velocity + gravity * Time.fixedDeltaTime;
        Vector3 nextPoint = grapplePoints[0] + toAdd;
        grapplingLastPoint = grapplePoints[0];
        grapplePoints[0] = nextPoint;
    }

    private void CheckCollisionPoints()
    {
        if (Physics.SphereCast(grapplingLastPoint, ropeWidth, grapplePoints[0] - grapplingLastPoint, out RaycastHit hit, (grapplePoints[0] - grapplingLastPoint).magnitude))
        {
            grapplePoints[0] = hit.point + (grapplePoints[0] - hit.point).normalized * ropeWidth;
        }

        if (Physics.SphereCast(grapplePoints[0], ropeWidth, grapplePoints[1] - grapplePoints[0], out hit, (grapplePoints[0] - grapplePoints[1]).magnitude))
        {
            grapplePoints.Insert(1, hit.point + (GetClosestPoint(hit.point, grapplePoints[0], grapplePoints[1]) - hit.point).normalized * ropeWidth);
        }
        float length = 0;
        for (int i = 0; i < grapplePoints.Count - 1; i++)
        {
            length += (grapplePoints[i] - grapplePoints[i + 1]).magnitude;
        }

        float totalDiffrence = 0;
        float firstDiffrence = Vector3.Distance(grapplePoints[0], grapplePoints[1]);
        for (int i = 0; i + 1 < grapplePoints.Count; i++)
        {
            totalDiffrence += Vector3.Distance(grapplePoints[i], grapplePoints[i + 1]);
        }

        if (totalDiffrence > ropeLength)
        {
            Vector3 diffrence = grapplePoints[1] - grapplePoints[0];
            Vector3 moveAmount = diffrence * (totalDiffrence - ropeLength) / totalDiffrence;
            grapplePoints[0] += moveAmount;
        }
    }

    private Vector3 GetClosestPoint(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineDirection = lineEnd - lineStart;
        float lineLength = lineDirection.magnitude;
        lineDirection.Normalize();
        Vector3 pointDirection = point - lineStart;
        float closestPoint = Vector3.Dot(pointDirection, lineDirection);
        closestPoint = Mathf.Clamp(closestPoint, 0f, lineLength);
        return lineStart + lineDirection * closestPoint;
    }
}
