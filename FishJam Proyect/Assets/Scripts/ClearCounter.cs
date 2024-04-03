using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClearCounter : BaseCounter
{

   public event EventHandler OnPlayerGrabbedItem;

   [SerializeField] private KitchenObjectSO kitchenObjectSO;


   public override void Interact(Player player)
   {
     if(!HasKitchenObject())
     {
      if(player.HasKitchenObject())
      {
         player.GetKitchenObject().SetKitchenObjectParent(this);
      } 
      else 
      {

      }
     } 
     else 
     {
      if(player.HasKitchenObject())
      {
         
      } 
      else 
      {
         GetKitchenObject().SetKitchenObjectParent(player);
      }
     }
   
   }
}
