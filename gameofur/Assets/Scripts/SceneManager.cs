using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    int currentPlayer; //   0: player_1     1: player_2
    int diceValue; // 0, 1, 2, 3, 4
    public float diceDelayTime; // diceval == 0
    public Player[] myPlayers;
    public int[] scores;
    public Camera mainCamera;
    public static SceneManager instance;

    ////////// UI //////////
    public Button dice;
    public Text diceValueText;
    public Text playerTurnText;
    public Text[] scoreText;

    void Start()
    {
        currentPlayer = 0;
        scores = new int[myPlayers.Length];

        for (int i = 0; i < myPlayers.Length; i++)
        {
            myPlayers[i].SetPlayer(i);
            scores[i] = 0;
        }

        instance = this;
    }

    public void OnDiceRolled()
    {
        dice.interactable = false;
        diceValue = Random.Range(0, 5);
        // diceValue = Random.Range(0, 2);
        diceValueText.text = diceValue.ToString();
        if (diceValue == 0) OnSwitchTurnHandler(true);
        else myPlayers[currentPlayer].CheckForOptions(diceValue);
    }

    public void OnSwitchTurnHandler(bool delayed)
    {
        if (delayed) StartCoroutine(ActivateDiceSwitchDelayed(diceDelayTime));
        else StartCoroutine(ActivateDiceSwitchDelayed(0.1f));
    }

    IEnumerator ActivateDiceSwitchDelayed(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        currentPlayer = 1 - currentPlayer;
        playerTurnText.text = (currentPlayer + 1).ToString();
        ActivateDice();
    }

    public void OnSameTurnHandler(bool delayed)
    {
        if (delayed) StartCoroutine(ActivateDiceSameDelayed(diceDelayTime));
        else StartCoroutine(ActivateDiceSameDelayed(0.1f));
    }

    IEnumerator ActivateDiceSameDelayed(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        Debug.Log("Roll Again!");
        ActivateDice();
    }

    void ActivateDice()
    {
        dice.interactable = true;
        diceValueText.text = "-";
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Piece piece = hit.transform.GetComponent<Piece>();
                if (piece != null)
                {
                    MoveThisPiece(currentPlayer, piece.GetPieceIndex(), diceValue);
                }
            }
        }
    }

    void MoveThisPiece(int _playerIndex, int _pieceIndex, int _diceval)
    {
        if (!myPlayers[_playerIndex].IsThisPieceActive(_pieceIndex)) return;
        myPlayers[_playerIndex].NavigateThisPiece(_pieceIndex, _diceval);
    }

    public void GrantOneScorePoint(int _playerIndex)
    {
        scores[_playerIndex] += 1;
        scoreText[_playerIndex].text = scores[_playerIndex].ToString();
        if (scores[_playerIndex] > 6)
        {   // On game end
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

}
