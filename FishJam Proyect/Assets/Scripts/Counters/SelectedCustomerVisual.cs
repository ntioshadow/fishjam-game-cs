using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCustomerVisual : MonoBehaviour {


    [SerializeField] private DeliveryCounter deliveryCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    [SerializeField] private GameObject[] characterGameObjectArray;


    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if (e.selectedCounter == deliveryCounter) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(true);
            
        }
        if(deliveryCounter.HasRecipe()){
            foreach (GameObject visualGameObject in characterGameObjectArray) {
            visualGameObject.SetActive(true);
            
        }
        }
    }

    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(false);
        }
        if(deliveryCounter.HasRecipe()){
            foreach (GameObject visualGameObject in characterGameObjectArray) {
            visualGameObject.SetActive(false);
            
        }
        }
    }

}