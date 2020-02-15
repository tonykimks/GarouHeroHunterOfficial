using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 4f;
    public Rigidbody2D rb;
    public Animator animator;
    public bool running = false;
    public AudioSource battleMusic;

    public GameObject battleStartPanel;

    Vector2 movement;

    private void Start()
    {
        battleStartPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("Running", running);

        if (Input.GetKey(KeyCode.X))
        {
            moveSpeed = 7f;
            running = true;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            moveSpeed = 4f;
            running = false;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetInteger("Direction", 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetInteger("Direction", 1);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("Direction", 3);
        }

        if (movement.x < 0.001 && movement.x > -0.001 && movement.y < 0.001 && movement.y > -0.001)
        {
            running = false;
        }




        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(startBattle());
        }
    }

    private void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    IEnumerator startBattle()
    {
        battleMusic.Play();
        battleStartPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        battleStartPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(0.2f);
        battleStartPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        battleStartPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(0.2f);
        battleStartPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        battleStartPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(0.2f);
        battleStartPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GarouHeroHunter");
        yield return 0;
    }
}
