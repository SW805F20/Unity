using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GoalUiRenderer : MonoBehaviour
{
    public GameStateHandler gameStateHandler;
    private RectTransform _rectTransform;
    private FieldGenerator _fieldGen;
    bool isVertical = false;

    Text team1Score;
    Text team2Score;
    [SerializeField]
    GameObject verticalGoalContainer;
    [SerializeField]
    GameObject horizontalGoalContainer;

    private TCPClient _tcpclient;

    // Start is called before the first frame update
    void Start()
    {
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
        _fieldGen = gameStateHandler.playingFieldObject.GetComponent<FieldGenerator>();
        _rectTransform = GetComponent<RectTransform>();
        _fieldGen.OnFieldCreated += CreateField;
        _tcpclient = GameObject.Find("ConnectionHandler").GetComponent<TCPClient>();
        _tcpclient.OnGoalScored += UpdateGoalScore;

        CreateField(gameStateHandler.anchor1, gameStateHandler.anchor2, gameStateHandler.anchor3, gameStateHandler.anchor4);
    }

    void GetTeamScoreComponents(GameObject container)
    {
        team1Score = container.transform.GetChild(0).GetComponent<Text>();
        team2Score = container.transform.GetChild(1).GetComponent<Text>();
    }

    private void CreateField(Vector3 anchor1, Vector3 anchor2, Vector3 anchor3, Vector3 anchor4)
    {
        float minx = Mathf.Min(anchor1.x, anchor2.x, anchor3.x, anchor4.x);
        float miny = Mathf.Min(anchor1.y, anchor2.y, anchor3.y, anchor4.y);

        float maxx = Mathf.Max(anchor1.x, anchor2.x, anchor3.x, anchor4.x);
        float maxy = Mathf.Max(anchor1.y, anchor2.y, anchor3.y, anchor4.y);

        float height = maxy - miny;
        float width = maxx - minx;

        if(height > width)
        {
            isVertical = true;
        }

        if (isVertical)
        {
            verticalGoalContainer.SetActive(true);
            GetTeamScoreComponents(verticalGoalContainer);
        }
        else
        {
            horizontalGoalContainer.SetActive(true);
            GetTeamScoreComponents(horizontalGoalContainer);
        }
    }
    private void UpdateGoalScore()
    {
        team1Score.text = gameStateHandler.team1Score.ToString();
        team2Score.text = gameStateHandler.team2Score.ToString();
    }
}
