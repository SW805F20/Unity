using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinCheck : MonoBehaviour
{
    public bool stop = false;
    GameStateHandler gameStateHandler;
    TCPClient _tcpclient;
    [SerializeField]
    Text endGameText;
    // Start is called before the first frame update
    void Start()
    {
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
        _tcpclient = GameObject.Find("ConnectionHandler").GetComponent<TCPClient>();
        _tcpclient.OnGoalScored += CheckWinCondition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            IncrementScore();
        }
            
            CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if(gameStateHandler.team1Score == gameStateHandler.goalsToWin || gameStateHandler.team2Score == gameStateHandler.goalsToWin)
        {
            gameStateHandler.team1Score = 0;
            gameStateHandler.team2Score = 0;
            endGameText.gameObject.SetActive(true);
            StartCoroutine("NewGameDelay");
            
        }
    }

    IEnumerator NewGameDelay()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("StartMenu");
    }

    //This is just a pepega hardcoded thing for test. Will be removed before merge.
    void IncrementScore()
    {
        if(gameStateHandler.team2Score < 101)
        {
            gameStateHandler.team2Score++;
        }
        if (gameStateHandler.team2Score == 100)
        {
            stop = true;
        }
        

    }
}
