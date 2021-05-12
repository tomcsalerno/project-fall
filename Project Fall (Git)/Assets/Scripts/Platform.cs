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
    bool isTriggered = false;

    SpriteRenderer sr2d;
    BoxCollider2D bc2d;

    void Awake()
    {
        // Puts Sprite Renderer and Box Collider into variables
        sr2d = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        if (inputHandler == null)
            inputHandler = GameObject.FindGameObjectWithTag("Input Handler").GetComponent<InputHandler>();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        isTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isTriggered = false;
    }

    public bool CollisionCheck()
    {
        return hasCollided;
    }

    public bool TriggerCheck()
    {
        return isTriggered;
    }

    public void OnGameStart() {
        // Setting platform color and disabling their colliders
        StartCoroutine(FadeTo(.5f, 0.25f));
        bc2d.isTrigger = true;
    }

    public void SetCurrentPlatform(bool i)
    {
        if (i)
        {
            isCurrentPlatform = true;
            bc2d.isTrigger = false;
            StartCoroutine(FadeTo(1f, 0.25f));
            return;
        }

        if (!i)
        {
            isCurrentPlatform = false;
            hasCollided = false;
            bc2d.enabled = false;
            StartCoroutine(FadeTo(.5f, 0.25f));
        }
    }

    public void SetWinPlatform()
    {
        sr2d.color = new Color(0f, 191 / 255f, 1f, .5f);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * .75f, transform.localScale.z);
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = sr2d.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            sr2d.color = new Color(sr2d.color.r, sr2d.color.g, sr2d.color.b, Mathf.Lerp(alpha, aValue, t));
            yield return null;
        }
    }
}
