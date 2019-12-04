using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsPanel : UIPanel
{
    public Resolution[] Resolutions
    {
        get;
        private set;
    }
    public Dropdown ResolutionDropdown
    {
        get;
        private set;
    }

    private void Awake()
    {
        Resolutions = Screen.resolutions;
        ResolutionDropdown = GetComponentInChildren<Dropdown>();
    }

    private void Start()
    {
        ResolutionDropdown.ClearOptions();

        var currentResolution = 0;

        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < Resolutions.Length; i++)
        {
            var resolutionOption = Resolutions[i].width + " x " + Resolutions[i].height;
            resolutionOptions.Add(resolutionOption);

            if(Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        ResolutionDropdown.AddOptions(resolutionOptions);
        ResolutionDropdown.value = currentResolution;
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = Resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public override void BackButton()
    {
        base.BackButton();

        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.OPTIONS);
    }
}
