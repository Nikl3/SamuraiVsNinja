using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    #region VARIABLES

    private Transform parentContainer;
    private Text playerNameText;
    private Text onigiriCountText;
    private Image[] healthpointImages;
    private Image rangeAttackCooldown;
    private Image dashAttackCooldown;

    #endregion VARIABLES

    #region PROPERTIES

    public bool IsRangeCooldown
    {
        get;
        private set;
    }
    public bool IsDashCooldown
    {
        get;
        private set;
    }
    public string PlayerName
    {
        get
        {
            return playerNameText.text;
        }
        set
        {
            playerNameText.text = value;
        }
    }

    #endregion PROPERTIES

    private void Awake() 
    {
        Initialize();
    }

    private void Start()
    {
        SetValues();
    }

    private void Initialize()
    {
        playerNameText = transform.Find("PlayerName").GetComponent<Text>();
        onigiriCountText = transform.Find("OnigiriIcon").GetComponentInChildren<Text>();
        parentContainer = GameObject.Find("HUD").transform.Find("PlayerInfoContainer");
        healthpointImages = transform.Find("HealthBar").GetComponentsInChildren<Image>();
        Array.Reverse(healthpointImages);
        rangeAttackCooldown = transform.Find("RangeAttackCooldown").transform.Find("CooldownImage").GetComponent<Image>();
        rangeAttackCooldown.gameObject.SetActive(false);
        dashAttackCooldown = transform.Find("DashCooldown").transform.Find("CooldownImage").GetComponent<Image>();
        dashAttackCooldown.gameObject.SetActive(false);
    }

    private void SetValues()
    {
        transform.SetParent(parentContainer);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        gameObject.name = playerNameText.text + " Info";
    }

    private void ResetHealthPointImages()
    {
        foreach (var healthImage in healthpointImages)
        {
            healthImage.gameObject.SetActive(true);
        }
    }

    public void UpdateOnigiris(int currentOnigiris)
    {
        onigiriCountText.text = currentOnigiris.ToString();
    }

    public void UpdateHealthPoints(int currentHealthPoints)
    {
        if (currentHealthPoints == 3)
        {
            ResetHealthPointImages();
            return;
        }        

        healthpointImages[currentHealthPoints - 1].gameObject.SetActive(false);
    }

    public void StartRangeCooldown(float rangeAttackCooldown)
    {
        StartCoroutine(IRangeAttackCooldown(0, rangeAttackCooldown));
    }

    public void StartDashCooldown(float dashCooldown)
    {
        StartCoroutine(IDashCooldown(0, dashCooldown));
    }

    #region COROUTINES

    private IEnumerator IRangeAttackCooldown(float targetFillAmount, float cooldownTime)
    {
        IsRangeCooldown = true;
        rangeAttackCooldown.gameObject.SetActive(true);
        while (rangeAttackCooldown.fillAmount != targetFillAmount)
        {
            rangeAttackCooldown.fillAmount += rangeAttackCooldown.fillAmount < targetFillAmount ? (1f / cooldownTime) * Time.deltaTime : -(1f / cooldownTime) * Time.deltaTime;
            yield return null;
        }
        rangeAttackCooldown.gameObject.SetActive(false);
        IsRangeCooldown = false;
        rangeAttackCooldown.fillAmount = 1;
    }

    private IEnumerator IDashCooldown(float targetFillAmount, float cooldownTime)
    {
        IsDashCooldown = true;
        dashAttackCooldown.gameObject.SetActive(true);
        while (dashAttackCooldown.fillAmount != targetFillAmount)
        {
            dashAttackCooldown.fillAmount += dashAttackCooldown.fillAmount < targetFillAmount ? (1f / cooldownTime) * Time.deltaTime : -(1f / cooldownTime) * Time.deltaTime;
            yield return null;
        }
        dashAttackCooldown.gameObject.SetActive(false);
        IsDashCooldown = false;
        dashAttackCooldown.fillAmount = 1;
    }

    private IEnumerator IRespawnCooldown(float cooldownTime, GameObject targetObject, Vector2 spawnPosition) {
        targetObject.SetActive(false);
        targetObject.transform.position = spawnPosition;
        yield return new WaitForSeconds(cooldownTime);
        targetObject.SetActive(true);
    }

    #endregion COROUTINES
}
