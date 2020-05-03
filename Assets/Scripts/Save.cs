using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Puzzle
{
    public string name;
    public float[] position;
    public float[] rotation;
    public Puzzle( string name, float[] position, float[] rotation)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
    }
}
[System.Serializable]
public class Save
{
    [SerializeField]
    public List<Puzzle> puzzles;
    
    
    public Save()
    {
        puzzles = new List<Puzzle>();
    }
}
