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
    [SerializeField] float bulletTimeDuration = 3.0f;
    [Range(0.0f, 1.0f)][SerializeField] float bulletTimeFactor;
    [Header("VFX")]
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] Slider bulletTimeWheel;

    private bool isAlive = true;

    private GameManager manager;
    private Rigidbody2D rb2d;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    float runInput = 0.0f;
    bool jump = false;
    private float defaultTimeScale;
    private float bulletTimer = 0.0f;
    private bool bulletTimeActive = false;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerController = gameObject.GetComponent<PlayerController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        defaultTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGameplayInput();
        if(bulletTimeActive)
        {
            bulletTimer -= Time.deltaTime / Time.timeScale;
            Debug.Log(bulletTimer);
            if (bulletTimer < 0.0f)
            {
                bulletTimer = 0.0f;
                bulletTimeActive = false;
                Time.timeScale = defaultTimeScale;
            }
            bulletTimeWheel.value = bulletTimer / bulletTimeDuration;
        }
        else
        {
            bulletTimeWheel.value = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        if(isAlive)
        {
            bool hasJumped = playerController.Move(runInput * Time.fixedDeltaTime, jump);
            if (hasJumped && !bulletTimeActive)
            {
                AudioManager.Play(AudioClipName.TickTock);
                bulletTimer = bulletTimeDuration;
                bulletTimeActive = true;
                Time.timeScale = defaultTimeScale * bulletTimeFactor;
            }
            jump = false;
        }
	}

    void CheckForGameplayInput()
    {
        if (isAlive)
        {
            runInput = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            if (Input.GetButtonUp("Jump"))
            {
                if (rb2d.velocity.y > 0)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * cutJumpHeight);
                }
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
        bulletTimeWheel.gameObject.SetActive(false);
        AudioManager.Play(AudioClipName.Goal);
        Destroy(gameObject);
    }
    public void Jump()
    {
        jump = true;

    }

    IEnumerator BecomeDead()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(smackPauseTime);
        Time.timeScale = defaultTimeScale;
    }
}
