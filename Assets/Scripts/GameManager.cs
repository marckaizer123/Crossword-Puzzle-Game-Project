using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private int gridCellCount;

    public int GridCellCount
    {
        get { return gridCellCount; }
    }

    private Sprite[] flags;
    private Sprite[] logos;
    private Sprite[] movies;


    public List<string> flagNames;
    public List<string> logoNames;
    public List<string> movieNames;

    [SerializeField]
    private int maxWordLength;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        GetWordLists();
    }

    private void GetWordLists()
    {
        flags = Resources.LoadAll<Sprite>("Flags") as Sprite[];
        logos = Resources.LoadAll<Sprite>("Logos") as Sprite[];
        movies = Resources.LoadAll<Sprite>("Movies") as Sprite[];


        foreach (Sprite sprite in flags)
        {
            flagNames.Add(sprite.name);
        }
        foreach (Sprite sprite in logos)
        {
            logoNames.Add(sprite.name);
        }
        foreach (Sprite sprite in movies)
        {
            movieNames.Add(sprite.name);
        }
    }


}
