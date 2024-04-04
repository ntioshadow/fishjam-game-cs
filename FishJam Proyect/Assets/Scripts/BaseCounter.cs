using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{  
   [SerializeField] private Transform counterTop;

   private KitchenObject kitchenObject;
    
    public virtual void Interact(Player player)
    {
        Debug.Log("Interacting with base counter");
    }

    public Transform GetKitchenObjectFollowTransform(){
      return counterTop;
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
