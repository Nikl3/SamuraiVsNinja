using System.Collections;
using UnityEngine;

public class CharacterSelectPanel : UIPanel
{
    public Sprite NinjaIconSprite;
    public Sprite SamuraiIconSprite;

    private UIButton startButton;

    private int joinedPlayers = 0;

    private readonly Coroutine[] coroutines = new Coroutine[4];
    private JoinField[] joinFields;
    private JoinField[] GetJoinFields()
    {
        return GetComponentsInChildren<JoinField>(true);
    }

    public bool CanJoin
    {
        get;
        private set;
    }
    private void Awake()
    {
        joinFields = GetJoinFields();
        startButton = defaultSelectedObject.GetComponent<UIButton>();
    }
    
    public override void Update()
    {
        base.Update();

        if (CanJoin)
        {
            HandlePlayerJoinings();
            HandleCharacterChange();
        }
    }

    public override void OpenBehaviour()
    {
        base.OpenBehaviour();
        CanJoin = true;
        IsEnoughJoinedPlayers();
    }
    public override void CloseBehaviour()
    {
        base.CloseBehaviour();

        UnSetAllJoinField();
        CanJoin = false;
    }

    private void IsEnoughJoinedPlayers()
    {
        startButton.interactable = joinedPlayers > 0 ? true : false;
    }
    private void HandlePlayerJoinings()
    {
        for (int i = 0; i < joinFields.Length; i++)
        {
            if (!joinFields[i].HasJoined)
            {
                if (InputManager.Instance.Start_ButtonDown(i + 1))
                {
                    coroutines[i] = StartCoroutine(IChangeCharacter(i + 1));
                    PlayerDataManager.Instance.PlayerJoin(i + 1);
                    SetJoinField(i + 1, PlayerDataManager.Instance.GetPlayerData(i).PlayerColor);
                }
            }
        }

        for (int i = 0; i < joinFields.Length; i++)
        {
            if (joinFields[i].HasJoined)
            {
                if (InputManager.Instance.Y_ButtonDown(i + 1))
                {
                    StopCoroutine(coroutines[i]);
                    PlayerDataManager.Instance.PlayerUnjoin(i + 1);
                    UnSetJoinField(i + 1);
                }
            }
        }
    }
    private void HandleCharacterChange()
    {
        for (int i = 0; i < joinFields.Length; i++)
        {
            if (joinFields[i].HasJoined)
                InputManager.Instance.GetHorizontalAxisRaw(i + 1);
        }
    }
    private void ChangePlayerIcon(JoinField joinField, PlayerData playerData)
    {
        playerData.CharacterType = playerData.CharacterType == CHARACTER_TYPE.NINJA ? CHARACTER_TYPE.SAMURAI : CHARACTER_TYPE.NINJA;
        joinField.ChangeSprite(playerData.CharacterType == CHARACTER_TYPE.NINJA ? NinjaIconSprite : SamuraiIconSprite);
    }

    public void SetJoinField(int playerID, Color joinColor)
    {
        joinFields[playerID - 1].ChangeJoinFieldVisuals(playerID, joinColor);
        joinedPlayers++;
        IsEnoughJoinedPlayers();
    }
    public void UnSetJoinField(int playerID)
    {
        joinFields[playerID - 1].UnChangeJoinFieldVisuals();
        joinedPlayers--;
        IsEnoughJoinedPlayers();
    }
    public void UnSetAllJoinField()
    {
        foreach (var coroutine in coroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }

        for (int i = 0; i < joinFields.Length; i++)
        {
            joinFields[i].UnChangeJoinFieldVisuals();
        }

        joinedPlayers = 0;
    }

    private IEnumerator IChangeCharacter(int id)
    {
        while (true)
        {
            yield return new WaitUntil(() => InputManager.Instance.GetHorizontalAxisRaw(id) == 0);
            ChangePlayerIcon(joinFields[id - 1], (PlayerDataManager.Instance.GetPlayerData(id - 1)));
            yield return new WaitUntil(() => InputManager.Instance.GetHorizontalAxisRaw(id) >= 0.8f || InputManager.Instance.GetHorizontalAxisRaw(id) <= -0.8f);
        }
    }

    public void StartButton()
    {
        CloseBehaviour();
        GameManager.Instance.LoadSceneAsync(2);
    }
    public override void BackButton()
    {
        base.BackButton();
        UIManager_Old.Instance.ChangePanelState(PANEL_STATE.MAIN_MENU);
    }
}
