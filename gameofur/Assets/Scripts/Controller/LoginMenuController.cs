using System;
using System.Threading.Tasks;
using FiroozehGameService.Core;
using FiroozehGameService.Models;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class LoginMenuController : MonoBehaviour
{
    public event Action Login;
    
    public InputField NickName, Email, PassWord;

    public Button Submit;

    public GameObject Switch;

    // Start is called before the first frame update
    private async void Start()
    {
        //Debug.Log(Application.persistentDataPath);
        SetEventHandler(); 
        await ConnectToGameService();
    }
    
    private void SetEventHandler()
    {
    }

    private async Task ConnectToGameService()
    {
        try
        {
            if (FileUtil.IsLoginBefore())
            {
                var userToken = FileUtil.GetUserToken();
                await GameService.Login(userToken);
                Login?.Invoke();
            }
            else
            {
                gameObject.GetComponent<Canvas>().enabled = true;
                
                Switch.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (NickName.IsActive())
                    {
                        NickName.gameObject.SetActive(false);
                        Switch.GetComponent<Text>().text = "SignUp";
                        Submit.gameObject.GetComponentInChildren<Text>().text = "Login";
                    }
                    else
                    {
                        NickName.gameObject.SetActive(true);
                        Switch.GetComponent<Text>().text = "Login";
                        Submit.gameObject.GetComponentInChildren<Text>().text = "SignUp";
                    }
                });

                Submit.onClick.AddListener(async () =>
                {
                    var nickName = NickName.text.Trim();
                    var email = Email.text.Trim();
                    var passWord = PassWord.text.Trim();

                    if (NickName.IsActive())
                    {
                        var userToken = await GameService.SignUp(nickName, email, passWord);
                        FileUtil.SaveUserToken(userToken);
                        Login?.Invoke();
                    }
                    else
                    {
                        var userToken = await GameService.Login(email, passWord);
                        FileUtil.SaveUserToken(userToken);
                        Debug.Log("Login Successful!");
                        Login?.Invoke();
                    }
                });
            }
        }
        catch (Exception e)
        {
            if (e is GameServiceException) Debug.LogError("GameService exception :" + e.Message);
            else Debug.LogError("Internal exception :" + e);
        }
    }
}
