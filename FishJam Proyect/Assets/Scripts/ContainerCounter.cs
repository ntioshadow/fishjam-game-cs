using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
   [SerializeField] private KitchenObjectSO kitchenObjectSO;


   public override void Interact(Player player){
    if(!player.HasKitchenObject()){
         Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
         kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
   }

}
}



