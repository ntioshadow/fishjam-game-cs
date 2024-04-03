using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent{

    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventsArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventsArgs : EventArgs {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private GameInputs gameInputs;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private Transform KitchenOPbjectHoldPoint;

    private Vector3 lastInteractionPosition;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

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
            selectedCounter.Interact(this);
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
            if(raycastHit.transform.TryGetComponent(out BaseCounter  baseCounter)){
                if(baseCounter != selectedCounter){
                    SetSelectedCounter(baseCounter);
                }
            }  else {
                    SetSelectedCounter(null);
                }
        } else {
            SetSelectedCounter(null);
        }

        
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

    private void SetSelectedCounter(BaseCounter selectedCounter){
        this.selectedCounter = selectedCounter;
                    OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventsArgs
                    {
                        selectedCounter = selectedCounter
                    });
    
    
    
   }
      public Transform GetKitchenObjectFollowTransform(){
      return KitchenOPbjectHoldPoint;
   }
   public void SetKitchenObject(KitchenObject kitchenObject){
      this.kitchenObject = kitchenObject;
   }
   public KitchenObject GetKitchenObject(){
      return kitchenObject;
   }
   public void ClearKitchenObject(){
      kitchenObject = null;
   }
   public bool HasKitchenObject(){
      return kitchenObject != null;
   }

}

