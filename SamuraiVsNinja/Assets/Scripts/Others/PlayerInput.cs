﻿
using UnityEngine;

public class PlayerInput : Singelton<PlayerInput>
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

    public PlayerEngine PlayerEngine { get; private set; }
    public CharacterController2D PlayerCharacterController2D { get; private set; }

    private void Awake()
    {
        PlayerEngine = GetComponent<PlayerEngine>();
        PlayerCharacterController2D = GetComponent<CharacterController2D>();
    }

    public void UpdateLocalInputs()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw(HORIZONTAL_AXIS), Input.GetAxisRaw(VERTICAL_AXIS));

        PlayerEngine.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
        {
            PlayerEngine.OnJumpInputDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            PlayerEngine.OnJumpInputUp();
        }

        PlayerEngine.CalculateMovement();
    }
}