using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public EndGameStats EndGameStats;
    public int OnigirisPicked;
    public int OnigirisLost;
    public int Kills;
    public int Deaths;
    public int Attacks;
    public int HitPreC;

    private Color defaultColor = new Color(1, 1, 1, 1);
    private Color emptyColor = new Color(1, 1, 1, 0.2f);

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

        if(Owner.PlayerData.PlayerType == PLAYER_TYPE.NINJA)
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
    }
    private void CreatePlayerIndicator()
    {
        var playerIndicator = Instantiate(ResourceManager.Instance.GetPrefabByIndex(4, 2));
        playerIndicator.GetComponent<PlayerIndicator>().ChangeTextVisuals("P" + Owner.PlayerData.ID, Owner.PlayerData.PlayerColor);
        playerIndicator.transform.SetParent(Owner.transform);
        playerIndicator.transform.localPosition = new Vector2(0, 4);
        playerIndicator.name = "Player " + Owner.PlayerData.ID + " Indicator";
    }
    private void ResetHealthPointIcons()
    {
        foreach (var healthpointImage in healthpointImages)
        {
            healthpointImage.color = defaultColor;
        }
    }

    public void UpdateEndPanelStats()
    {
        EndGameStats.SetEndGameStats(playerIcon.sprite, Owner.name, OnigirisPicked, OnigirisLost, Kills, Deaths, Attacks, HitPreC);               
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
