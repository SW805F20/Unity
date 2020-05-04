using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalUiRenderer : MonoBehaviour
{
    [SerializeField]
    GameObject gameState;
    GameStateHandler gameStateHandler;
    private RectTransform _rectTransform;
    private FieldGenerator _fieldGen;

    [SerializeField]
    GameObject team1Score;
    [SerializeField]
    GameObject team2Score;



    // Start is called before the first frame update
    void Start()
    {
        gameStateHandler = gameState.GetComponent<GameStateHandler>();
        _fieldGen = gameStateHandler.playingFieldObject.GetComponent<FieldGenerator>();
        _rectTransform = GetComponent<RectTransform>();
        _fieldGen.OnFieldCreated += OnFieldCreated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnFieldCreated(Vector3 anchor1, Vector3 anchor2, Vector3 anchor3, Vector3 anchor4)
    {
        bool isVertical = false;
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



    }
}
