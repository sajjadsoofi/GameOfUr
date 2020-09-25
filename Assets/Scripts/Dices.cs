using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dices : MonoBehaviour
{
    public int diceNumber = 0;
    public void RandomGenerate()
    {
        diceNumber = Random.Range(0, 5);
        gameObject.GetComponent<Text>().text = "" + diceNumber;
    }
}
