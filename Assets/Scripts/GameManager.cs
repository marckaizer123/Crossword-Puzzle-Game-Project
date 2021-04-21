using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private GameObject categoryPanel;

    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField]
    private GameObject creditsPanel;

    [SerializeField]
    private GameObject crosswordPanel;

    [SerializeField]
    private GameObject quizPanel;

    [SerializeField]
    private GameObject scorePanel;

    public Sprite[] flags;
    public Sprite[] logos;
    public Sprite[] places;

    public List<KeyValuePair<string, Sprite>> flagNames = new List<KeyValuePair<string, Sprite>>();
    public List<KeyValuePair<string, Sprite>> logoNames= new List<KeyValuePair<string, Sprite>>();
    public List<KeyValuePair<string, Sprite>> placeNames = new List<KeyValuePair<string, Sprite>>();

    // Start is called before the first frame update
    void Start()
    {
        GetWordLists();
    }

    private void GetWordLists()
    {
        flags = Resources.LoadAll<Sprite>("Flags");
        logos = Resources.LoadAll<Sprite>("Logos");
        places = Resources.LoadAll<Sprite>("Places");

        foreach (Sprite sprite in flags)
        {
                flagNames.Add(new KeyValuePair<string, Sprite>(sprite.name.ToUpper().Replace(" ", string.Empty), sprite));       
        }
        foreach (Sprite sprite in logos)
        {
                logoNames.Add(new KeyValuePair<string, Sprite>(sprite.name.ToUpper().Replace(" ", string.Empty), sprite));
        }
        foreach (Sprite sprite in places)
        {
                placeNames.Add(new KeyValuePair<string, Sprite>(sprite.name.ToUpper().Replace(" ", string.Empty), sprite));
        }
    }

    public void ChooseCategory(string category)
    {
        crosswordPanel.SetActive(true);
        quizPanel.SetActive(true);
        Crossword.Instance.GetMasterList(category);
        mainMenuPanel.SetActive(false);

    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        menuPanel.SetActive(true);
    }

    public void ShowCategories()
    {
        menuPanel.SetActive(false);
        categoryPanel.SetActive(true);
    }
    public void ShowOptions()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void ShowCredits()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }


    [SerializeField]
    private GameObject grid;

    public void PlayAgain()
    {
        foreach (Transform child in grid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        scorePanel.SetActive(false);  
        crosswordPanel.SetActive(false);
        quizPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        categoryPanel.SetActive(true);

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
        menuPanel.SetActive(true);
    }




}
