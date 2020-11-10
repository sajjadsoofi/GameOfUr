using System.Collections.Generic;
using UnityEngine;

public delegate void EndActionHandler(); // Piece reached end point contract

public class Piece : MonoBehaviour
{

    public int playerIndex;
    int pieceIndex;
    Vector2 myplaceholder;
    bool isMoving;
    public bool isActive; // Piece enables collider only when active
    BoxCollider2D myCollider;
    public int currentTile; // -1: not on board
    Queue<Vector2> pathPoints = new Queue<Vector2>();
    Vector2 currentPoint;
    public event EndActionHandler OnPathEnded;


    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.enabled = false;
    }

    public void SetPiece(int _pcindex, int _plindex, Vector2 _place)
    {
        pieceIndex = _pcindex;
        playerIndex = _plindex;
        myplaceholder = _place;
        transform.position = myplaceholder;
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

    public void Action(Queue<Vector2> _pathPoints, int _targetTile, EndActionHandler _endaction)
    {
        currentTile = _targetTile;
        OnPathEnded = _endaction;

        // New points if empty
        if (pathPoints.Count < 1) pathPoints = _pathPoints;
        // Add points if not
        else foreach (Vector2 item in _pathPoints) pathPoints.Enqueue(item);

        transform.GetChild(0).GetComponent<Animator>().enabled = true;
        DequeueNextPoint();
    }

    void DequeueNextPoint()
    {
        // Unpack next target point
        if (pathPoints.Count > 0)
        {
            currentPoint = pathPoints.Dequeue();
            isMoving = true;
        }
        else
        {   // Last target reached
            currentPoint = Vector2.zero;
            transform.GetChild(0).GetComponent<Animator>().enabled = false;
            if (OnPathEnded != null) OnPathEnded();
        }
    }

    void Move()
    {
        Vector2 distanceToTarget = currentPoint - (Vector2)transform.position;
        Vector2 currentVelocity = distanceToTarget.normalized * (4 * Time.deltaTime);
        // Move towards target
        if (distanceToTarget.sqrMagnitude - currentVelocity.sqrMagnitude > 0.0001f)
        {
            transform.Translate(currentVelocity);
        }
        else
        {   // Reached target
            transform.position = currentPoint;
            isMoving = false;
            DequeueNextPoint();
        }
    }

    void Update()
    {
        if (isMoving) Move();
    }

    public void KillMe()
    {
        Deactivate();
        Queue<Vector2> q = new Queue<Vector2>();
        q.Enqueue(myplaceholder);
        Action(q, -1, null);
    }

    public void WinMe()
    {
        Deactivate();
        this.enabled = false;
    }

}
