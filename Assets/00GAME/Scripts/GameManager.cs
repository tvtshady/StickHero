using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    private static GameManager _instance;
    public static GameManager instance=>_instance;

    
    [SerializeField]
    float maxScale = 10, _speed = 2;
    [SerializeField]
    Transform rotateTranform, endRotateTransform;


    [SerializeField] PlayController controller;

    protected void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
        {
            _instance = this.GetComponent<GameManager>();
        }
        else if (instance.GetInstanceID() != this.GetInstanceID())
        {
            Destroy(this);
        }
        currentState = GAME_STATE.START;

    }

    // Start is called before the first frame update
    private int _score,_highScore,_yourScore;

    private GAME_STATE _currentState;

    public GAME_STATE currentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            Observer.instance.Notify(CONSTANT.gameState, _currentState);
        }
    }

    private PLAYER_STATE _currentPlayerState;

    public PLAYER_STATE currentPlayerState
    {
        get { return _currentPlayerState; }
        set
        {
            _currentPlayerState = value;
            Observer.instance.Notify(CONSTANT.playerState, _currentPlayerState);
        }
    }

    public int score
    {
        get { return _score; }
        set
        {
            if (value < 0)
                return;
            _score = value;
            Observer.instance.Notify(CONSTANT.score, _score);
        }
    }


    public int highscore
    {
        get { return _highScore; }
        set
        {
            if (value < 0)
                return;
            _highScore = value;
            Observer.instance.Notify(CONSTANT.highscore, _highScore);
        }
    }

    public int yourscore
    {
        get { return _yourScore; }
        set
        {
            if (value < 0)
                return;
            _yourScore = value;
            Observer.instance.Notify(CONSTANT.yourscore, _yourScore);
        }
    }

    /*public GAME_STATE currentState 
    { 
        get { return _currentState; }
        set { _currentState = value; }
    }*/

    public enum GAME_STATE
    {
        START,
        INPUT,
        GROWING,
        NONE,
        GAMEOVER
    }

    public enum PLAYER_STATE
    {
        IDLE,
        GROW,
        RUN,
        DIE,
        KICK
    }

    public void SetHighScore()
    {
        int high = PlayerPrefs.GetInt(CONSTANT.SaveHighScore);
        yourscore = _score;
        if (_score > high)
        {
            this.highscore = _score;
            PlayerPrefs.SetInt(CONSTANT.SaveHighScore, this.highscore);
        }
        else
            this.highscore = high;
    }

    public void Home()
    {
        currentState = GAME_STATE.START;
        currentPlayerState=PLAYER_STATE.IDLE;
        controller.CreateStartObjects();
    }

    public void ReStart()
    {
        currentState = GAME_STATE.INPUT;
        currentPlayerState = PLAYER_STATE.IDLE;
        controller.CreateStartObjects();
        controller.CreatePlatform();
    }


    public void StartGame()
    {
        currentState = GAME_STATE.INPUT;
        controller.CreateStartObjects();
        controller.CreatePlatform();
    }
    


}
