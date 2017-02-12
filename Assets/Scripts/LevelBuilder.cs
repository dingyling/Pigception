using UnityEngine;
using System.Text;
using System.IO;
using System.Collections;
using System;

public class LevelBuilder : MonoBehaviour {

    private Vector2 startPos;
    private float PixelSize;
    private Vector2 tempPos;
    private GameObject obstacle;
    private GameObject door;
    private FileInfo[] lvls;
    private int numLvls;
    private string lvlDir;
    private int lvlNum;

    public GameObject wall;
    public GameObject wall_wc;
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

    void quadrant (int a, int b, int c, int d, char l, int levelWidth, int levelHeight, string[] levelLines) {

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
                    GameObject oWall = new GameObject();
                    if (l == 'r') {
                        oWall = Instantiate(wall_wc, spawnPosition, Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        oWall = Instantiate(wall, spawnPosition, Quaternion.identity) as GameObject;
                    }
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

                    if (l == 'r') { 
                        GameObject oPlayer = Instantiate(player, spawnPosition, Quaternion.identity) as GameObject;
                        oPlayer.transform.parent = transform;
                    }

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

    FileInfo[] GetLevels(string Dir)
    {
        DirectoryInfo lvlDir = new DirectoryInfo(Dir);
        FileInfo[] lvls = lvlDir.GetFiles("*.txt");

        return lvls;
    }

    public void LoadNextLevel()
    {
        if (lvlNum > 0)
        {
            Transform[] oldObjects = transform.GetComponentsInChildren<Transform>();
            foreach (Transform oldObject in oldObjects)
            {
                if (!(oldObject.tag == "GameController")) { 
                    Destroy(oldObject.gameObject);
                }
            }
        }
        
        if (lvlNum == numLvls)
        {
            lvlNum = 1;
        }
        else
        {
            lvlNum++;
        }
        
        string lvlName = lvlDir + lvls[lvlNum-1].Name;
        string lvlRAW = System.IO.File.ReadAllText(lvlName);
        string[] lvlLines = lvlRAW.Split('\n');
        int lvlWidth = lvlLines[0].Length - 1;
        int lvlHeight = lvlLines.Length;
        PixelSize = (float).64;

        startPos = new Vector2(PixelSize / 2 * (lvlWidth - 1), PixelSize / 2 * (lvlHeight - 1));

        //1st quadrant
        quadrant(0, 0, 1, 1, 'r', lvlWidth, lvlHeight, lvlLines);

        //2st quadrant
        quadrant(0, 1, -1, 1, 'y', lvlWidth, lvlHeight, lvlLines);

        //3st quadrant
        quadrant(1, 0, 1, -1, 'g', lvlWidth, lvlHeight, lvlLines);

        //4st quadrant
        quadrant(1, 1, -1, -1, 'p', lvlWidth, lvlHeight, lvlLines);
    }


    // Use this for initialization
    void Start () {

        lvlDir = "Assets/Levels/";
        lvls = GetLevels(lvlDir);
        numLvls = lvls.Length;
        lvlNum = 0;

        LoadNextLevel();
        
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
