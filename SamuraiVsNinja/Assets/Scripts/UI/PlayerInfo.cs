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
    private Image rangeAttackCooldownImage;
    private Image dashAttackCooldownImage;
    private Image playerRespawnCooldownImage;

    #endregion VARIABLES

    #region PROPERTIES

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
        rangeAttackCooldownImage = transform.Find("RangeAttackCooldown").transform.Find("CooldownImage").GetComponent<Image>();
        rangeAttackCooldownImage.gameObject.SetActive(false);
        dashAttackCooldownImage = transform.Find("DashCooldown").transform.Find("CooldownImage").GetComponent<Image>();
        dashAttackCooldownImage.gameObject.SetActive(false);
        playerRespawnCooldownImage = transform.Find("PlayerIcon").transform.Find("CooldownImage").GetComponent<Image>();
        playerRespawnCooldownImage.gameObject.SetActive(false);
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

    public void StartRangeCooldown(bool IsCooldown,  float rangeAttackCooldown)
    {
        StartCoroutine(ICooldown(rangeAttackCooldownImage, 0, rangeAttackCooldown));
    }

    public void StartDashCooldown(float dashCooldown)
    {
        StartCoroutine(ICooldown(dashAttackCooldownImage, 0, dashCooldown));
    }

    public void StartRespawnCooldown( float respawnCooldown)
    {
        StartCoroutine(ICooldown(playerRespawnCooldownImage, 0, respawnCooldown));
    }

    #region COROUTINES

    private IEnumerator ICooldown(Image cooldownImage, float targetFillAmount, float cooldownTime)
    {
        cooldownImage.gameObject.SetActive(true);
        while (cooldownImage.fillAmount != targetFillAmount)
        {
            cooldownImage.fillAmount += cooldownImage.fillAmount < targetFillAmount ? (1f / cooldownTime) * Time.deltaTime : -(1f / cooldownTime) * Time.deltaTime;
            yield return null;
        }
        cooldownImage.gameObject.SetActive(false);
        Fabric.EventManager.Instance.PostEvent("Cooldown");

        cooldownImage.fillAmount = 1;
    }

    #endregion COROUTINES
}
