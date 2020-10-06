using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceTotalDisplay : MonoBehaviour
{
    StateManager stateManager;

    private void Start()
    {
        stateManager = GameObject.FindObjectOfType<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateManager.isDoneRolling == false)
        {
            GetComponent<Text>().text = "= ?";
        }
        else
        {
            GetComponent<Text>().text = "= " + stateManager.diceTotal;
        }

        
    }
}
