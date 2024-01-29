using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCameraDespawner : MonoBehaviour
{
    private float timeToDespawn = 5f;
    private float timer;
    private bool isOutOfView;
    private IDespawn despawn;

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
        CheckCameraBounds();

        if (isOutOfView)
        {
            timer += Time.deltaTime;
            if (timer >= timeToDespawn)
            {
                Despawn();
            }
        }
    }

    private void CheckCameraBounds()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (!onScreen && !isOutOfView)
        {
            isOutOfView = true;
            timer = 0; // Reset timer
        }
        else if (onScreen && isOutOfView)
        {
            isOutOfView = false;
        }
    }

    private void Despawn()
    {
        Debug.Log("Despawning " + gameObject.name + " via off camera despawner");
        timer = 0;
        despawn.Despawn();
    }
}
