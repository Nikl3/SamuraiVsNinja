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
    private Image cooldownImage;

    public bool IsCooldown
    {
        get;
        private set;
    }
    public string PlayerName
    {
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
        cooldownImage = transform.Find("RangeAttackCooldown").transform.Find("CooldownImage").GetComponent<Image>();
        cooldownImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        transform.SetParent(parentContainer);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        gameObject.name = playerNameText.text + " Info";
    }

    public void ModifyCoinValues(int amount)
    {
        onigiris += amount;
        onigiriCountText.text = onigiris.ToString();
    }

    public void TakeDamage()
    {
        for (int i = 0; i < healthpoints.Length; i++)
        {
            if (healthpoints[i].gameObject.activeSelf)
            {
                healthpoints[i].gameObject.SetActive(false);
                return;
            }
        }
    }

    public void StartRangeCooldown(float rangeAttackCooldown)
    {
        StartCoroutine(IRangeAttackCooldown(0, rangeAttackCooldown));
    }

    private IEnumerator IRangeAttackCooldown(float targetFillAmount, float cooldownTime)
    {
        IsCooldown = true;
        cooldownImage.gameObject.SetActive(true);
        while (cooldownImage.fillAmount != targetFillAmount)
        {
            cooldownImage.fillAmount += cooldownImage.fillAmount < targetFillAmount ? (1f / cooldownTime) * Time.unscaledDeltaTime : -(1f / cooldownTime) * Time.unscaledDeltaTime;
            yield return null;
        }
        cooldownImage.gameObject.SetActive(false);
        IsCooldown = false;
        cooldownImage.fillAmount = 1;
    }
}
