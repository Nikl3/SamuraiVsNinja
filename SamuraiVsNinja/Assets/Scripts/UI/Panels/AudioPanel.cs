using System.Collections.Generic;

public class AudioPanel : UIPanel
{
    public UIScrollbar[] VolumeScrollbars
    {
        get;
        private set;
    }

    private void Awake()
    {       
        VolumeScrollbars = transform.GetComponentsInChildren<UIScrollbar>();
    }

    private void Start()
    {
        foreach (var volumeScrollbar in VolumeScrollbars)
        {
            volumeScrollbar.value = GameMaster.Instance.LoadVolume(volumeScrollbar.name);
        }
    }

    public override void BackButton()
    {
        base.BackButton();

        UIManager.Instance.ChangePanelState(PANEL_STATE.OPTIONS);
    }
}
