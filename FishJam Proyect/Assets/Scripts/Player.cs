using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private GameInputs gameInputs;

    private void Update()
    {
        Vector2 inputVector = gameInputs.GetMovementVectorNormalized();
       
        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += movement * speed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, movement, Time.deltaTime * 10.0f);
    }
}

