using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class DeliveryCounter : BaseCounter {

    public float deliveryTimeLimit = 30f;
    private float deliveryTimer;
    [SerializeField] private float eatingTimeMax = 7f;
    public GameObject sharkPrefab;
    private float eatingTime;
    public enum State {
        Idle,
        Ordering,
        Waiting,
        Eating,
    }
    private State state;

    private RecipeSO currentRecipe;

    private void Start()
    {
        ResetTimer();
        state = State.Idle;
    }

    private void Update()
    {
        switch(state) {
            case State.Idle:
                break;
            case State.Ordering:
                decreaseTime();
                break;
            case State.Waiting:
                decreaseTime();
                break;
            case State.Eating:
                Eat();
                break;
        }
    }

    private void Eat()
    {
        eatingTime -= Time.deltaTime;
        if (eatingTime <= 0f)
            {
                currentRecipe = null;
                state = State.Idle;
                ResetTimer();
            }
    }

    private void decreaseTime(){
        deliveryTimer -= Time.deltaTime;
            if (deliveryTimer <= 0f)
            {
                DeliveryManager.Instance.FailedRecipe();
                currentRecipe = null;
                state = State.Idle;
                SpawnShark();
                ResetTimer();
            }
    }

    private void SpawnShark()
    {
        Vector3 spawnPosition = transform.position + new Vector3(1f, 0f, 0f);
        Instantiate(sharkPrefab, spawnPosition, Quaternion.identity);
    }

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                // Only accepts Plates
                if(CheckRecipeCorrect(plateKitchenObject)){
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    DeliveryManager.Instance.CorrectRecipe();
                    currentRecipe = null;
                    startEating();
                    
                }
                else{
                    DeliveryManager.Instance.IncorrectRecipe();
                    player.GetKitchenObject().DestroySelf();
                    deliveryTimer -= 5;
                }
            }
        }
    }

    private bool CheckRecipeCorrect(PlateKitchenObject plateKitchenObject)
    {
        if (currentRecipe.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
            // Has the same number of ingredients
            bool plateContentsMatchesRecipe = true;
            foreach (KitchenObjectSO recipeKitchenObjectSO in currentRecipe.kitchenObjectSOList) {
                // Cycling through all ingredients in the Recipe
                bool ingredientFound = false;
                foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                    // Cycling through all ingredients in the Plate
                    if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                        // Ingredient matches!
                        ingredientFound = true;
                        break;
                    }
                }
                if (!ingredientFound) {
                    // This Recipe ingredient was not found on the Plate
                    plateContentsMatchesRecipe = false;
                }
            }

            if (plateContentsMatchesRecipe) {
                // Player delivered the correct recipe!
                return true;
            }
        }

        // No matches found!
        // Player did not deliver a correct recipe
        return false;
    }

    public void AssignRecipe(RecipeSO recipe)
    {
        currentRecipe = recipe;
        state = State.Waiting;
        ResetTimer();
    }

    private void ResetTimer()
    {
        deliveryTimer = deliveryTimeLimit;
    }
    private void startEating()
    {
        eatingTime = eatingTimeMax;
        state = State.Eating;
    }

    public bool HasRecipe(){
        return currentRecipe != null;
    }
    public RecipeSO GetCurrentRecipe(){
        return currentRecipe;
    }

}