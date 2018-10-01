using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPanel : UIPanel
{
    public Sprite NinjaIconSprite;
    public Sprite SamuraiIconSprite;

    //private bool CanStart()
    //{
    //	return startButton.interactable = PlayerDataManager.Instance.CurrentlyJoinedPlayers == 4 || PlayerDataManager.Instance.CurrentlyJoinedPlayers == 2 ? true : false;
    //}
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
    }
    public override void CloseBehaviour()
    {
        base.CloseBehaviour();

        UnSetAllJoinField();
        CanJoin = false;
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
        playerData.PlayerType = playerData.PlayerType == PLAYER_TYPE.NINJA ? PLAYER_TYPE.SAMURAI : PLAYER_TYPE.NINJA;
        joinField.ChangeSprite(playerData.PlayerType == PLAYER_TYPE.NINJA ? SamuraiIconSprite : NinjaIconSprite);
    }

    public void SetJoinField(int playerID, Color joinColor)
    {
        joinFields[playerID - 1].ChangeJoinFieldVisuals(playerID, joinColor);
    }
    public void UnSetJoinField(int playerID)
    {
        joinFields[playerID - 1].UnChangeJoinFieldVisuals();
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
        GameMaster.Instance.LoadScene(1);
    }
    public override void BackButton()
    {
        base.BackButton();
        UIManager.Instance.ChangePanelState(PANEL_STATE.MAIN_MENU);
    }
}
