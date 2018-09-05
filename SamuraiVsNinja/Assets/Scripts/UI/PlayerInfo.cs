using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    private Transform parentContainer;
    //private Image playerImage;
    private Text coinCountText;
    private string text;
    private int coins;
    [SerializeField]
    private GameObject[] healthpoints;
    public Image atImage;
    private float attackCoolDown = 3f;

    private void Awake() 
    {
        //playerImage = GetComponent<Image>();
        coinCountText = GetComponentInChildren<Text>();
        parentContainer = GameObject.Find("HUD").transform.Find("PlayerInfoContainer");
    }

    private void Start()
    {
        transform.SetParent(parentContainer);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public void ModifyCoinValues(int amount)
    {
        coins += amount;
        coinCountText.text = coins.ToString();
    }

    public void AttackInd() {
        atImage.fillAmount -= 0.1f / attackCoolDown * Time.deltaTime;
    }

    public void TakeDMG() {
        print("damage taken");
        for (int i = 0; i < healthpoints.Length; i++) {
            if (healthpoints[i].activeSelf) {
                healthpoints[i].SetActive(false);
                return;
            }

        }
    }
}
