using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {


    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }


    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    [SerializeField] private RecipeListSO recipeListSO;


    private List<KitchenObjectSO> kitchenObjectSOList;
    private Boolean hasCompleteRecipe = false;
    private RecipeSO completedDish;


    private void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {
        print("TryAdd");
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) {
            // Not a valid ingredient
            print("Not a valid ingredient");
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            // Already has this type
            print("Already has this type");
            return false;
        } if (hasCompleteRecipe){
            print("Cannot fit more");
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            hasCompleteRecipe = checkRecipes();

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO,
            });

            
            print("Added");
            return true;
        }
    }

    private bool checkRecipes()
    {
        foreach (RecipeSO currentRecipe in recipeListSO.recipeSOList)
        {
            if (currentRecipe.kitchenObjectSOList.Count == kitchenObjectSOList.Count)
            {
                // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in currentRecipe.kitchenObjectSOList)
                {
                    // Cycling through all ingredients in the Recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in kitchenObjectSOList)
                    {
                        // Cycling through all ingredients in the Plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This Recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe!
                    completedDish = currentRecipe;
                    return true;
                }
            }

            // No matches found!
            // Player did not deliver a correct recipe
        }
        return false;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return kitchenObjectSOList;
    }

    public KitchenObjectSO GetFinalRecipe(){
        return completedDish.result;
    }

    public Boolean HasCompleted(){
        return hasCompleteRecipe;
    }


}