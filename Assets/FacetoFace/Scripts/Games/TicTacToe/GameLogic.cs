using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    public Transform board;
    public Text participantID;
    public Text participant2ID;
    public Text playerID;

	// Use this for initialization
	void Start () {
        TicTacToeManager.Instance.board = board;
        TicTacToeManager.Instance.ManageTurns(1);
	}

    public void OnButtonClicked(Button button)
    {
        TicTacToeManager.Instance.OnButtonClicked(button);
    }

    void Update()
    {
        participantID.text = TicTacToeManager.Instance.player1;
        participant2ID.text = TicTacToeManager.Instance.player2;
        playerID.text = TicTacToeManager.Instance.GetSelf().ParticipantId;
    }
}
