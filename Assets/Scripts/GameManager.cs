using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private int gridCellCount = 10;

    public int GridCellCount
    {
        get { return gridCellCount; }
    }

    public Sprite[] flags;
    public Sprite[] logos;
    public Sprite[] movies;

    public List<KeyValuePair<string, Sprite>> flagNames = new List<KeyValuePair<string, Sprite>>();
    public List<KeyValuePair<string, Sprite>> logoNames= new List<KeyValuePair<string, Sprite>>();
    public List<KeyValuePair<string, Sprite>> movieNames = new List<KeyValuePair<string, Sprite>>();



    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        GetWordLists();

        CrosswordManager.Instance.GetMasterList("Flags");
    }

    private void GetWordLists()
    {
        flags = Resources.LoadAll<Sprite>("Flags");
        logos = Resources.LoadAll<Sprite>("Logos");
        movies = Resources.LoadAll<Sprite>("Movies");

        foreach (Sprite sprite in flags)
        {
            if (sprite.name.Length < 3 * gridCellCount / 5)
            {
                flagNames.Add(new KeyValuePair<string, Sprite>(sprite.name, sprite));
            }           
        }
        foreach (Sprite sprite in logos)
        {
            if (sprite.name.Length < 3 * gridCellCount / 5)
            {
                logoNames.Add(new KeyValuePair<string, Sprite>(sprite.name, sprite));
            }
        }
        foreach (Sprite sprite in movies)
        {
            if (sprite.name.Length < 3 * gridCellCount / 5)
            {
                movieNames.Add(new KeyValuePair<string, Sprite>(sprite.name, sprite));
            }
        }
    }


}
