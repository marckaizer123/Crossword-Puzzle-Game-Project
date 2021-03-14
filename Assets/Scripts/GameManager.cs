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
                flagNames.Add(new KeyValuePair<string, Sprite>(sprite.name.ToUpper(), sprite)); ;
            }           
        }
        foreach (Sprite sprite in logos)
        {
            if (sprite.name.Length < 3 * gridCellCount / 5)
            {
                logoNames.Add(new KeyValuePair<string, Sprite>(sprite.name.ToUpper(), sprite));
            }
        }
        foreach (Sprite sprite in movies)
        {
            if (sprite.name.Length < 3 * gridCellCount / 5)
            {
                movieNames.Add(new KeyValuePair<string, Sprite>(sprite.name.ToUpper(), sprite));
            }
        }
    }


    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject categoryPanel;

    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField]
    private GameObject creditsPanel;

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
    }

    public void ShowCategories()
    {
        mainMenuPanel.SetActive(false);
        categoryPanel.SetActive(true);
    }
    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void ShowCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        categoryPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }




}
