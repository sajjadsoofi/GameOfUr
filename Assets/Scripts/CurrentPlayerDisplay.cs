using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerDisplay : MonoBehaviour
{
    Text turnText;
    StateManager stateManager;

    string[] numberWords = { "One", "Two" };

    // Start is called before the first frame update
    void Start()
    {
        turnText = GetComponent<Text>();
        stateManager = GameObject.FindObjectOfType<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        turnText.text = "Current Player " + numberWords[stateManager.currentPlayerID];
    }
}
