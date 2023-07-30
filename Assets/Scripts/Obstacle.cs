using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public int length = 1;
    public int height = 1;
    public int x = 0;
    public int y = 0;
    public bool isMovable = true;
    public bool isWinCondition = false;

    private GameObject objectInstance;

    public GameObject arrow_Up;
    public GameObject arrow_Down;
    public GameObject arrow_Left;
    public GameObject arrow_Right;

    private void Start()
    {

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            String name = transform.GetChild(i).transform.name;  //delete second .transform ?
            switch (name)
            {
                case "Arrow_Up": arrow_Up = transform.GetChild(i).gameObject;  break;
                case "Arrow_Down": arrow_Down = transform.GetChild(i).gameObject;  break; 
                case "Arrow_Left": arrow_Left = transform.GetChild(i).gameObject;  break; 
                case "Arrow_Right": arrow_Right = transform.GetChild(i).gameObject;  break;
            }
        }
        HideAllArrows();
    }

    public void HideAllArrows()
    {
        arrow_Up.SetActive(false);
        arrow_Down.SetActive(false);
        arrow_Left.SetActive(false);
        arrow_Right.SetActive(false);
    }

    public void DisplayValidArrows()
    {

        HideAllArrows();
        if (!isMovable) return;

        var check = GameManager.getInstance();

        if(check.CanBeMovedLeft(this))
        {
            arrow_Left.SetActive(true);
        }
        if (check.CanBeMovedRight(this))
        {
            arrow_Right.SetActive(true);
        }
        if (check.CanBeMovedUp(this))
        {
            arrow_Up.SetActive(true);
        }
        if (check.CanBeMovedDown(this))
        {
            arrow_Down.SetActive(true);
        }

    }

    private void OnMouseDown()
    {
        GameManager.getInstance().HideAllArrowsOnScreen();
        DisplayValidArrows();

    }

}
