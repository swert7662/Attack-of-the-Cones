using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopupFX : MonoBehaviour, IDespawn
{
    [SerializeField] VertexGradient startingGradient;
    private TextMeshPro textMesh;

    public float startingVanishTime = .1f;
    private float currentVanishTime;
    public float vanishingSpeed = 20f;

    private Vector3 startingScale;
    private float scaleUpAmount = 1.1f;
    private float scaleDownAmount = 2f;

    public float moveYSpeed = 20f;
    private TMP_ColorGradient textColor;
    private VertexGradient currentGradient;
    private Color startingTextColor;

    private void OnEnable()
    {
        currentVanishTime = startingVanishTime;
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        startingScale = transform.localScale;
        //textColor = new TMP_ColorGradient();
        //textColor = startingGradient;
        currentGradient = startingGradient;
    }

    void Update()
    {
        if (currentVanishTime > startingVanishTime * .5f)
        {
            transform.position += Vector3.up * moveYSpeed * Time.deltaTime;
            transform.localScale += Vector3.one * scaleUpAmount * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.up * (moveYSpeed / 2) * Time.deltaTime;
            transform.localScale -= Vector3.one * scaleDownAmount * Time.deltaTime;
        }

        currentVanishTime -= Time.deltaTime;
        if (currentVanishTime <= 0f)
        {
            currentGradient.topLeft.a -= vanishingSpeed * Time.deltaTime;
            currentGradient.topRight.a -= vanishingSpeed * Time.deltaTime;
            currentGradient.bottomLeft.a -= vanishingSpeed * Time.deltaTime;
            currentGradient.bottomRight.a -= vanishingSpeed * Time.deltaTime;

            // Apply the updated gradient
            textMesh.colorGradient = currentGradient;

            if (currentGradient.topLeft.a <= 0f)
            {
                //Despawn();
                Destroy(gameObject);
            }
        }
    }
    public void SetDamageTextValue(float damageValue)
    {
        textMesh.text = damageValue.ToString();
    }

    public void ResetForPool()
    {
        currentGradient = startingGradient;
        transform.localScale = startingScale;
    }

    public void Despawn()
    {
        ResetForPool();
        ObjectPoolManager.DespawnObject(gameObject);
    }
}
