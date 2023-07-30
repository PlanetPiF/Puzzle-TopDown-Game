using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject obstacleBoxes;
    public GameObject obstaclePlant1;
    public GameObject obstaclePlant2;
    public GameObject obstacleStroller;
    public GameObject obstacleSuitcases;
    public GameObject obstacleBike;
    public GameObject family;
    public GameOverScreen gameOverScreen;

    private List<GameObject> obstacleGameObjects;
    private List<Obstacle> obstacles;  //TODO refactor to use a single list

    private const float OFFSET_VERTICAL = 1f;
    private const float OFFSET_HORIZONTAL = 1f;

    private const int GRID_ROWS = 6;
    private const int GRID_COLUMNS = 6;

    private const int WINNING_LOCATION_ROW = 4;
    private const int WINNING_LOCATION_COL = 5;  //TODO fix x and y names to avoid confusion?

    private int[,] grid;

    public int[,] getGrid() { return grid; }

    //Singleton
    private static GameManager gameManager;
    public static GameManager getInstance()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        }

        if(gameManager == null)
        {
            Debug.Log("ERROR!");
        } 
        
        return gameManager;
    }

    void InitGrid()
    {
        grid = new int[GRID_ROWS, GRID_COLUMNS];
        for (int x =0; x< GRID_ROWS; x++)
        {
            for(int y=0; y < GRID_COLUMNS; y++)
            {
                grid[x,y] = 0;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        InitGrid();

        RectTransform rectTransform = GetComponent<RectTransform>();

        obstacleGameObjects = new List<GameObject>();
        obstacles = new List<Obstacle>();

        // Spawn Obstacles
        SpawnBoxes(4, 3);
        SpawnPlant1(0,4);
        SpawnFamily(1,0);
        SpawnPlant2(4,0);
        SpawnSuitcases(2, 2);
        SpawnStroller(2, 1);
        SpawnBike(1,1);

        //TODO  Random / Procedural Generation of obstacles
    }

    public void HideAllArrowsOnScreen()
    {
        foreach(Obstacle obstacle in obstacles)
        {
            obstacle.HideAllArrows();
        }
    }

    void EndGame()
    {
        gameOverScreen.Setup();
    }

    void UpdateGrid(int x, int y, int len, int hei)
    {
        int updated = 0;
        for(int i = 0; i < hei; i++)
        {
            for(int j=0; j < len; j++)
            {
                grid[x + i, y + j] = 1;
                updated++;
            }
        }
    }

    public void PerformGameOverCheck(Obstacle obstacle)
    {
        //check win condition (Family object made it to the exit door location)
        if (obstacle.isWinCondition == false) return;

        if(obstacle.x == WINNING_LOCATION_ROW && obstacle.y == WINNING_LOCATION_COL)
        {
            foreach(GameObject o in obstacleGameObjects)
            {
                Destroy(o);
            }
            EndGame();  
        }
    }

    public bool CanBeMovedLeft(Obstacle obstacle)
    {
        if (obstacle == null) return false;

        return CanBeMovedLeft(obstacle.x, obstacle.y, obstacle.length, obstacle.height);
    }

    public bool CanBeMovedRight(Obstacle obstacle)
    {   
        if(obstacle == null) return false;
        int x = obstacle.x;
        int y   = obstacle.y;
        int length = obstacle.length;
        int height = obstacle.height;

        if (y + length >= GRID_COLUMNS) return false;

        for (int i = 0; i < height; i++)
        {
            if (grid[x + i, y + length] == 1)
            {
                return false;
            }
        }

        return true;
    }

    public bool CanBeMovedUp(Obstacle obstacle)
    {
        if (obstacle == null) return false;
        int x = obstacle.x;
        int y = obstacle.y;
        int length = obstacle.length;

        if (x == 0) return false;

        for (int i = 0; i < length; i++)
        {
            if (grid[x - 1, y + i] == 1)
            {
                return false;
            }
        }

        return true;
    }

    public bool CanBeMovedDown(Obstacle obstacle)
    {
        if (obstacle == null) return false;
        int x = obstacle.x;
        int y = obstacle.y;
        int length = obstacle.length;
        int height = obstacle.height;

        if (x + height >= GRID_ROWS) return false;

        for (int i = 0; i < length; i++)
        {
            if (grid[x + height , y + i] == 1)
            {
                return false;
            }
        }

        return true;
    }

    public bool CanBeMovedLeft(int x, int y, int len, int hei)
    {
        if (y == 0) return false;

        for (int i = 0; i < hei; i++)
        {
            if (grid[x + i , y - 1] == 1)
            {
                return false;
            }
        }

        return true;
    }

    public void MoveLeft(Obstacle obstacle)
    {
        int x = obstacle.x;
        int y = obstacle.y;
        int length = obstacle.length;
        int heigth = obstacle.height;

        for (int i = 0; i < heigth; i++)
        {
            // fill in new location
            grid[x + i, y - 1] = 1;
            // free last location
            grid[x + i, y + length-1] = 0;
        }

        obstacle.y -= 1;

    }

    public void MoveRight(Obstacle obstacle)
    {
        int x = obstacle.x;
        int y = obstacle.y;
        int length = obstacle.length;
        int heigth = obstacle.height;

        for (int i = 0; i < heigth; i++)
        {
            // fill in new location
            grid[x + i , y] = 0;
            // free last location
            grid[x + i , y + length] = 1;

        }

        obstacle.y += 1;

    }

    public void MoveUp(Obstacle obstacle)
    {
        int x = obstacle.x;
        int y = obstacle.y;
        int length = obstacle.length;
        int heigth = obstacle.height;

        for (int i = 0; i < length; i++)
        {
            // fill in new location
            grid[x - 1 , y + i] = 1;
            // free last location
            grid[x + heigth - 1 , y + i] = 0;

        }

        obstacle.x -= 1;

    }

    public void MoveDown(Obstacle obstacle)
    {
        int x = obstacle.x;
        int y = obstacle.y;
        int length = obstacle.length;
        int heigth = obstacle.height;

        for (int i = 0; i < length; i++)
        {
            // fill in new location
            grid[x + heigth , y + i] = 1;
            // free last location
            grid[x , y + i] = 0;

        }
        
        obstacle.x += 1; 

    }

    void SpawnFamily(int row, int column)
    {
        GameObject familyTile = Instantiate(family, new Vector3(transform.position.x + (OFFSET_HORIZONTAL * column), transform.position.y - (OFFSET_VERTICAL * row)), transform.rotation);

        familyTile.name = "Family";
        familyTile.transform.SetParent(this.transform);

        //set Obstacle params here
        Obstacle ob = familyTile.GetComponent<Obstacle>();
        ob.length = 1;
        ob.height = 1;
        ob.x = row;
        ob.y = column;
        ob.isWinCondition = true;

        obstacleGameObjects.Add(familyTile);
        obstacles.Add(ob);
        UpdateGrid(ob.x, ob.y, ob.length, ob.height);
    }

    void SpawnBoxes(int row, int column)
    {
        GameObject boxObstacle = Instantiate(obstacleBoxes, 
            new Vector3(transform.position.x + (OFFSET_HORIZONTAL * column), transform.position.y - (OFFSET_VERTICAL * row)), transform.rotation);

        boxObstacle.name = "Boxes";
        boxObstacle.transform.SetParent(this.transform);

        //set Obstacle params here
        Obstacle ob = boxObstacle.GetComponent<Obstacle>();
        ob.length = 3;
        ob.height = 2;
        ob.x = row;
        ob.y = column;

        obstacleGameObjects.Add(boxObstacle);
        obstacles.Add(ob);
        UpdateGrid(ob.x,ob.y, ob.length, ob.height);
    }

    void SpawnPlant1(int row, int column)
    {
        GameObject plant1Obstacle = Instantiate(obstaclePlant1, 
            new Vector3(transform.position.x + (OFFSET_HORIZONTAL * column), transform.position.y - (OFFSET_VERTICAL * row)), transform.rotation);

        plant1Obstacle.name = "Plant1";
        plant1Obstacle.transform.SetParent(this.transform);

        //set Obstacle params here
        Obstacle ob = plant1Obstacle.GetComponent<Obstacle>();
        ob.length = 2;
        ob.height = 3;
        ob.x = row;
        ob.y = column;

        obstacleGameObjects.Add(plant1Obstacle);
        obstacles.Add(ob);
        UpdateGrid(ob.x, ob.y, ob.length, ob.height);

    }

    void SpawnPlant2(int row, int column)
    {
        GameObject plant1Obstacle = Instantiate(obstaclePlant2, 
            new Vector3(transform.position.x + (OFFSET_HORIZONTAL * column), transform.position.y - (OFFSET_VERTICAL * row)), transform.rotation);

        plant1Obstacle.name = "Plant2";
        plant1Obstacle.transform.SetParent(this.transform);

        //set Obstacle params here
        Obstacle ob = plant1Obstacle.GetComponent<Obstacle>();
        ob.length = 2;
        ob.height = 2;
        ob.x = row;
        ob.y = column;

        obstacleGameObjects.Add(plant1Obstacle);
        obstacles.Add(ob);
        UpdateGrid(ob.x, ob.y, ob.length, ob.height);

    }
    void SpawnSuitcases(int row, int column)
    {
        GameObject suitcases = Instantiate(obstacleSuitcases, 
            new Vector3(transform.position.x + (OFFSET_HORIZONTAL * column), transform.position.y - (OFFSET_VERTICAL * row)), transform.rotation);

        suitcases.name = "Suitcases";
        suitcases.transform.SetParent(this.transform);

        //set Obstacle params here
        Obstacle ob = suitcases.GetComponent<Obstacle>();
        ob.length = 2;
        ob.height = 2;
        ob.x = row;
        ob.y = column;

        obstacleGameObjects.Add(suitcases);
        obstacles.Add(ob);
        UpdateGrid(ob.x, ob.y, ob.length, ob.height);

    }
    void SpawnStroller(int row, int column)
    {
        GameObject stroller = Instantiate(obstacleStroller, 
            new Vector3(transform.position.x + (OFFSET_HORIZONTAL * column), transform.position.y - (OFFSET_VERTICAL * row)), transform.rotation);


        stroller.name = "Stroller";
        stroller.transform.SetParent(this.transform);

        //set Obstacle params here
        Obstacle ob = stroller.GetComponent<Obstacle>();
        ob.length = 1;
        ob.height = 2;
        ob.x = row;
        ob.y = column;

        obstacleGameObjects.Add(stroller);
        obstacles.Add(ob);
        UpdateGrid(ob.x, ob.y, ob.length, ob.height);

    }

    void SpawnBike(int row, int column)
    {
        GameObject bike = Instantiate(obstacleBike, 
            new Vector3(transform.position.x + (OFFSET_HORIZONTAL * column), transform.position.y - (OFFSET_VERTICAL * row)), transform.rotation);

        bike.name = "Bike";
        bike.transform.SetParent(this.transform);

        //set Obstacle params here
        Obstacle ob = bike.GetComponent<Obstacle>();
        ob.length = 3;
        ob.height = 1;
        ob.x = row;
        ob.y = column;

        obstacleGameObjects.Add(bike);
        obstacles.Add(ob);
        UpdateGrid(ob.x, ob.y, ob.length, ob.height);
    }

}
