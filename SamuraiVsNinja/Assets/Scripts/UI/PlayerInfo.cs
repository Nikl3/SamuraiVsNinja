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
    private int healthpt = 3;
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
        if (healthpt > 1) {
            for (int i = 0; i < healthpoints.Length; i++) {
                if (healthpoints[i].gameObject.activeSelf) {
                    healthpoints[i].gameObject.SetActive(false);
                    healthpt--;
                    return;
                }
            }
        }
        else {
            healthpoints[healthpoints.Length - 1].gameObject.SetActive(false);
            healthpt--;
            print("helat loppu, respawncooldown activated");
        }
    }



    

    public void StartRangeCooldown(float rangeAttackCooldown)
    {
        StartCoroutine(IRangeAttackCooldown(rangeAttackCooldown));
    }

    private IEnumerator IRangeAttackCooldown(float cooldownTime)
    {
        IsCooldown = true;
        cooldownImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(cooldownTime);
        cooldownImage.gameObject.SetActive(false);
        IsCooldown = false;
  
    }
}
