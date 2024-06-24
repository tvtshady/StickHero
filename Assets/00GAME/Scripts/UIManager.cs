using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _score, yourScore, highScore;
    [SerializeField]
    Button btnStart;
    [SerializeField]
    Image pnlGameOver;
    void Start()
    {
        Observer.instance.AddListener(CONSTANT.score, UpdateScore);
        Observer.instance.AddListener(CONSTANT.gameState, UpdatePanel);
        Observer.instance.AddListener(CONSTANT.highscore, UpdateHScore);
        Observer.instance.AddListener(CONSTANT.yourscore, UpdateYScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScore(object score)
    {
        _score.text=score.ToString();
    }
    void UpdateHScore(object score)
    {
        highScore.text = score.ToString();
    }
    void UpdateYScore(object score)
    {
        yourScore.text = score.ToString();
    }

    void UpdatePanel(object gameState)
    {
        GameManager.GAME_STATE state= (GameManager.GAME_STATE)gameState;
        switch (state)
        {
            case GameManager.GAME_STATE.START:
                btnStart.gameObject.SetActive(true);
                _score.gameObject.SetActive(false);
                pnlGameOver.gameObject.SetActive(false);
                GameManager.instance.score = 0;
                break;
            case GameManager.GAME_STATE.GAMEOVER:
                btnStart.gameObject.SetActive(false);
                _score.gameObject.SetActive(false);
                pnlGameOver.gameObject.SetActive(true);
                GameManager.instance.SetHighScore();
                break;
            case GameManager.GAME_STATE.INPUT:
                btnStart.gameObject.SetActive(false);
                _score.gameObject.SetActive(true);
                pnlGameOver.gameObject.SetActive(false);
                
                break;
        }
    }

    public void StartGame()
    {
        GameManager.instance.StartGame();
        AudioManager.instance.PlaySound(AudioManager.AUDIO_TYPE.BUTTON);
    }

    public void HomeButton()
    {
        GameManager.instance.Home();
        AudioManager.instance.PlaySound(AudioManager.AUDIO_TYPE.BUTTON);
    }

    public void ReStartButton()
    {
        GameManager.instance.ReStart();
        GameManager.instance.score = 0;
        AudioManager.instance.PlaySound(AudioManager.AUDIO_TYPE.BUTTON);
    }




}
