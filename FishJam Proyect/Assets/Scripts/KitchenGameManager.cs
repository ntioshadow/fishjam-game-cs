using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour {


    public static KitchenGameManager Instance { get; private set; }



    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnLiveLoss;


    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }


    private State state;
    private float countdownToStartTimer = 3f;
    private float invulnerabilityTimer;
    private float invulnerabilityTimerMax = 5f;
    private bool isGamePaused = false;
    public int lives;
    public int maxLives;


    private void Awake() {
        Instance = this;

        state = State.WaitingToStart;
    }

    private void Start() {
        lives = maxLives;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        Player.Instance.OnPlayerEnemyCollision += Player_OnEnemyCollided;
    }

    private void Player_OnEnemyCollided(object sender, EventArgs e)
    {
        Hit();
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (state == State.WaitingToStart) {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        TogglePauseGame();
    }

    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f) {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                invulnerabilityTimer -= Time.deltaTime;
                if (lives <= 0) {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive() {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer() {
        return countdownToStartTimer;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    // public float GetGamePlayingTimerNormalized() {
    //     //return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    // }
    public void SetLives(int newLives){
        lives = newLives;
    }
    public int GetLives(){
        return lives;
    }
    public void Hit(){
        if(invulnerabilityTimer<=0 && state == State.GamePlaying){
            lives -= 1;
            invulnerabilityTimer = invulnerabilityTimerMax;
            OnLiveLoss?.Invoke(this, EventArgs.Empty);
        }
    }

    public void TogglePauseGame() {
        isGamePaused = !isGamePaused;
        if (isGamePaused) {
            Time.timeScale = 0f;

            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;

            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

}