using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{

    public int[] diceValues;
    public Sprite[] diceImageOne;
    public Sprite[] diceImageZero;
    public int diceTotal;

    public bool isDoneRolling = false;

    // Start is called before the first frame update
    void Start()
    {
        diceValues = new int[4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewTurn()
    {
        //The Start Of Player's turn

        isDoneRolling = false;
    }

    public void RollTheDice()
    {
        diceTotal = 0;
        for (int i = 0; i < diceValues.Length; i++)
        {
            diceValues[i] = Random.Range(0, 2);
            diceTotal += diceValues[i];

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


            isDoneRolling = true;
        }
    }
}
