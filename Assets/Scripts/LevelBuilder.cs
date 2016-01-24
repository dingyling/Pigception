using UnityEngine;
using System.Text;
using System.IO;
using System.Collections;
using System;

public class LevelBuilder : MonoBehaviour {

    private string levelRAW;
    private string levelName;
    private string[] levelLines;
    private int levelWidth;
    private int levelHeight;
    private Vector2 startPos;
    private float PixelSize;
    private Vector2 tempPos;
    private GameObject obstacle;
    private GameObject door;

    public string level;
    public GameObject wall;
    public GameObject blank;
    public GameObject player;
    public GameObject blank_wc;

    public GameObject obstacle_g;
    public GameObject obstacle_r;
    public GameObject obstacle_p;
    public GameObject obstacle_y;

    public GameObject door_g;
    public GameObject door_r;
    public GameObject door_p;
    public GameObject door_y;
    public GameObject door_goal;

    void quadrant (int a, int b, int c, int d, char l) {

        int xGridFix = 0;
        int yGridFix = 0;
        char[] obstacles = { 'r', 'g', 'y', 'p' };

        switch (l)
        {
            case 'r':
                obstacle = obstacle_r;
                door = door_r;
                break;
            case 'y':
                obstacle = obstacle_y;
                door = door_y;
                xGridFix = 2;
                break;
            case 'g':
                obstacle = obstacle_g;
                door = door_g;
                yGridFix = 2;
                break;
            case 'p':
                obstacle = obstacle_p;
                door = door_p;
                xGridFix = 2;
                yGridFix = 2;
                break;
        }

        tempPos = startPos + new Vector2(c * PixelSize / 2 * (levelWidth - xGridFix), d * PixelSize / 2 * (levelHeight - yGridFix));
        for (int i = a; i < levelHeight; i++)
        {
            for (int j = b; j < levelWidth; j++)
            {
                if (levelLines[i][j] == '#')
                {
                    Vector3 spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oWall = Instantiate(wall, spawnPosition, Quaternion.identity) as GameObject;
                    oWall.transform.parent = transform;
                }
                else if (levelLines[i][j] == l)
                {
                    Vector3 spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oRObstacle = Instantiate(obstacle, spawnPosition, Quaternion.identity) as GameObject;
                    oRObstacle.transform.parent = transform;

                    spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oBlank = Instantiate(blank, spawnPosition, Quaternion.identity) as GameObject;
                    oBlank.transform.parent = transform;
                }
                else if (levelLines[i][j] == Char.ToUpper(l))
                {
                    Vector3 spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oDoor = Instantiate(door, spawnPosition, Quaternion.identity) as GameObject;
                    oDoor.transform.parent = transform;

                }
                else if (levelLines[i][j] == '$')
                {
                    Vector3 spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oDoor = Instantiate(door_goal, spawnPosition, Quaternion.identity) as GameObject;
                    oDoor.transform.parent = transform;
                }
                else if (levelLines[i][j] == '@')
                {
                    Vector3 spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oPlayer = Instantiate(player, spawnPosition, Quaternion.identity) as GameObject;
                    oPlayer.transform.parent = transform;

                    spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oBlank = Instantiate(blank, spawnPosition, Quaternion.identity) as GameObject;
                    oBlank.transform.parent = transform;
                }
                else if (levelLines[i][j] == ' ')
                {
                    Vector3 spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oBlank = Instantiate(blank, spawnPosition, Quaternion.identity) as GameObject;
                    oBlank.transform.parent = transform;
                }
                else if (Array.IndexOf(obstacles, levelLines[i][j]) >= 0) {
                    Vector3 spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oBlankWC = Instantiate(blank_wc, spawnPosition, Quaternion.identity) as GameObject;
                    oBlankWC.transform.parent = transform;
                    oBlankWC.tag = GetTag(levelLines[i][j]);
                }
                else
                {
                    Vector3 spawnPosition = new Vector3(-tempPos.x + PixelSize * j, tempPos.y - PixelSize * i, -1);
                    GameObject oBlank = Instantiate(blank, spawnPosition, Quaternion.identity) as GameObject;
                    oBlank.transform.parent = transform;
                }
            }
        }
    }

    string GetTag (char l)
    {
        string currentTag = "Untagged";

        switch (l)
        {
            case 'r':
                currentTag = "red";
                break;
            case 'g':
                currentTag = "green";
                break;
            case 'y':
                currentTag = "yellow";
                break;
            case 'p':
                currentTag = "purple";
                break;

        }
        return currentTag;
    }

    // Use this for initialization
    void Start () {

        levelName = "Assets/Levels/" + level + ".txt";
        levelRAW = System.IO.File.ReadAllText(levelName);
        levelLines = levelRAW.Split('\n');
        levelWidth = levelLines[0].Length-1;
        levelHeight = levelLines.Length;
        PixelSize = (float).64;

        startPos = new Vector2 (PixelSize / 2 * (levelWidth-1), PixelSize / 2 * (levelHeight - 1));
        //Debug.Log(words[0][0]);

        //1st quadrant
        quadrant(0,0,1,1, 'r');

        //2st quadrant
        quadrant(0, 1, -1, 1, 'y');

        //3st quadrant
        quadrant(1, 0, 1, -1, 'g');

        //4st quadrant
        quadrant(1,1 , -1,-1, 'p');
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
