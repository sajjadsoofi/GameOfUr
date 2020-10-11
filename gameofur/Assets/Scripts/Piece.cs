﻿using System.Collections.Generic;
using UnityEngine;

public delegate void PieceReachedHandler(); // Piece reached end point contract

public class Piece : MonoBehaviour
{

    public int playerIndex;
    int pieceIndex;
    bool isMoving;
    public bool isActive; // Piece enables collider only when active
    BoxCollider2D myCollider;
    public int currentTile; // -1: not on board
    Queue<Vector2> pathPoints = new Queue<Vector2>();
    Vector2 currentPoint;
    public event PieceReachedHandler OnPathEnded;


    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.enabled = false;
    }

    public void SetPiece(int _pcindex, int _plindex)
    {
        pieceIndex = _pcindex;
        playerIndex = _plindex;
    }

    public int GetPieceIndex()
    {
        return pieceIndex;
    }

    public void Activate()
    {
        isActive = true;
        myCollider.enabled = true;
    }
    public void Deactivate()
    {
        isActive = false;
        myCollider.enabled = false;
    }

    public void Action(Queue<Vector2> _pathPoints, int _targetTile)
    {
        currentTile = _targetTile;

        // New points if empty
        if (pathPoints.Count < 1) pathPoints = _pathPoints;
        // Add points if not
        else foreach (Vector2 item in _pathPoints) pathPoints.Enqueue(item);

        transform.GetChild(0).GetComponent<Animator>().enabled = true;
        isMoving = true;
        DequeueNextPoint();
    }

    void DequeueNextPoint()
    {
        // Unpack next target point
        if (pathPoints.Count > 0)
        {
            currentPoint = pathPoints.Dequeue();
        }
        else
        {   // Last target reached
            currentPoint = Vector2.zero;
            transform.GetChild(0).GetComponent<Animator>().enabled = false;
            isMoving = false;
            if (OnPathEnded != null)
            {
                OnPathEnded();
                OnPathEnded = null;
            }
        }
    }

    void Move()
    {
        Vector2 distanceToTarget = currentPoint - (Vector2)transform.position;
        Vector2 currentVelocity = distanceToTarget.normalized * 1.5f * Time.deltaTime;
        // Move towards target
        if (distanceToTarget.magnitude > currentVelocity.magnitude)
        {
            transform.Translate(currentVelocity);
        }
        else
        {   // Reached target
            transform.position = currentPoint;
            DequeueNextPoint();
        }
    }

    void Update()
    {
        if (isMoving) Move();
    }

}