using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SceneManagement;

public class InvitationEvents : MonoBehaviour {

    // associated Text component to display the message.
    public Text message;

    // the invitation object being processed.
    private Invitation inv;

    private bool processed = false;
    private string inviterName = null;

    public GameEventsController gameEventController;

    // Update is called once per frame
    void Update()
    {

        inv = (inv != null) ? inv : InvitationManager.Instance.Invitation;
        if (inv == null && !processed)
        {
            Debug.Log("No Invite -- back to main");
            message.text = "No gufjg invites";
            //NavigationUtil.ShowMainMenu();
            return;
        }

        if (inviterName == null)
        {
            inviterName = (inv.Inviter == null || inv.Inviter.DisplayName == null) ? "Someone" :
      inv.Inviter.DisplayName;
            message.text = inviterName + " is challenging you!";
        }

        if (TicTacToeManager.Instance != null)
        {
            switch (TicTacToeManager.Instance.State)
            {
                case TicTacToeManager.GameState.Aborted:
                    Debug.Log("Aborted -- back to main");
                    gameEventController.SwitchingMenus(gameEventController.mainMenu);
                    break;
                case TicTacToeManager.GameState.Finished:
                    Debug.Log("Finished-- back to main");
                    gameEventController.SwitchingMenus(gameEventController.mainMenu);
                    break;
                case TicTacToeManager.GameState.Playing:
                    //NavigationUtil.ShowPlayingPanel();
                    SceneManager.LoadScene(1);
                    break;
                case TicTacToeManager.GameState.SettingUp:
                    message.text = "Setting up Race...";
                    break;
                case TicTacToeManager.GameState.SetupFailed:
                    Debug.Log("Failed -- back to main");
                    gameEventController.SwitchingMenus(gameEventController.mainMenu);
                    break;
            }
        }
    }

    // Handler script for the Accept button.  This method should be added
    // to the On Click list for the accept button.
    public void OnAccept()
    {

        if (processed)
        {
            return;
        }

        processed = true;
        InvitationManager.Instance.Clear();

        TicTacToeManager.AcceptInvitation(inv.InvitationId);
        Debug.Log("Accepted! RaceManager state is now " + TicTacToeManager.Instance.State);

    }

    // Handler script for the decline button.
    public void OnDecline()
    {

        if (processed)
        {
            return;
        }

        processed = true;
        InvitationManager.Instance.DeclineInvitation();

        gameEventController.SwitchingMenus(gameEventController.mainMenu);
    }
}
