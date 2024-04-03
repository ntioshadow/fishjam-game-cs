using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventsArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventsArgs : EventArgs {
        public ClearCounter selectedCounter;
    }
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private GameInputs gameInputs;
    [SerializeField] private LayerMask interactableLayerMask;

    private Vector3 lastInteractionPosition;
    private ClearCounter selectedCounter;

    private void Awake(){
        if (Instance != null) {
            Debug.LogError("There are 2 or more players");
        }
        Instance = this;
    }

    private void Start(){
        gameInputs.OnInteractionPressed += GameInputs_OnInteractionPressed;
    }

    private void GameInputs_OnInteractionPressed(object sender, System.EventArgs e)
    {
        if(selectedCounter != null){
            selectedCounter.Interact();
        }
    }
      
    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction(){

        
        Vector2 inputVector = gameInputs.GetMovementVectorNormalized();

        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y);

        if(movement != Vector3.zero){
            lastInteractionPosition = movement;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractionPosition, out RaycastHit raycastHit, interactDistance,interactableLayerMask)){
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)){
                if(clearCounter != selectedCounter){
                    SetSelectedCounter(clearCounter);
                }
            }  else {
                    SetSelectedCounter(null);
                }
        } else {
            SetSelectedCounter(null);
        }

        Debug.Log(selectedCounter);
        
    }

    private void HandleMovement(){
        
        Vector2 inputVector = gameInputs.GetMovementVectorNormalized();
       
        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = speed * Time.deltaTime;
        float playerRadius= .7f;
        float playerHeight = 1.8f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position +  Vector3.up * playerHeight,playerRadius,movement,moveDistance);

        if(!canMove){
            Vector3 moveDirX = new Vector3(movement.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position +  Vector3.up * playerHeight,playerRadius,moveDirX,moveDistance);

            if(canMove){
                movement = moveDirX;
            } 
            
            else {
                Vector3 moveDirZ = new Vector3(0, 0, movement.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position +  Vector3.up * playerHeight,playerRadius,moveDirZ,moveDistance);
                if(canMove){
                    movement = moveDirZ;
                } 
                else {
                }
            
            }
        }
        if(canMove){
            transform.position += movement * moveDistance;   
        }
        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movement, Time.deltaTime * rotationSpeed);
    }

    private void SetSelectedCounter(ClearCounter selectedCounter){
        this.selectedCounter = selectedCounter;
                    OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventsArgs
                    {
                        selectedCounter = selectedCounter
                    });
    }
}

