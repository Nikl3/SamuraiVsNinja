using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    private Transform parentContainer;
    //private Image playerImage;
    private Text playerNameText;
    private Text onigiriCountText;
    private int onigiris;
    private Image[] healthpoints;
    private int healthPoints = 3;
    private Image rangeAttackCooldown;
    private Image dashAttackCooldown;
    private readonly int targetOnigiri = 3;

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

    private void Awake() 
    {
        //playerImage = GetComponent<Image>();
        playerNameText = transform.Find("PlayerName").GetComponent<Text>();
        onigiriCountText = transform.Find("OnigiriIcon").GetComponentInChildren<Text>();
        parentContainer = GameObject.Find("HUD").transform.Find("PlayerInfoContainer");
        healthpoints = transform.Find("HealthBar").GetComponentsInChildren<Image>();
        Array.Reverse(healthpoints);
        rangeAttackCooldown = transform.Find("RangeAttackCooldown").transform.Find("CooldownImage").GetComponent<Image>();
        rangeAttackCooldown.gameObject.SetActive(false);
        dashAttackCooldown = transform.Find("DashCooldown").transform.Find("CooldownImage").GetComponent<Image>();
        dashAttackCooldown.gameObject.SetActive(false);
    }

    private void Start()
    {
        transform.SetParent(parentContainer);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        gameObject.name = playerNameText.text + " Info";
    }

    private void ResetPlayerStats(Player hittedPlayer)
    {
        hittedPlayer.transform.position = GameManager.Instance.RandomSpawnPoint();
        healthPoints = 3;
        foreach (var healthpoint in healthpoints)
        {
            healthpoint.gameObject.SetActive(true);
        }
    }

    public void ModifyCoinValues(int amount)
    {
        if (onigiris == targetOnigiri - 1)
        {
            onigiris += amount;
            onigiriCountText.text = onigiris.ToString();
            GameManager.Instance.Victory(PlayerName);
        }
        else
        {
            onigiris += amount;
            onigiriCountText.text = onigiris.ToString();
        }
    }

    public void TakeDamage(Player hittedPlayer, Vector2 direction)
    {
        if (hittedPlayer.CurrentState == PlayerState.Normal)
        {
            if (healthPoints > 1)
            {
                hittedPlayer.PlayerEngine.OnKnockback(Vector2.right * 10, direction.x);
                hittedPlayer.PlayAudioClip(2);

                for (int i = 0; i < healthpoints.Length; i++)
                {
                    if (healthpoints[i].gameObject.activeSelf)
                    {
                        healthpoints[i].gameObject.SetActive(false);
                        healthPoints--;
                        hittedPlayer.ReturnState();
                        if (onigiris > 0)
                        {
                            ModifyCoinValues(-1);
                            Instantiate(ResourceManager.Instance.GetPrefabByIndex(1, 0), hittedPlayer.transform.position, Quaternion.identity);
                        }
                        return;
                    }
                }
            }
            else
            {
                hittedPlayer.PlayAudioClip(3);
                Instantiate(ResourceManager.Instance.GetPrefabByIndex(5, 0), hittedPlayer.transform.position, Quaternion.identity);
                healthpoints[healthpoints.Length - 1].gameObject.SetActive(false);
                healthPoints--;
                hittedPlayer.ReturnState(0.4f);
                ResetPlayerStats(hittedPlayer);
            }
        }
    }

    public void StartRangeCooldown(float rangeAttackCooldown)
    {
        StartCoroutine(IRangeAttackCooldown(0, rangeAttackCooldown));
    }

    public void StartDashCooldown(float dashCooldown)
    {
        StartCoroutine(IDashCooldown(0, dashCooldown));
    }

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
}
