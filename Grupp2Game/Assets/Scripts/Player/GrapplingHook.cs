using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using System.Drawing;
using Unity.Cinemachine;
using UnityEditor;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class GrapplingHook : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> grapplePoints = new List<Vector3>();
    private Vector3 grapplingLastPoint;
    private float ropeLength;
    private RaycastHit CameraHitPoint;
    private InputAction ShootAction;
    private Vector3 forceToAdd;
    private Rigidbody playerRigidbody;
    private InputAction dashAction;
    private Vector3 objectHitVector;
    private float drag;

    private Vector3 checkPoint1;
    private Vector3 checkPoint2;

    [SerializeField] private int PhysicsIterations = 10;
    [SerializeField] private LayerMask grapplingLayerMask;
    [SerializeField] private int lineCollisionDetail = 10;
    [SerializeField] private float ropeWidth = 0.1f;
    [SerializeField] private float ropeOffset = 0.03f;
    [SerializeField] private float checkDistance = 0.1f;
    [SerializeField] private float dashForce = 0.5f;
    [SerializeField] private float maxRopeLength = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        ShootAction = InputSystem.actions.FindAction("Attack");
        dashAction = InputSystem.actions.FindAction("Sprint");
        ShootAction.performed += _ => Shoot();
        ShootAction.canceled += _ => ShootRelease();
        dashAction.performed += _ => Dash();
        playerRigidbody = GetComponent<Rigidbody>();
        drag = playerRigidbody.linearDamping;
    }

    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (grapplePoints.Count == 0 || grapplePoints == null)
            return;
        /*for (int i = 0; i < PhysicsIterations; i++)
        {
            CheckCollisionPoints();
        }*/
        FixGrappleLength();
        UpdatePhysics();
    }
    
    private void FixGrappleLength() 
    {
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

    private void LateUpdate()
    {
        lineRenderer.positionCount = grapplePoints.Count;
        lineRenderer.SetPositions(grapplePoints.ToArray());
        if (grapplePoints.Count == 0 || grapplePoints == null)
            return;
        //print($"Position set\nGrapplepoint 0: {grapplePoints[0]}\nlastPoint: {grapplingLastPoint}");
        transform.position = grapplePoints[0];
        //playerRigidbody.linearVelocity = (grapplePoints[0] - grapplingLastPoint);
    }

    private void UpdatePhysics()
    {
        objectHitVector = Vector3.zero;
        playerRigidbody.linearVelocity = (grapplePoints[0] - grapplingLastPoint) / Time.fixedDeltaTime;
        Vector3 velocity = grapplePoints[0] - grapplingLastPoint;
        Vector3 gravity = Vector3.zero;
        gravity = Physics.gravity * Time.fixedDeltaTime;
        /*else
        {
            velocity = Vector3.Scale(playerRigidbody.linearVelocity.normalized, Vector3.one - transform.up) * playerRigidbody.linearVelocity.magnitude * Time.fixedDeltaTime;
            grapplingLastPoint = grapplePoints[0];
        }*/
        //Vector3 toAdd = velocity + (forceToAdd + gravity) * Time.fixedDeltaTime;
        velocity = velocity + (forceToAdd + gravity) * Time.fixedDeltaTime;
        velocity *= Mathf.Clamp01(1f - drag * Time.fixedDeltaTime);
        //("cool vector" + vector);
        Vector3 toAdd = (velocity + (forceToAdd + gravity) * Time.fixedDeltaTime);
        //print($" thing {Vector3.Scale(velocity  + (forceToAdd + gravity) * Time.fixedDeltaTime, (Vector3.one - objectHitVector.Abs() * 2))}");
        //print(Vector3.one - objectHitVector.Abs());
        SetObjectHitVector();
        float dot = Vector3.Dot(velocity.normalized, objectHitVector);
        Vector3 vector = Vector3.Scale(velocity.Abs(), objectHitVector) * dot;
        Vector3 nextPoint = grapplePoints[0] + toAdd - vector;
        grapplingLastPoint = grapplePoints[0];
        grapplePoints[0] = nextPoint;

        forceToAdd = Vector3.zero;
        objectHitVector = Vector3.zero;
    }

    private void SetObjectHitVector()
    {
        bool collided = false;
        var tempPoint = Vector3.zero;
        var point = grapplePoints[0];
        Collider coolPoint = null;
        for (float i = 0; i <= 1; i += 0.5f)
        {
            point = Vector3.Lerp(grapplingLastPoint, grapplePoints[0], i);
            Physics.OverlapSphere(point, transform.lossyScale.x / 2, grapplingLayerMask, QueryTriggerInteraction.Ignore).ToList().ForEach(x =>
            {
                if (x.transform.tag == "Player")
                    return;
                coolPoint = x;
                collided = true;
                tempPoint = point - x.ClosestPoint(point);

                grapplingLastPoint = grapplePoints[0];
                objectHitVector += (x.ClosestPoint(point) - point).normalized;
            });
            if (collided)
            {
                objectHitVector = objectHitVector.normalized;
                break;
            }
        }

        if (!collided)
            return;
        /*var tempDir = coolPoint.ClosestPoint(point) - point;
        if (Vector3.Dot(tempDir, point - grapplingLastPoint) < 0)
        {
            print("boom");
        }*/
        grapplePoints[0] += (coolPoint.ClosestPoint(grapplePoints[0]) + (tempPoint.normalized * (transform.lossyScale.x / 2 + 0.005f))) - grapplePoints[0]/* * 0.9f*/;
    }

    private void CheckCollisionPoints()
    {
        int runs = 0;
        for (float detail = 0f / lineCollisionDetail; detail <= 1; detail += 1f / lineCollisionDetail)
        {
            runs++;
            var lerpedVector = Vector3.Lerp(grapplingLastPoint, grapplePoints[0], detail);
            if (Physics.SphereCast(grapplePoints[1], ropeWidth, lerpedVector - grapplePoints[1], out RaycastHit hit4, Vector3.Distance(grapplePoints[1], lerpedVector), grapplingLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit4.transform.tag == "Player")
                    continue;
                Vector3 point = hit4.point;
                var direction = (GetClosestPoint(point, lerpedVector, grapplePoints[1]) - hit4.point).normalized;
                point += direction * (ropeWidth + ropeOffset);
                Physics.OverlapSphere(point, ropeWidth, grapplingLayerMask, QueryTriggerInteraction.Ignore).ToList().ForEach(x =>
                {
                    var tempPoint = point - x.ClosestPoint(point);
                    point = x.ClosestPoint(point) + (tempPoint.normalized * (ropeWidth + ropeOffset));
                });
                /*if (grapplePoints.Exists(x => Vector3.Distance(x, point) < 0.05f))
                    continue;*/
                grapplePoints.Insert(1, point);
            }

            Physics.OverlapSphere(grapplePoints[1], ropeWidth, grapplingLayerMask, QueryTriggerInteraction.Ignore).ToList().ForEach(x =>
            {
                var tempPoint = lerpedVector - x.ClosestPoint(grapplePoints[1]);
                grapplePoints[1] = x.ClosestPoint(grapplePoints[1]) + (tempPoint.normalized * (ropeWidth + ropeOffset));
            });


            if (grapplePoints.Count > 2)
            {
                checkPoint1 = grapplePoints[1] + (lerpedVector - grapplePoints[1]).normalized * MathF.Min(checkDistance, (lerpedVector - grapplePoints[1]).magnitude);
                checkPoint2 = grapplePoints[1] + (grapplePoints[2] - grapplePoints[1]).normalized * MathF.Min(checkDistance, (grapplePoints[2] - grapplePoints[1]).magnitude);
            }

            if (grapplePoints.Count > 2 && (!Physics.SphereCast(checkPoint1, ropeWidth, checkPoint2 - checkPoint1, out RaycastHit hit2, (checkPoint2 - checkPoint1).magnitude, grapplingLayerMask, QueryTriggerInteraction.Ignore) &&
                   !Physics.SphereCast(lerpedVector, ropeWidth, grapplePoints[2] - lerpedVector, out hit2, (grapplePoints[2] - lerpedVector).magnitude, grapplingLayerMask, QueryTriggerInteraction.Ignore)))
            {
                grapplePoints.RemoveAt(1);
                if (grapplePoints.Count > 2)
                {
                    checkPoint1 = grapplePoints[1] + (lerpedVector - grapplePoints[1]).normalized * MathF.Min(checkDistance, (lerpedVector - grapplePoints[1]).magnitude);
                    checkPoint2 = grapplePoints[1] + (grapplePoints[2] - grapplePoints[1]).normalized * MathF.Min(checkDistance, (grapplePoints[2] - grapplePoints[1]).magnitude);
                }
            }
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

    private void Shoot()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out CameraHitPoint, maxRopeLength, grapplingLayerMask, QueryTriggerInteraction.Ignore) && !CameraHitPoint.transform.CompareTag("UnGrappable"))
        {
            print("Shooting");
            grapplePoints = new List<Vector3>();
            grapplePoints.Add(transform.position);
            grapplePoints.Add(CameraHitPoint.point + CameraHitPoint.normal * (ropeWidth + ropeOffset));
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = ropeWidth;
            lineRenderer.endWidth = ropeWidth;
            grapplingLastPoint = transform.position - playerRigidbody.linearVelocity * Time.fixedDeltaTime;
            lineRenderer.SetPosition(0, grapplePoints[0]);
            lineRenderer.SetPosition(1, grapplePoints[1]);
            ropeLength = (grapplePoints[0] - grapplePoints[1]).magnitude + 0.005f;
        }
    }

    private void ShootRelease()
    {
        lineRenderer.positionCount = 0;
        if (grapplePoints.Count == 0 || grapplePoints == null)
            return;
        transform.position = grapplePoints[0];
        print($"GrappleReleased\nGrapplepoint 0: {grapplePoints[0]}\nlastPoint: {grapplingLastPoint}");
        //playerRigidbody.AddForce((grapplePoints[0] - grapplingLastPoint) * Time.fixedDeltaTime, ForceMode.VelocityChange);
        grapplePoints = new List<Vector3>();
    }

    private void Dash()
    {
        if (grapplePoints.Count == 0 || grapplePoints == null)
            return;
        Vector3 dashDirection = grapplePoints[0] - grapplingLastPoint;
        AddForce(dashDirection.normalized * dashForce, ForceMode.Impulse);
    }

    public void AddForce(Vector3 force)
    {
        forceToAdd += force;
    }

    public bool CanGrapple()
    {
        return Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out CameraHitPoint, maxRopeLength, grapplingLayerMask, QueryTriggerInteraction.Ignore);
    }

    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        //print("Adding force");
        switch (forceMode)
        {
            case ForceMode.Force:
                forceToAdd += force * Time.fixedDeltaTime / playerRigidbody.mass;
                break;
            case ForceMode.Impulse:
                forceToAdd += force / playerRigidbody.mass;
                break;
            case ForceMode.Acceleration:
                forceToAdd += force * Time.fixedDeltaTime;
                break;
            case ForceMode.VelocityChange:
                forceToAdd += force;
                break;
        }
    }

    public bool IsGrappled()
    {
        return !(grapplePoints == null || grapplePoints.Count == 0);
    }

    public void SetSpeed(Vector3 speed)
    {
        grapplingLastPoint = grapplePoints[0] - speed;
    }

    public void ResetVerticalVelocity()
    {
        grapplingLastPoint = grapplePoints[0] + Vector3.Scale(grapplingLastPoint - grapplePoints[0], Vector3.one - transform.up);
    }

    private void OnDrawGizmos()
    {
        if (grapplePoints.Count == 0 || grapplePoints == null)
            return;
        Gizmos.color = UnityEngine.Color.red;
        for (int i = 0; i < grapplePoints.Count; i++)
        {
            Gizmos.DrawSphere(grapplePoints[i], ropeWidth);
        }
        Gizmos.color = UnityEngine.Color.magenta;
        Gizmos.DrawSphere(checkPoint1, ropeWidth);
        Gizmos.color = UnityEngine.Color.blue;

        Gizmos.DrawSphere(checkPoint2, ropeWidth);
        Gizmos.DrawLine(checkPoint1, checkPoint2);
        Gizmos.DrawRay(new Ray(grapplePoints[0], objectHitVector));
    }
}
