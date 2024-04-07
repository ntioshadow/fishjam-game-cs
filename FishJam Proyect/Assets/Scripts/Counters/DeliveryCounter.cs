using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter {

    public float deliveryTimeLimit = 30f;
    private float deliveryTimer;

    private RecipeSO currentRecipe;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (currentRecipe != null)
        {
            deliveryTimer -= Time.deltaTime;
            if (deliveryTimer <= 0f)
            {
                //DeliveryManager.Instance.OnRecipeFailed?.Invoke(this, EventArgs.Empty);
                ResetTimer();
            }
        }
    }

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                // Only accepts Plates

                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                player.GetKitchenObject().DestroySelf();
            }
        }
    }
    public void AssignRecipe(RecipeSO recipe)
    {
        currentRecipe = recipe;
        deliveryTimer = deliveryTimeLimit; // Reset delivery timer when assigning a new recipe
    }

    private void ResetTimer()
    {
        deliveryTimer = deliveryTimeLimit;
    }

}