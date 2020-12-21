﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPosition;
    private float previousTime;
    public float fallTime = 1f;
    public static int height = 20;
    public static int width = 10;
    public static Transform[,] grid = new Transform[width, height];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPosition), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPosition), new Vector3(0, 0, 1), -90);
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : 10))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                this.enabled = false;
                FindObjectOfType<SpawnTetromino>().newTetromino();
            }
            previousTime = Time.time;
        }

    }

    void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }
    
    bool HasLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
        
    }

    void RowDown(int i)
    {
        for(int y = i; y < height; y++ )
            for(int j =0; j < width; j++)
                if(grid[j,y] != null)
                {

                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
    }
    void AddToGrid()
    {
        foreach(Transform children in transform)
        {
            int RoundedX = Mathf.RoundToInt(children.transform.position.x);
            int RoundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[RoundedX, RoundedY] = children;
         }
    }
    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int RoundedX = Mathf.RoundToInt(children.transform.position.x);
            int RoundedY = Mathf.RoundToInt(children.transform.position.y);

            if (RoundedX < 0 || RoundedX >= width || RoundedY < 0 || RoundedY >= height)
            {
                return false;
            }
            if (grid[RoundedX, RoundedY] != null)
                return false;
        }

        return true;
    }
}
