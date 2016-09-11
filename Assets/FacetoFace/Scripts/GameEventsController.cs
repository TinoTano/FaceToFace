using UnityEngine;
using System.Collections;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine.UI;

public class GameEventsController : MonoBehaviour
{

    private System.Action<bool> mAuthCallback;
    private bool mAuthOnStart = true;
    private bool mSigningIn = false;
    private bool firstEnter = true;

    public Button signInButton;
    public Button signOutButton;
    public Button quickMatchButton;
    public Button invitePlayerButton;
    public Button inboxInvitesButton;
    public Text signMessageText;
    public GameObject mainMenu;
    public GameObject loggingMenu;
    public GameObject invitationMenu;
    private GameObject currentMenu = null;


    // Use this for initialization
    void Start()
    {
        currentMenu = loggingMenu;

        mAuthCallback = (bool success) =>
        {
            Debug.Log("In Auth callback, success = " + success);
            mSigningIn = false;
            if (success)
            {
                signInButton.interactable = true;
                signMessageText.text = "";
                Debug.Log("Auth succes!!");
                SwitchingMenus(mainMenu);
            }
            else
            {
                if (!firstEnter)
                {
                    signMessageText.text = "Sign in failed...";
                    signInButton.interactable = true;
                }
                
                Debug.Log("Auth failed!!");
            }

        };

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .WithInvitationDelegate(InvitationManager.Instance.OnInvitationReceived)
        .Build();
        PlayGamesPlatform.InitializeInstance(config);

        // try silent authentication
        if (mAuthOnStart)
        {
            Authorize(true);
        }
    }

    // This starts a "non-silent" login process.
    public void OnSignInClicked()
    {
        signMessageText.text = "Signing in...";
        signInButton.interactable = false;
        firstEnter = false;
        Authorize(false);
    }

    //Starts the signin process.
    void Authorize(bool silent)
    {
        if (!mSigningIn)
        {
            Debug.Log("Starting sign-in...");
            PlayGamesPlatform.Instance.Authenticate(mAuthCallback, silent);
        }
        else
        {
            Debug.Log("Already started signing in");
            signMessageText.text = "Already started signing in!";
        }
    }

    public void SwitchingMenus(GameObject newActiveMenu)
    {
        newActiveMenu.gameObject.SetActive(true);
        currentMenu.gameObject.SetActive(false);
        currentMenu = newActiveMenu;
    }
}
