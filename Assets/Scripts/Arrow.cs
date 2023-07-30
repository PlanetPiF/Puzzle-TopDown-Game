using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
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
/*    public Rigidbody2D rb;*/

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<GameObject>();
       /* rb = GetComponent<Rigidbody2D>();
        EnableRagdoll();*/
    }

/*    // Let the rigidbody take control and detect collisions.
    void EnableRagdoll()
    {
        rb.isKinematic = false;
    }

    // Let animation control the rigidbody and ignore collisions.
    void DisableRagdoll()
    {
        rb.isKinematic = true;

    }*/

    private void OnMouseDown()
    {
        gameManager = GameManager.getInstance();

        var parent = this.transform.parent;
        Obstacle parentObstacle = this.transform.parent.gameObject.GetComponent<Obstacle>();
       /* parentObstacle.ApplyDamage(parentObstacle.x);
        parentObstacle.ApplyDamage(parentObstacle.y);*/
        /* parent.SendMessage("ApplyDamage", 5.0);*/

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
        /*this.gameObject.SetActive(false);  */

        // fire an EVENT to GameManager --> update the game/board?
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*this.gameObject.SetActive(false);*/
        Debug.Log("Collision detected!");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Collision detected TYPE #2!");
    }

}
