using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerIconsUI : MonoBehaviour {


    [SerializeField] private DeliveryCounter deliveryCounter;
    [SerializeField] private Transform iconTemplate;


    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        deliveryCounter.OnRecipeAdded += deliveryCounter_OnRecipeAdded;
    }

    private void deliveryCounter_OnRecipeAdded(object sender, DeliveryCounter.OnRecipeAddedEventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in transform) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in deliveryCounter.GetCurrentRecipe().kitchenObjectSOList) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }

}