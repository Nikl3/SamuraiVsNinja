using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public EndGameStats EGS;
    public int OnigirisPicked;
    public int OnigirisLost;
    public int Kills;
    public int Deaths;
    public int Attacks;
    public int HitPreC;

    #region VARIABLES

    private Text playerNameText;
    private Text onigiriCountText;
    private Outline playerNameTextOutline;
    private Image[] healthpointImages;

    private Image playerIcon, playerDashIcon, playerProjectileIcon;
    private Image rangeAttackCooldownImage, dashAttackCooldownImage, playerRespawnCooldownImage;

    #endregion VARIABLES

    #region PROPERTIES

    public Player Owner
    {
        get;
        set;
    }

    #endregion PROPERTIES

    private void Awake() 
    {
        Initialize();
    }

    private void Start()
    {
        SetValues();
        CreatePlayerIndicator();       
    }

    private void Initialize()
    {
        playerNameText = transform.Find("PlayerName").GetComponent<Text>();
        playerNameTextOutline = playerNameText.GetComponent<Outline>();
        onigiriCountText = transform.Find("OnigiriIcon").GetComponentInChildren<Text>();

        playerIcon = transform.Find("PlayerIcon").transform.Find("Icon").GetComponent<Image>();
        playerDashIcon = transform.Find("DashCooldown").transform.Find("Icon").GetComponent<Image>();
        playerProjectileIcon = transform.Find("RangeAttackCooldown").transform.Find("Icon").GetComponent<Image>();
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
        transform.SetParent(UIManager.Instance.PlayerInfoContainer);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        playerNameText.text = Owner.name;
        playerNameText.color = Owner.PlayerData.PlayerColor;
        playerNameTextOutline.effectColor = Color.white;
        gameObject.name = playerNameText.text + " Info";

        playerIcon.sprite = Owner.PlayerData.PlayerIconSprite;
        playerDashIcon.sprite = Owner.PlayerData.DashIconSprite;
        playerProjectileIcon.sprite = Owner.PlayerData.ProjectileIconSprite;
    }

    private void CreatePlayerIndicator()
    {
        var playerIndicator = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 2));
        playerIndicator.GetComponent<PlayerIndicator>().ChangeTextVisuals("P" + Owner.PlayerData.ID, Owner.PlayerData.PlayerColor);
        playerIndicator.transform.SetParent(Owner.transform);
        playerIndicator.transform.localPosition = new Vector2(0, 4);
        playerIndicator.name = "Player " + Owner.PlayerData.ID + " Indicator";
    }

    private void ResetHealthPointImages()
    {
        foreach (var healthImage in healthpointImages)
        {
            healthImage.gameObject.SetActive(true);
        }
    }

    public void UpdateEndPanelStats()
    {
        EGS.SetEGStats(Owner.name, OnigirisPicked, OnigirisLost, Kills, Deaths, Attacks, HitPreC);               
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
