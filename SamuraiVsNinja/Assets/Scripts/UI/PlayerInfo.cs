using Fabric;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    #region VARIABLES

    private Color defaultColor = new Color(1, 1, 1, 1);
    private Color emptyColor = new Color(1, 1, 1, 0.2f);

    private Text playerNameText;
    private Text onigiriCountText;
    private Outline playerNameTextOutline;
    private Image[] healthpointImages;

    private Image playerIcon, playerDashIcon, playerProjectileIcon;
    private Image throwAttackCooldownImage, dashAttackCooldownImage, playerRespawnCooldownImage;

    #endregion VARIABLES

    #region PROPERTIES

    public Player Owner
    {
        get;
        set;
    }
    public int OnigirisPicked { get; set; }
    public int OnigirisLost { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Attacks { get; set; }
    public int TotalHits { get; set; }
    public float HitPercent { get; set; }

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
        playerNameTextOutline = playerNameText.GetComponent<Outline>();
        onigiriCountText = transform.Find("OnigiriIcon").GetComponentInChildren<Text>();

        playerIcon = transform.Find("PlayerIcon").transform.Find("Icon").GetComponent<Image>();
        playerDashIcon = transform.Find("DashCooldown").transform.Find("Icon").GetComponent<Image>();
        playerProjectileIcon = transform.Find("RangeAttackCooldown").transform.Find("Icon").GetComponent<Image>();
        healthpointImages = transform.Find("HealthBar").GetComponentsInChildren<Image>();
        Array.Reverse(healthpointImages);
        throwAttackCooldownImage = transform.Find("RangeAttackCooldown").transform.Find("CooldownImage").GetComponent<Image>();
        dashAttackCooldownImage = transform.Find("DashCooldown").transform.Find("CooldownImage").GetComponent<Image>();
        playerRespawnCooldownImage = transform.Find("PlayerIcon").transform.Find("CooldownImage").GetComponent<Image>();
 
    }
    private void SetValues()
    {
        transform.SetParent(UIManager.Instance.PlayerInfoContainer);
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector2(0.8f, 0.8f);
        playerNameText.text = Owner.name;
        playerNameText.color = Owner.PlayerData.PlayerColor;
        playerNameTextOutline.effectColor = Color.black;
        gameObject.name = playerNameText.text + " Info";
    }
   
    private void ResetHealthPointIcons()
    {
        foreach (var healthpointImage in healthpointImages)
        {
            healthpointImage.color = defaultColor;
        }
    }

    public void SetPlayerInfoIcons()
    {
        if (Owner.PlayerData.PlayerType == PLAYER_TYPE.NINJA)
        {
            playerIcon.sprite = PlayerDataManager.Instance.PlayerIconSprite[0];
            playerDashIcon.sprite = PlayerDataManager.Instance.DashIconSprite[0];
            playerProjectileIcon.sprite = PlayerDataManager.Instance.ProjectileIconSprite[0];
        }
        else
        {
            playerIcon.sprite = PlayerDataManager.Instance.PlayerIconSprite[1];
            playerDashIcon.sprite = PlayerDataManager.Instance.DashIconSprite[1];
            playerProjectileIcon.sprite = PlayerDataManager.Instance.ProjectileIconSprite[1];
        }

        throwAttackCooldownImage.gameObject.SetActive(false);
        dashAttackCooldownImage.gameObject.SetActive(false);
        playerRespawnCooldownImage.gameObject.SetActive(false);
    }

    public void UpdateEndPanelStats()
    {
        if(Attacks != 0)
        {
            HitPercent = (TotalHits / (float)Attacks) * 100f;
        }

        Owner.PlayerData.EndGameStats.SetEndGameStats(playerIcon.sprite, Owner.name, OnigirisPicked, OnigirisLost, Kills, Deaths, HitPercent);               
    }
    public void UpdateOnigiris(int currentOnigiris)
    {
        onigiriCountText.text = currentOnigiris.ToString();
    }
    public void UpdateHealthPoints(int currentHealthPoints)
    {
        if(currentHealthPoints == 3)
        {
            ResetHealthPointIcons();
            return;
        }

        healthpointImages[currentHealthPoints].color = emptyColor;
    }
    public void StartThrowCooldown(bool IsCooldown,  float rangeAttackCooldown)
    {
        StartCoroutine(ICooldown(throwAttackCooldownImage, 0, rangeAttackCooldown));
    }
    public void StartDashCooldown(float dashCooldown)
    {
        StartCoroutine(ICooldown(dashAttackCooldownImage, 0, dashCooldown));
    }
    //public void StartRespawnCooldown( float respawnCooldown)
    //{
    //    StartCoroutine(ICooldown(playerRespawnCooldownImage, 0, respawnCooldown));
    //}

    #region COROUTINES

    private IEnumerator ICooldown(Image cooldownImage, float targetFillAmount, float cooldownTime)
    {
        cooldownImage.gameObject.SetActive(true);
        while (cooldownImage.fillAmount != targetFillAmount)
        {
            cooldownImage.fillAmount += cooldownImage.fillAmount < targetFillAmount ? (1f / cooldownTime) * Time.deltaTime : -(1f / cooldownTime) * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        cooldownImage.gameObject.SetActive(false);
        EventManager.Instance.PostEvent("Cooldown");

        cooldownImage.fillAmount = 1;
    }

    #endregion COROUTINES
}
