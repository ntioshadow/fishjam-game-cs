using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI liveCounter;

    private void Start(){
        KitchenGameManager.Instance.OnLiveLoss += KitchenGameManager_OnLiveLoss;
    }

    private void KitchenGameManager_OnLiveLoss(object sender, EventArgs e)
    {
        liveCounter.text = KitchenGameManager.Instance.GetLives().ToString();
    }

}