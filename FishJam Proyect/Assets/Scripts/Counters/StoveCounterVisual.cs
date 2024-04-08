using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {


    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;

    [SerializeField] private GameObject stoveOffGameObject;
    //[SerializeField] private GameObject particlesGameObject;


    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool showVisual = e.state == StoveCounter.State.Frying;
        stoveOnGameObject.SetActive(showVisual);
        stoveOffGameObject.SetActive(!showVisual);
        //particlesGameObject.SetActive(showVisual);
    }

}