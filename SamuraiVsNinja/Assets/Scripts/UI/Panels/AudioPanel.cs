using UnityEngine;

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
        SetScrollbarValues();
    }

    private void SetScrollbarValues()
    {
        VolumeScrollbars[0].value = DecibelToLinearValue(PlayerPrefs.GetFloat("Master"));
        VolumeScrollbars[1].value = DecibelToLinearValue(PlayerPrefs.GetFloat("Music"));
        VolumeScrollbars[2].value = DecibelToLinearValue(PlayerPrefs.GetFloat("Sfx"));
    }

    private float DecibelToLinearValue(float decibelValue)
    {
        return Mathf.Pow(10.0f, decibelValue / 20.0f);
    }

    private float FloatToDesibel(float value)
    {
        return value != 0 ? Mathf.Log10(value) * 20.0f : -80f;
    }

    public void MasterScrollbar(float value)
    {
        GameManager.Instance.AudioMixer.SetFloat("Master", FloatToDesibel(value));
    }

    public void MusicScrollbar(float value)
    {
        GameManager.Instance.AudioMixer.SetFloat("Music", FloatToDesibel(value));
    }

    public void SfxScrollbar(float value)
    {
        GameManager.Instance.AudioMixer.SetFloat("Sfx", FloatToDesibel(value));
    }

    public override void BackButton()
    {
        base.BackButton();

        GameManager.Instance.SaveChannelValues();

        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.OPTIONS);
    }
}
