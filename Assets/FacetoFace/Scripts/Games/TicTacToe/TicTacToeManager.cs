using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


public class TicTacToeManager : RealTimeMultiplayerListener {

    const int MinOpponents = 1;
    const int MaxOpponents = 1;
    const int GameVariant = 0;  // default
    public Text text;
    private bool showingWaitingRoom = false;
    public string player1;
    public string player2;
    bool myTurn;
    int turn;
    public Transform board;

    static TicTacToeManager sInstance = null;

    public enum GameState
    {
        SettingUp,
        Playing,
        Finished,
        SetupFailed,
        Aborted
    };

    private GameState mGameState = GameState.SettingUp;

    public string mMyParticipantId = "";

    private float mRoomSetupProgress = 0.0f;

    const float FakeProgressSpeed = 1.0f;
    const float MaxFakeProgress = 30.0f;
    float mRoomSetupStartTime = 0.0f;

    private TicTacToeManager()
    {
        mRoomSetupStartTime = Time.time;
    }

    public static void CreateQuickGame()
    {
        sInstance = new TicTacToeManager();
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents,
            GameVariant, sInstance);
    }

    public static void CreateWithInvitationScreen()
    {
        sInstance = new TicTacToeManager();
        PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents,
            GameVariant, sInstance);
    }

    public static void AcceptFromInbox()
    {
        sInstance = new TicTacToeManager();
        PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(sInstance);
    }

    public static void AcceptInvitation(string invitationId)
    {
        sInstance = new TicTacToeManager();
        PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitationId, sInstance);
    }

    public GameState State
    {
        get
        {
            return mGameState;
        }
    }

    public static TicTacToeManager Instance
    {
        get
        {
            return sInstance;
        }
    }

    public float RoomSetupProgress
    {
        get
        {
            float fakeProgress = (Time.time - mRoomSetupStartTime) * FakeProgressSpeed;
            if (fakeProgress > MaxFakeProgress)
            {
                fakeProgress = MaxFakeProgress;
            }
            float progress = mRoomSetupProgress + fakeProgress;
            return progress < 99.0f ? progress : 99.0f;
        }
    }

    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            mGameState = GameState.Playing;
            mMyParticipantId = GetSelf().ParticipantId;
            SceneManager.LoadScene(1);
            //SetupTrack();
            SetupPlayers();
        }
        else
        {
            mGameState = GameState.SetupFailed;
        }
    }

    public void OnLeftRoom()
    {
        if (mGameState != GameState.Finished)
        {
            mGameState = GameState.Aborted;
        }
    }

    public void OnPeersConnected(string[] peers)
    {
    }

    public void OnParticipantLeft(Participant participant)
    {
    }

    public void OnPeersDisconnected(string[] peers)
    {
        foreach (string peer in peers)
        {
            // if this peer has left and hasn't finished the game,
            // consider them to have abandoned the game
            /*mGotFinalScore.Add(peer);
            mRacerScore[peer] = 0;
            RemoveCarFor(peer);*/
        }

        // if, as a result, we are the only player in the race, it's over
        List<Participant> racers = GetRacers();
        if (mGameState == GameState.Playing && (racers == null || racers.Count < 2))
        {
            mGameState = GameState.Aborted;
        }
    }

    /*public void OnRoomSetupProgress(float percent)
    {
        mRoomSetupProgress = percent;
    }*/

    public void OnRoomSetupProgress(float progress)
    {
        // show the default waiting room.
        if (!showingWaitingRoom)
        {
            showingWaitingRoom = true;
            PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        }
    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        /*int score = (int)data[1];

        if (data[0] == (byte)'I')
        {
            // interim score update
            mRacerScore[senderId] = score;
        }
        else if (data[0] == (byte)'F')
        {
            // finish notification
            if (!mGotFinalScore.Contains(senderId))
            {
                // record final score
                mRacerScore[senderId] = score;
                mGotFinalScore.Add(senderId);
                UpdateMyRank();

                // finish race too, if we haven't yet
                if (mRaceState == RaceState.Playing)
                {
                    FinishRace();
                }
            }
            else
            {
                Debug.LogWarning("Received duplicate finish notification for " + senderId);
            }
        }*/
    }

    public void CleanUp()
    {
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        mGameState = GameState.Aborted;
        sInstance = null;
        //TearDownTrack();
    }

    public Participant GetSelf()
    {
        return PlayGamesPlatform.Instance.RealTime.GetSelf();
    }

    public List<Participant> GetRacers()
    {
        return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
    }

    public Participant GetParticipant(string participantId)
    {
        return PlayGamesPlatform.Instance.RealTime.GetParticipant(participantId);
    }

    void SetupPlayers()
    {
        List<Participant> racers = GetRacers();
        player1 = racers[0].ParticipantId;
        player2 = racers[1].ParticipantId;
    }

    public void SetupBoard()
    {
        foreach (Button go in board)
        {
            if (myTurn)
            {
                if (go.GetComponentInChildren<Text>().text == "")
                {
                    go.interactable = true;
                }
            }
            else
            {
                go.interactable = false;
            }
        }
    }

    public void OnButtonClicked(Button button)
    {
        if (turn == 1)
        {
            button.GetComponentInChildren<Text>().text = "X";
            ManageTurns(2);
        }
        else
        {
            button.GetComponentInChildren<Text>().text = "O";
            ManageTurns(1);
        }
    }

    public void ManageTurns(int nextTurn)
    {
        turn = nextTurn;
        
        if(turn == 1){
            if (mMyParticipantId == player1)
            {
                myTurn = true;
            }
            else
            {
                myTurn = false;
            }
        }
        if (turn == 2)
        {
            if (mMyParticipantId == player2)
            {
                myTurn = true;
            }
            else
            {
                myTurn = false;
            }
        }
        SetupBoard();
    }
}
