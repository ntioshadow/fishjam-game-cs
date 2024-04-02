using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInputs : MonoBehaviour
{
    public event EventHandler OnInteractionPressed;
    private PlayerInputActions playerInputActions;
    private void  Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interaction.performed += Interaction_performed;
    }

    private void Interaction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Interaction Pressed");
        OnInteractionPressed?.Invoke(this, EventArgs.Empty);
    }
   public Vector2 GetMovementVectorNormalized()
   {
       Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

       inputVector = inputVector.normalized;

       return inputVector;
   }
}
