using System.Collections;
using System.Collections.Generic;
using FiroozehGameService.Handlers;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas gameUI;
    [SerializeField]
    private Canvas loginUI;
    [SerializeField]
    private Canvas startUI;
    
    // Start is called before the first frame update
    void Start()
    {
        var menuController = GetComponentInChildren<LoginMenuController>();
        menuController.Login += MenuControllerOnLogin;

        var startController = GetComponentInChildren<StartMenuController>();
        startController.ReadyToPlay += StartControllerOnReadyToPlay;
    }

    private void StartControllerOnReadyToPlay()
    {
        loginUI.enabled = false;
        startUI.enabled = false;
    }

    private void MenuControllerOnLogin()
    {
        loginUI.enabled = false;
        startUI.enabled = true;
    }

}
