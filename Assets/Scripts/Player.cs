using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float powerUpDuration;
    [SerializeField] private Image staminaBar;
    [SerializeField] private float maxStamina;
    [SerializeField] private float runCost;
    [SerializeField] private float staminaRegenSpeed;
    [SerializeField] private float delayStaminaRegen;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private int playerHealth;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private float invisibleTime;
    [SerializeField] public float walkNoise;
    [SerializeField] public float runNoise;
    [SerializeField] private float rotationTime = 0.1f;

    private Animator animator;
    private float currStamina;
    private float staminaRegenTime;
    private Coroutine powerUpCoroutine;
    public Action OnPowerUpStart;
    public Action OnPowerUpStop;
    [HideInInspector] public bool isPowerUp;
    [HideInInspector] public bool isInvisible;
    [HideInInspector] public float currNoise;
    private Rigidbody _rigidbody;
    private static Action OnPlayerLose;
    private float rotationVelocity;
    private float maxSpeed;
    private bool isInfinite;

    void Awake()
    {
        isInfinite = true;
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        currStamina = maxStamina;
        currNoise = walkNoise;
    }

    void Start()
    {
        healthText.text = "Health: " + playerHealth.ToString();
        HideLockCursor();
    }

    void Update()
    {
        Movement();
    }

    private IEnumerator StartPowerUp()
    {
        isPowerUp = true;
        animator.SetTrigger("PowerUpState");
        currStamina = maxStamina;
        staminaBar.fillAmount = currStamina / maxStamina;
        Debug.Log("Start Power Up");
        OnPowerUpStart?.Invoke();
        yield return new WaitForSeconds(powerUpDuration);
        isPowerUp = false;
        animator.SetTrigger("IdleState");
        OnPowerUpStop?.Invoke();
        Debug.Log("Stop Power Up");
    }

    public void PickPowerUp()
    {
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
        powerUpCoroutine = StartCoroutine(StartPowerUp());
    }

    private void HideLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnFootstep()
    {
        
    }

    private void Movement()
    {
        float currSpeed = speed;
        MovementSpeed(ref currSpeed);
        maxSpeed = currSpeed;

        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (moveInput.magnitude >= 0.1)
        {
            Vector3 moveDirection;
            Rotating(out moveDirection, moveInput);

            _rigidbody.AddForce(moveDirection.normalized * speed);

            if (_rigidbody.velocity.magnitude > maxSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
            }

            animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    private void Rotating(out Vector3 moveDirection, Vector3 moveInput)
    {
        moveDirection = moveInput.x * mainCamera.right + moveInput.z * mainCamera.forward;
        float rotationAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref rotationVelocity, rotationTime);
        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
    }

    private void MovementSpeed(ref float currSpeed)
    {
        if (IsRunning() && currStamina > 0)
        {
            staminaRegenTime = 0;
            currSpeed = runningSpeed;
            if (currStamina > 0 && !isPowerUp)
            {
                currStamina -= runCost * Time.deltaTime;
                if (currStamina < 0) currStamina = 0;
            }
        }
        if (staminaRegenTime >= delayStaminaRegen && currStamina < maxStamina)
        {
            currStamina += staminaRegenSpeed * Time.deltaTime;
            if (currStamina >= maxStamina) currStamina = maxStamina;
        }
        else
        {
            staminaRegenTime += Time.deltaTime;
        }
        if (currStamina < maxStamina && (staminaRegenTime >= delayStaminaRegen || IsRunning()))
        {
            staminaBar.fillAmount = currStamina / maxStamina;
        }
    }

    private bool IsRunning()
    {
        float value = Input.GetAxis("Fire3");
        if (value == 1)
        {
            currNoise = runNoise;
            return true;
        }
        else
        {
            currNoise = walkNoise;
            return false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isPowerUp && other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Dead();
        }
    }

    public void Dead(int damage)
    {
        if (playerHealth <= 0) return;
        playerHealth -= damage;
        if (playerHealth == 0)
        {
            Debug.Log("You Lose!");
            OnPlayerLose?.Invoke();
        }
        else
        {
            StartCoroutine(InvisibleTimer());
            transform.position = respawnPoint.position;
        }
        healthText.text = "Health: " + playerHealth.ToString();
    }

    private IEnumerator InvisibleTimer()
    {
        isInvisible = true;
        animator.SetTrigger("InvisibleState");
        yield return new WaitForSeconds(invisibleTime);
        isInvisible = false;
        animator.SetTrigger("IdleState");
    }

    public void SetOnPlayerLose(Action function)
    {
        OnPlayerLose += function;
    }
}
