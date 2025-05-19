using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Image staminaBar;
    [SerializeField] private float walkMaxSpeed;
    [SerializeField] private float runMaxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxStamina;
    [SerializeField] private float runCost;
    [SerializeField] private float staminaRegenSpeed;
    [SerializeField] private float delayStaminaRegen;
    [SerializeField] public float walkNoise;
    [SerializeField] public float runNoise;
    [SerializeField] private float rotationTime = 0.1f;

    private Animator animator;
    private CharacterController characterController;
    private Vector3 moveInput;
    private bool isStop;
    private float rotationVelocity;
    private float currMaxSpeed;
    private float currSpeed;
    private float currStamina;
    private float staminaRegenTime;
    private bool isStaminaInfinite;
    [HideInInspector] public float currNoise;

    private void Awake()
    {
        isStop = false;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currSpeed = 0;
        currStamina = maxStamina;
        currNoise = walkNoise;
        currMaxSpeed = walkMaxSpeed;
    }

    private void Update()
    {
        SetCurrMaxSpeed();
        SetCurrNoise();
        GravityPhysic();
        Movement();
    }

    private void Movement()
    {
        if(isStop) return;
        StaminaManagement(ref currMaxSpeed);

        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (HasMovementInput())
        {
            Vector3 moveDirection;
            Rotating(out moveDirection, moveInput);

            CalculateCurrSpeed(acceleration);
            characterController.Move(moveDirection * currSpeed * Time.deltaTime);

            setAnimatorSpeed(currSpeed);
        }
        else if (currSpeed > 0)
        {
            CalculateCurrSpeed(deceleration);
            setAnimatorSpeed(currSpeed);
        }
    }

    private void CalculateCurrSpeed(float speedChangeRate)
    {
        currSpeed = Mathf.MoveTowards(currSpeed, currMaxSpeed, speedChangeRate * Time.deltaTime);
    }

    private void GravityPhysic()
    {
        if (!characterController.isGrounded)
        {
            Vector3 gravityVelocity = Physics.gravity;
            gravityVelocity.y += gravityVelocity.y * Time.deltaTime;
            characterController.Move(gravityVelocity * Time.deltaTime);
        }
    }

    private void Rotating(out Vector3 moveDirection, Vector3 moveInput)
    {
        Vector3 cameraFoward = mainCamera.forward;
        cameraFoward.y = 0;
        Vector3 cameraRight = mainCamera.right;
        cameraRight.y = 0;

        moveDirection = moveInput.x * cameraRight + moveInput.z * cameraFoward;
        float rotationAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref rotationVelocity, rotationTime);
        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
    }

    private void StaminaManagement(ref float currMaxSpeed)
    {
        if (IsRunning() && currStamina > 0)
        {
            staminaRegenTime = 0;
            if (currStamina > 0 && !isStaminaInfinite)
            {
                currStamina -= runCost * Time.deltaTime;
                if (currStamina < 0) currStamina = 0;
            }
        }
        else if (staminaRegenTime >= delayStaminaRegen && currStamina < maxStamina && !IsRunning())
        {
            currStamina += staminaRegenSpeed * Time.deltaTime;
            if (currStamina >= maxStamina) currStamina = maxStamina;
        }
        else if (!IsRunning())
        {
            staminaRegenTime += Time.deltaTime;
        }
        if (currStamina < maxStamina && (staminaRegenTime >= delayStaminaRegen || IsRunning()))
        {
            UpdateStaminaBar();
        }
    }

    private bool HasMovementInput()
    {
        if (moveInput.magnitude >= 0.1f) return true;
        else return false;
    }

    private void SetCurrNoise()
    {
        if (IsRunning()) currNoise = runNoise;
        else if (HasMovementInput()) currNoise = walkNoise;
        else currNoise = 0;
    }

    private void SetCurrMaxSpeed()
    {
        if (isStop) return;
        else if (IsRunning() && currStamina > 0) currMaxSpeed = runMaxSpeed;
        else if (HasMovementInput()) currMaxSpeed = walkMaxSpeed;
        else currMaxSpeed = 0;
    }

    private void setAnimatorSpeed(float value)
    {
        animator.SetFloat("Speed", value);
    }

    private void UpdateStaminaBar()
    {
        staminaBar.fillAmount = currStamina / maxStamina;
    }

    private bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    public void ActiveInfinite()
    {
        isStaminaInfinite = true;
        currStamina = maxStamina;
        UpdateStaminaBar();
    }

    public void DeactivateInfinite()
    {
        isStaminaInfinite = false;
    }

    public void StopMovementEnabled()
    {
        isStop = !isStop;
        characterController.enabled = !isStop;
        if (isStop)
        {
            currSpeed = 0;
            currMaxSpeed = 0;
        }
    }

    public void OnFootstep()
    {

    }
}
