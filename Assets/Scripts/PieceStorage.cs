using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceStorage : MonoBehaviour
{
    public GameObject piecePrefab;
    public Tile startingTile;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject thePiece = Instantiate(piecePrefab);
            thePiece.GetComponent<PlayerPiece>().startingTile = this.startingTile;
            thePiece.GetComponent<PlayerPiece>().myPieceStorage = this;

            AddPieceToStorage(thePiece, this.transform.GetChild(i) );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPieceToStorage(GameObject thePiece , Transform thePlaceHolder = null)
    {
        if (thePlaceHolder == null)
        {
            // Find the first empty placeHolder
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Transform p = this.transform.GetChild(i);
                if (p.childCount == 0)
                {
                    thePlaceHolder = p;
                    break;
                }
            }

            if (thePlaceHolder == null)
            {
                Debug.LogError("[PieceStorage.cs] this should not happen!");
                return;
            }
        }

        // parent the piece to the placeHolder
        thePiece.transform.SetParent(thePlaceHolder);
        // REset the piece's local position to 0,0,0
        thePiece.transform.localPosition = Vector2.zero;

    }
}
