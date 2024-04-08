using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {


    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;


    public static DeliveryManager Instance { get; private set; }


    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private DeliveryCounter[] customers;


    private int waitingRecipe;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 12f;
    private int waitingRecipesMax = 1;
    private int successfulRecipesAmount;


    private void Awake() {
        Instance = this;
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (//KitchenGameManager.Instance.IsGamePlaying() && 
            waitingRecipe < customers.Length) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                foreach (var customer in customers) {
                    if (!customer.HasRecipe()) {
                        customer.AssignRecipe(waitingRecipeSO);
                        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                        waitingRecipe += 1;
                        break;

                    }
                }
            }
        }
    }

    public int GetWaitingRecipe() {
        return waitingRecipe;
    }

    public int GetSuccessfulRecipesAmount() {
        return successfulRecipesAmount;
    }

    public void FailedRecipe(){
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        waitingRecipe -= 1;
        print("failed");
    }
    public void IncorrectRecipe(){
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    public void CorrectRecipe(){
        print("success");
        waitingRecipe -= 1;
        successfulRecipesAmount++;
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }

}
