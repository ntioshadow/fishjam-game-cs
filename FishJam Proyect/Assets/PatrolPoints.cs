using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    public static PatrolPoints Instance { get; private set; }
    [SerializeField] private Transform[] points;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Patrol Point Controller instance");
        }
        Instance = this;
    }
    public Transform[] getPoints(){
        return points;
    }
}
