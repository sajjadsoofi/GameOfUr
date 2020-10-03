using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceTotalDisplay : MonoBehaviour
{
    public DiceRoller diceRoller;

    // Update is called once per frame
    void Update()
    {
        if (diceRoller.isDoneRolling == false)
        {
            GetComponent<Text>().text = "= ?";
        }
        else
        {
            GetComponent<Text>().text = "= " + diceRoller.diceTotal;
        }

        
    }
}
