using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour {
    public GameObject joinPanel, infoPanel;
    public Image infoPanelBG;

    public void SetPlayer(Color color) {
        joinPanel.SetActive(false);
        infoPanel.SetActive(true);

        infoPanelBG.color = color;
    }
}
