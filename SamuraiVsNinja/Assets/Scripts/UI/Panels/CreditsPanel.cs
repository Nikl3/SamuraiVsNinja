﻿using UnityEngine;

public class CreditsPanel : UIPanel
{
    private bool isCreditsRunning = false;

    public override void OpenBehaviour()
    {
        base.OpenBehaviour();

        if (!isCreditsRunning)
        {
            ManageCreditsAnimation(UIManager_Old.Instance.UIManagerAnimator, true);
        }
    }
    public override void CloseBehaviour()
    {
        if (isCreditsRunning)
        {
            ManageCreditsAnimation(UIManager_Old.Instance.UIManagerAnimator, false);
        }

        base.CloseBehaviour();
    }

    private void ManageCreditsAnimation(Animator animator, bool isPlaying)
    {
        animator.SetBool("IsCreditsPlaying", isPlaying);
        isCreditsRunning = isPlaying;       
    }

    public override void BackButton()
    {
        base.BackButton();
        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.MAIN_MENU);
    }
}
