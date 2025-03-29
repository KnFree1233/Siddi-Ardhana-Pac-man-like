using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float powerUpDuration;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private int playerHealth;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private float invisibleTime;
    [SerializeField] private AudioSource powerUpSFX;
    [SerializeField] private AudioSource powerDownSFX;
    [SerializeField] private AudioSource playerDead;

    PlayerMovement playerMovement;
    private Coroutine powerUpCoroutine;
    public Action OnPowerUpStart;
    public Action OnPowerUpStop;
    [HideInInspector] public bool isPowerUp;
    [HideInInspector] public bool isInvisible;
    private static Action OnPlayerLose;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        healthText.text = playerHealth.ToString();
        HideLockCursor();
    }

    private IEnumerator StartPowerUp()
    {
        isPowerUp = true;
        powerUpSFX.Play();
        playerMovement.ActiveInfinite();
        Debug.Log("Start Power Up");
        OnPowerUpStart?.Invoke();
        yield return new WaitForSeconds(powerUpDuration);
        isPowerUp = false;
        powerDownSFX.Play();
        playerMovement.DeactivateInfinite();
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isPowerUp && hit.gameObject.CompareTag("Enemy"))
        {
            hit.gameObject.GetComponent<Enemy>().Dead();
        }
    }

    public void Dead(int damage)
    {
        playerDead.Play();
        if (playerHealth <= 0) return;
        playerHealth -= damage;
        if (playerHealth == 0)
        {
            OnPlayerLose?.Invoke();
        }
        else
        {
            StartCoroutine(InvisibleTimer());
            transform.position = respawnPoint.position;
        }
        healthText.text = playerHealth.ToString();
    }

    private IEnumerator InvisibleTimer()
    {
        isInvisible = true;
        yield return new WaitForSeconds(invisibleTime);
        isInvisible = false;
    }

    public void SetOnPlayerLose(Action function)
    {
        OnPlayerLose += function;
    }

    public float GetCurrNoise()
    {
        return playerMovement.currNoise;
    }

    public float GetWalkNoise()
    {
        return playerMovement.walkNoise;
    }

    public float GetRunNoise()
    {
        return playerMovement.runNoise;
    }
}
