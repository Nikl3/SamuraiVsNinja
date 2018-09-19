using UnityEngine;

public class CreditsPanel : UIPanel
{
    private string creditsAnimationTag = "Credits";
    private bool isCreditsRunning = false;

    public string CreditsAnimationTag
    {
        get
        {
            return creditsAnimationTag;
        }

        set
        {
            creditsAnimationTag = value;
        }
    }

    public override void OpenBehaviour()
    {
        base.OpenBehaviour();

        if (!isCreditsRunning)
        {
            isCreditsRunning = true;
            //UIManager.Instance.MainMenuCanvasAnimator.Play("Credits");
        }
    }

    private void CancelCredits()
    {
        if (InputManager.Instance.Y_ButtonDown(1) && isCreditsRunning)
        {
            //StopAnimation(UIManager.Instance.MainMenuCanvasAnimator);
        }
    }

    private void StopAnimation(Animator animator)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag(CreditsAnimationTag))
        {
            animator.Play("Credits", 0, 0.98f);
            isCreditsRunning = false;
        }
    }

    private void Update()
    {
        if (isCreditsRunning)
        {
            CancelCredits();
            return;
        }
    }
}
