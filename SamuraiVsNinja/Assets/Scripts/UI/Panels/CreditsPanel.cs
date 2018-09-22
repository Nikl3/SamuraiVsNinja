using UnityEngine;

public class CreditsPanel : UIPanel
{
    private readonly string creditsAnimationTag = "Credits";
    private bool isCreditsRunning = false;

    public override void OpenBehaviour()
    {
        base.OpenBehaviour();

        if (!isCreditsRunning)
        {
            isCreditsRunning = true;
            UIManager.Instance.Animator.SetTrigger(creditsAnimationTag);
        }
    }

    public override void CloseBehaviour()
    {
        if (isCreditsRunning)
        {
            StopAnimation(UIManager.Instance.Animator);
        }

        base.CloseBehaviour();      
    }

    private void StopAnimation(Animator animator)
    {      
        animator.Play("Credits", 0, 0.98f);
        isCreditsRunning = false;       
    }
}
