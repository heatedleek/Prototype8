using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float runSpeed;
    [SerializeField] float knockbackForce;
    [Range(0.0f, 1.0f)][SerializeField] float cutJumpHeight;
    [SerializeField] float smackPauseTime;

    [Header("VFX")]
    [SerializeField] ParticleSystem deathParticles;

    [SerializeField] Hook hook;


    private bool isAlive = true;

    private GameManager manager;
    private Rigidbody2D rb2d;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    float runInput = 0.0f;
    bool jump = false;
    private float defaultTimeScale;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerController = gameObject.GetComponent<PlayerController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultTimeScale = Time.timeScale;
        hook.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGameplayInput();
    }

    private void FixedUpdate()
    {
        if(isAlive)
        {
            bool hasJumped = playerController.Move(runInput * Time.fixedDeltaTime, jump);
            jump = false;
        }
	}

    void CheckForGameplayInput()
    {
        if (isAlive)
        {
            if(playerController.GetIsGrounded())
            {
                Jump();
            }
            runInput = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (Input.GetButtonDown("Jump"))
            {
                EnableHook();
            }
            if (Input.GetButtonUp("Jump"))
            {
                DisableHook();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Death"))
        {
            if(isAlive)
            {
                StartCoroutine("BecomeDead");
                Kill(collision.transform.position);
            }
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void Kill(Vector3 killerPos)
    {
        isAlive = false;
        spriteRenderer.color = Color.red;
        deathParticles.Play();
        AudioManager.Play(AudioClipName.Death);
        rb2d.freezeRotation = false;
        Vector2 direction = (transform.position - killerPos).normalized;
        rb2d.AddTorque(Random.Range(-knockbackForce, knockbackForce), ForceMode2D.Impulse);
        rb2d.velocity = direction * knockbackForce;
        manager.Lose();

    }

    public void Win()
    {
        Time.timeScale = defaultTimeScale;
        AudioManager.Play(AudioClipName.Goal);
        Destroy(gameObject);
    }
    public void Jump()
    {
        jump = true;

    }

    public void EnableHook()
    {
        
        hook.gameObject.SetActive(true);
        hook.ResetHook();

    }
    public void DisableHook()
    {
        hook.Unhooken();
        hook.gameObject.SetActive(false);
    }

    IEnumerator BecomeDead()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(smackPauseTime);
        Time.timeScale = defaultTimeScale;
    }
}
