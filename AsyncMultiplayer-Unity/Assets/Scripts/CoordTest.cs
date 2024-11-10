using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordTest : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    int[,] coord = new int[5, 5];
    private void Start()
    {
        // create a 2D array of size 5x5
        for (int i = 0; i < coord.GetLength(0); i++)
        {
            for (int j = 0; j < coord.GetLength(1); j++)
            {
                coord[i, j] = i + j;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(coord[x, y]);
        }
    }
}
