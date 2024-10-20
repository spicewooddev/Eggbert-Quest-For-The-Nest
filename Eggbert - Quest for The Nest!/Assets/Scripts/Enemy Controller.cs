using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform player;
    bool isPlayerDetected;

    Vector3 spawnPosition;
    [SerializeField] float movementSpeed = 3;

    private bool canAttack = false;
    private bool isSearching = false;

    public State currentState = State.Idle;

    [System.Serializable]
    public enum State { Idle, Attack, Searching, Return }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isPlayerDetected = GetComponent<EnemyDetector>().enemyControllerCheck;
        spawnPosition = transform.position;
    }

    void Update()
    {
        /*
        if (currentState != State.Idle)
        {
            
        }
        */

        switch (currentState)
        {
            case State.Idle:
                //Default path

                //TO DO:
                //Ant is meant to walk back and forth in a designated path. haven't coded this yet
                Debug.Log(isPlayerDetected);
                if (isPlayerDetected)
                {
                    currentState = State.Attack;
                }
                break;

            case State.Attack:
                canAttack = true;
                //When the player is within the Ant's range of sight, the ant will follow the player
                //The ant will try to touch the player in an attempt to damage the player
                if (isPlayerDetected)
                {
                    Debug.Log("Enemy has sighted Player!");
                    canAttack = true;
                }
                else
                {
                    canAttack = false;
                    isSearching = true;

                    Invoke(nameof(State.Searching), 3.0f);

                    currentState = State.Return;
                }
                break;

            case State.Searching:
                canAttack = false;
                //Enemy is going to stand still for a couple of seconds to check if the player is still nearby
                //If the player does not re-enter the enemy's line of sight, they will return to where they were prior
                //To the player first encountering them.
                if (isSearching)
                {
                    Debug.Log("Enemy is Searching for Player!");
                    if (isPlayerDetected)
                    {
                        isSearching = false;
                        currentState = State.Attack;
                    }
                }
                break;

            case State.Return:
                canAttack = false;
                isSearching = false;
                //Ant will return to where they were before detecting the player
                if (isPlayerDetected)
                {
                    currentState = State.Attack;
                }
                else
                {
                    Debug.Log("Enemy is now Returning to Start Position!");
                    if (transform.position == spawnPosition)
                    {
                        currentState = State.Idle;
                        Debug.Log("Enemy is now Idle again!");
                    }
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isPlayerDetected)
        {
            if (transform.position.x < player.position.x)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.position, -movementSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.position, movementSpeed * Time.deltaTime);
            }
        }

        if (currentState == State.Return && !canAttack)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, spawnPosition, movementSpeed * Time.deltaTime);
        }
    }
}