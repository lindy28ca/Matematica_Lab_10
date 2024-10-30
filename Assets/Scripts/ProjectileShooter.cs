using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform launchPoint;
    public float launchSpeed = 20f;
    public LineRenderer trajectoryLine;
    public Transform target;

    private InputAction shootAction;

    private void Awake()
    {
        var playerInput = new PlayerInput();
        shootAction = playerInput.Player.Shoot;
        shootAction.performed += _ => LaunchProjectile();

    }

    private void LaunchProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 directionToTarget = (target.position - launchPoint.position).normalized;
            rb.velocity = directionToTarget * launchSpeed;
        }
        Destroy(projectile, 0.5f);
    }

    private Vector3 CalculateLaunchVelocity(float speed, float angle)
    {
        float radianAngle = angle * Mathf.Deg2Rad;
        float vx = speed * Mathf.Cos(radianAngle);
        float vy = speed * Mathf.Sin(radianAngle);
        return new Vector3(vx, vy, 0);
    }

    private void DrawTrajectory(Vector3 startPos, Vector3 initialVelocity)
    {
        int points = 30;
        trajectoryLine.positionCount = points;
        Vector3 currentPosition = startPos;
        Vector3 currentVelocity = initialVelocity;

        for (int i = 0; i < points; i++)
        {
            trajectoryLine.SetPosition(i, currentPosition);
            currentPosition += currentVelocity * Time.fixedDeltaTime;
            currentVelocity.y += Physics.gravity.y * Time.fixedDeltaTime;
        }
    }

    private void OnEnable() => shootAction.Enable();
    private void OnDisable() => shootAction.Disable();
}
