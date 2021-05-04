using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [SerializeField]
    private InputHandler inputHandler;

    [SerializeField]
    float rotationSpeed = 50f;

    float movement;
    bool isCurrentPlatform = false;
    bool hasCollided = false;

    SpriteRenderer sr2d;
    BoxCollider2D bc2d;

    void Awake()
    {
        // Puts Sprite Renderer and Box Collider into variables
        sr2d = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();

        // Setting platform color and disabling their colliders
        sr2d.color = new Color(sr2d.color.r, sr2d.color.g, sr2d.color.b, .5f);
        bc2d.enabled = false;
    }

    void Start() 
    {
        SetCurrentPlatform(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if player can and is rotating the platform
        Rotate();
    }

    void Rotate()
    {
        movement = inputHandler.GetRotationInput();

        if (isCurrentPlatform)
            transform.Rotate(Vector3.forward * -movement * rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        hasCollided = true;
    }

    public void SetCurrentPlatform(bool i)
    {
        if (i)
        {
            isCurrentPlatform = true;
            bc2d.enabled = true;
            StartCoroutine(FadeTo(1f, 0.25f));
            return;
        }

        if (!i)
        {
            isCurrentPlatform = false;
            bc2d.enabled = false;
            StartCoroutine(FadeTo(.5f, 0.25f));
        }

        IEnumerator FadeTo(float aValue, float aTime)
        {
            float alpha = sr2d.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(sr2d.color.r, sr2d.color.g, sr2d.color.b, Mathf.Lerp(alpha, aValue, t));
                sr2d.color = newColor;
                yield return null;
            }
        }
    }
}
