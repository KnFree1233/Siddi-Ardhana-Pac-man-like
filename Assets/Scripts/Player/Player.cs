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
        SfxManager.Instance.PlayAudio("Power Up");
        playerMovement.ActiveInfinite();
        Debug.Log("Start Power Up");
        OnPowerUpStart?.Invoke();
        yield return new WaitForSeconds(powerUpDuration);
        isPowerUp = false;
        SfxManager.Instance.PlayAudio("Power Down");
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
            Enemy enemy = hit.gameObject.GetComponent<Enemy>();
            if(!enemy.isDead) enemy.Dead();
        }
    }

    public void Dead()
    {
        isInvisible = true;
        if (playerHealth <= 0) return;
        playerHealth--;
        if (playerHealth == 0)
        {
            OnPlayerLose?.Invoke();
        }
        else
        {
            SfxManager.Instance.PlayAudio("Dead");
            playerMovement.StopMovementEnabled();
            transform.position = respawnPoint.position;
            playerMovement.StopMovementEnabled();
            StartCoroutine(InvisibleTimer());
        }
        healthText.text = playerHealth.ToString();
    }

    private IEnumerator InvisibleTimer()
    {
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
