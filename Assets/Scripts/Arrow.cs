using UnityEngine;

public class Arrow : MonoBehaviour
{

    public GameObject parent;

    private GameManager gameManager;

    public enum ArrowType
    {
        Up,
        Down,
        Left,
        Right
    }

    public ArrowType arrowType;

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<GameObject>();
    }


    private void OnMouseDown()
    {
        gameManager = GameManager.getInstance();

        var parent = this.transform.parent;
        Obstacle parentObstacle = this.transform.parent.gameObject.GetComponent<Obstacle>();

        switch (arrowType)
        {
            case ArrowType.Up:
                gameManager.MoveUp(parentObstacle);
                parent.position += Vector3.up; 
                break;
            case ArrowType.Down:
                gameManager.MoveDown(parentObstacle);
                parent.position += Vector3.down; 
                break;
            case ArrowType.Left: 
                gameManager.MoveLeft(parentObstacle);
                parent.position += Vector3.left * 1f; 
                break;
            case ArrowType.Right:
                gameManager.MoveRight(parentObstacle);
                parent.position += Vector3.right * 1f; 
                break;
        }

        //Check win condition after each move
        gameManager.PerformGameOverCheck(parentObstacle);

        parentObstacle.DisplayValidArrows();
    }
}
