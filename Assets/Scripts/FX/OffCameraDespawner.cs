using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCameraDespawner : MonoBehaviour
{
    private float timeToDespawn = 8f;
    private float timer = 0f;
    private bool isOutOfView = false;
    private IDespawn despawn;

    private float checkInterval = 2.0f;
    private float nextCheckTime = 0f;

    private void OnEnable()
    {
        // Reset states when the object is enabled
        isOutOfView = false;
        timer = 0;
        nextCheckTime = 0f; // Start checking immediately upon enable
        checkInterval = 2.0f;
    }

    private void Awake()
    {
        despawn = GetComponent<IDespawn>();
        if (despawn == null)
        {
            Debug.LogError(this.ToString() + " No IDespawn component found on " + gameObject.name);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextCheckTime)
        {
            CheckCameraBounds();

            if (timeToDespawn - timer <= 2.0f)
            {
                // Increase check frequency in the last 2 seconds
                checkInterval = 0f;
            }
            else
            {
                nextCheckTime = timer + checkInterval;
            }
        }

        if (isOutOfView && timer >= timeToDespawn)
        {
            Despawn();
        }
    }

    private void CheckCameraBounds()
    {
        //Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 screenPoint = ProCamera2D.Instance.GameCamera.WorldToViewportPoint(transform.position);
        bool currentlyOnScreen = screenPoint.z > 0 && screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;

        // Update isOutOfView based on whether the object is currently on screen or not
        isOutOfView = !currentlyOnScreen;
    }

    private void Despawn()
    {
        // Debug.Log("Despawning " + gameObject.name + " via off camera despawner");
        despawn.Despawn();

        // Optionally reset the timer and isOutOfView state here if the object can be re-enabled without being destroyed
        timer = 0;
        isOutOfView = false;
    }
}
