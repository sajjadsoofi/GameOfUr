using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{

    public int[] diceValues;
    public Sprite[] diceImageOne;
    public Sprite[] diceImageZero;

    StateManager theStateMAnager;

    // Start is called before the first frame update
    void Start()
    {
        theStateMAnager = GameObject.FindObjectOfType<StateManager>();
        diceValues = new int[4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void RollTheDice()
    {
        if (theStateMAnager.isDoneRolling == true)
        {
            return;
        }

        theStateMAnager.diceTotal = 0;
        for (int i = 0; i < diceValues.Length; i++)
        {
            diceValues[i] = Random.Range(0, 2);
            theStateMAnager.diceTotal += diceValues[i];

            if (diceValues[i] == 0)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    diceImageZero[Random.Range(0, diceImageZero.Length)];
            }
            else
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    diceImageOne[Random.Range(0, diceImageOne.Length)];
            }

            
        }
        theStateMAnager.diceTotal = 15;
        theStateMAnager.isDoneRolling = true;
        theStateMAnager.CheckLegalMoves();
    }
}
