using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowingTutorial : MonoBehaviour
{
    public static ThrowingTutorial instance;

    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public TextMeshProUGUI shurikenCountText; // Reference to your UI text

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        readyToThrow = true;
        UpdateShurikenUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Collider projectileCollider = projectile.GetComponent<Collider>();
        Collider[] playerColliders = GetComponentsInChildren<Collider>();

        foreach (Collider playerCollider in playerColliders)
        {
            Physics.IgnoreCollision(projectileCollider, playerCollider, true);
        }

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        Debug.Log("Current shuriken count: " + totalThrows);
        UpdateShurikenUI();

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    public void AddShuriken(int value)
    {
        totalThrows += value;
        Debug.Log("Current shuriken count: " + totalThrows);
        UpdateShurikenUI();
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void UpdateShurikenUI()
    {
        if (shurikenCountText != null)
            shurikenCountText.text = totalThrows.ToString();
    }
}
