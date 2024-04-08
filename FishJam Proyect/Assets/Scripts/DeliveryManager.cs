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
    private float spawnRecipeTimerMax = 20f;
    private int waitingRecipesMax = 2;
    private int successfulRecipesAmount;
    private int failedRecipesAmount;


    private void Awake() {
        Instance = this;
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (//KitchenGameManager.Instance.IsGamePlaying() && 
            waitingRecipe < customers.Length && waitingRecipe < waitingRecipesMax) {
                DeliveryCounter[] shuffledList = reshuffle(customers);
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                foreach (var customer in shuffledList) {
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

    DeliveryCounter[] reshuffle(DeliveryCounter[] array)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < array.Length; t++ )
        {
            DeliveryCounter tmp = array[t];
            int r = UnityEngine.Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
        return array;
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
        failedRecipesAmount++;
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
