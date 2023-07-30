using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public int id = 0; // use name as ID instead? 
    public int length = 1;
    public int height = 1;
    public int x = 0;
    public int y = 0;
    public bool isSelected = true;  //TODO  set to false by default
    public bool isMovable = true;
    public bool isWinCondition = false;

    private GameObject objectInstance;

    private List<GameObject> arrows;
    public GameObject arrow_Up;
    public GameObject arrow_Down;
    public GameObject arrow_Left;
    public GameObject arrow_Right;

/*    public void ApplyDamage(float damage)
    {
        print(damage);
    }*/

    private void Start()
    {
        arrows = new List<GameObject>();
/*
        Transform[] ts = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;*/

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

    public void ShowAllArrows()
    {
        arrow_Up.SetActive(true);
        arrow_Down.SetActive(true);
        arrow_Left.SetActive(true);
        arrow_Right.SetActive(true);
    }

    public void DisplayAllArrows(bool showArrows)
    {
        arrow_Up.SetActive(showArrows);
        arrow_Down.SetActive(showArrows);
        arrow_Left.SetActive(showArrows);
        arrow_Right.SetActive(showArrows);
    }
    public void DisplayValidArrows()
    {
      //  if (!isSelected) return;

        DisplayAllArrows(false);
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
        this.isSelected = true; // deselect all others
        DisplayValidArrows();

    }

    /*    private void Update()
        {
            if (timer < 5)
            {
                timer += Time.deltaTime;
            }
            else
            {
                this.gameObject.SetActive(true);
                timer = 0;
            }
        }*/

}
