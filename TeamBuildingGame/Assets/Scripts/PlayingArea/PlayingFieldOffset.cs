using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayingFieldOffset : MonoBehaviour
{
    public FieldGenerator playingField;
    private List<float> xAnchors = new List<float>();
    private List<float> yAnchors = new List<float>();
    private float xMax, yMax, xMiddle, yMiddle;
    GameStateHandler gameStateHandler;

    void Awake(){
        gameStateHandler = GameObject.Find("GameState").GetComponent<GameStateHandler>();
    }

    void Start()
    {
        // Since the playingfield is a mesh, it is drawn from the bottom left, rather than the center as regular shapes.
        // As such, an offset has to be introduced to ensure it is placed in the center of the camera.
        // This is done by finding the maximum edge lengths, and then the midpoint such that it can be offset.
        xAnchors.Add(gameStateHandler.anchor1.x);
        xAnchors.Add(gameStateHandler.anchor2.x);
        xAnchors.Add(gameStateHandler.anchor3.x);
        xAnchors.Add(gameStateHandler.anchor4.x);

        yAnchors.Add(gameStateHandler.anchor1.y);
        yAnchors.Add(gameStateHandler.anchor2.y);
        yAnchors.Add(gameStateHandler.anchor3.y);
        yAnchors.Add(gameStateHandler.anchor4.y);
        xMax = xAnchors.Max();
        yMax = yAnchors.Max();
        xMiddle = xMax / 2f;
        yMiddle = yMax / 2f;

        // Since meshes are drawn from the bottom left, the offset has to be the negative of the middle.
        transform.position = new Vector3(-xMiddle, -yMiddle, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
