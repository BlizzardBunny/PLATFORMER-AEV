using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    float startX = -9.08f;
    float startY = -3.18f;

    bool isAlive = true;

    Rigidbody2D rigidbody;
    Animator animator;
    [SerializeField] Collider2D bodyCollider2D;
    [SerializeField] Collider2D feetCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Run();        
        FlipSprite();

        if (CrossPlatformInputManager.GetButton("Cancel"))
        {
            Application.Quit();
        }

        if (CrossPlatformInputManager.GetButton("Fire1"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow*runSpeed, rigidbody.velocity.y);
        rigidbody.velocity = playerVelocity;

        if (Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon)
        {
            animator.SetTrigger("Run");
            animator.ResetTrigger("Idle");
        }
        else
        {
            animator.ResetTrigger("Run");
            animator.SetTrigger("Idle");
        }
    }

    private void Jump()
    {
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            rigidbody.velocity += jumpVelocityToAdd;
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Takeoff");
            StartCoroutine(inAir(0.2f));
        }

        IEnumerator inAir(float delay)
        {
            yield return new WaitForSeconds(delay);
            animator.ResetTrigger("Takeoff");
            animator.SetTrigger("Midair");
        }

        if (Mathf.Abs(rigidbody.velocity.y) > Mathf.Epsilon)
        {
            animator.SetTrigger("Midair");
        }
        else
        {
            animator.ResetTrigger("Midair");
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody.velocity.x), 1f);
        }
    }
}
