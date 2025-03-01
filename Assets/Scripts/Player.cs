using System;
using System.Collections;
using Unity.VisualScripting;
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

    private float currStamina;
    private float staminaRegenTime;
    private Coroutine powerUpCoroutine;
    public Action OnPowerUpStart;
    public Action OnPowerUpStop;

    Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        currStamina = maxStamina;
    }

    void Start()
    {
        HideLockCursor();
    }

    void Update()
    {
        Movement();
    }

    private IEnumerator StartPowerUp()
    {
        Debug.Log("Start Power Up");
        OnPowerUpStart?.Invoke();
        yield return new WaitForSeconds(powerUpDuration);
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

    private void Movement()
    {
        float currSpeed = speed;
        if (Running() && currStamina > 0)
        {
            staminaRegenTime = 0;
            currSpeed = runningSpeed;
            if (currStamina > 0)
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
        staminaBar.fillAmount = currStamina / maxStamina;

        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveDirection = mainCamera.transform.TransformDirection(moveInput);
        moveDirection.y = 0;

        Vector3 newMovement = _rigidbody.position + moveDirection.normalized * currSpeed * Time.deltaTime;

        _rigidbody.MovePosition(newMovement);
    }

    private bool Running()
    {
        float value = Input.GetAxis("Fire3");
        if (value == 1) return true;
        else return false;
    }
}
