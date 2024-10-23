using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//pathing from here: https://www.youtube.com/watch?v=RuvfOl8HhhM

public class EnemyController : MonoBehaviour
{
    Transform player;
    bool isPlayerDetected;

    Vector3 spawnPosition;
    [SerializeField] float movementSpeed = 3;

    Vector3 desiredDestination;
    Vector3 leftmostPath;
    Vector3 rightmostPath;
    [SerializeField] float leftmostPosition;
    [SerializeField] float rightmostPosition;

    [SerializeField] float searchTime = 3;

    public State currentState = State.Idle;

    [System.Serializable]
    public enum State { Idle, Attack, Searching, Return }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        spawnPosition = transform.position;
        leftmostPath = new Vector3(spawnPosition.x - leftmostPosition, spawnPosition.y, spawnPosition.z);
        rightmostPath = new Vector3(spawnPosition.x + rightmostPosition, spawnPosition.y, spawnPosition.z);

        desiredDestination = rightmostPath;
    }

    void Update()
    {
        isPlayerDetected = gameObject.GetComponent<EnemyDetector>().PlayerDetected;

        switch (currentState)
        {
            case State.Idle:
                //Default path
                if (isPlayerDetected)
                {
                    currentState = State.Attack;
                }
                break;

            case State.Attack:
                //When the player is within the Ant's range of sight, the ant will follow the player
                //The ant will try to touch the player in an attempt to damage the player
                searchTime = 3;
                if (isPlayerDetected)
                {
                    //TO DO:
                    //if enemy collision and player collision touch
                    //AND player is not attacking, player takes damage
                }
                else
                {
                    currentState = State.Searching;
                }
                break;

            case State.Searching:
                //Enemy is going to stand still for a couple of seconds to check if the player is still nearby
                //If the player does not re-enter the enemy's line of sight, they will return to where they were prior
                //To the player first encountering them.
                if (searchTime > 0)
                {
                    if (isPlayerDetected)
                    {
                        currentState = State.Attack;
                    }

                    searchTime -= Time.deltaTime;
                }
                else
                {
                    currentState = State.Return;
                }
                break;

            case State.Return:
                searchTime = 3;

                //Ant will return to where they were before detecting the player
                if (isPlayerDetected)
                {
                    currentState = State.Attack;
                }
                else
                {
                    if (transform.position == spawnPosition)
                    {
                        currentState = State.Idle;
                    }
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isPlayerDetected)
        {
            //TO DO:
            //once i actually make a spritesheet for the enemies,
            //we can use transform.localScale here to flip the sprites towards the player
            if (transform.position.x < player.position.x)
            {
                
            }
            else
            {
                
            }

            this.transform.position = Vector2.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
        }

        if (currentState == State.Idle)
        {
            if (desiredDestination == leftmostPath)
            {
                this.transform.position = Vector2.MoveTowards(transform.position, leftmostPath, movementSpeed * Time.deltaTime);
            }
            else
            {
                this.transform.position = Vector2.MoveTowards(transform.position, rightmostPath, movementSpeed * Time.deltaTime);
            }


            if (Vector2.Distance(transform.position, desiredDestination) < 1 && desiredDestination == rightmostPath)
            {
                desiredDestination = leftmostPath;
            }

            if (Vector2.Distance(transform.position, desiredDestination) < 1 && desiredDestination == leftmostPath) 
            {
                desiredDestination = rightmostPath;
            }
        }

        if (currentState == State.Return)
        {
            this.transform.position = Vector2.MoveTowards(transform.position, spawnPosition, movementSpeed * Time.deltaTime);
        }
    }
}