using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : Singleton<Quiz>
{
    [SerializeField]
    private GameObject clue;

    [SerializeField]
    private GameObject clue2;

    [SerializeField]
    private Text answer;

    [SerializeField]
    private Text answer2;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private GameObject answerPanel;

    private int index = 0;
    private int previousIndex = 0;
    string currentAnswer;
    string actualAnswer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PreviousWord()
    {
        previousIndex = index;
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = Crossword.Instance.quizList.Count - 1;
        }

        ChangeClue(index);
        RemoveHighlights(previousIndex);
        FocusOnWord(index);
        HighlightTiles(index);
        ShowAnswerText(index);
    }

    public void NextWord()
    {
        previousIndex = index;
        if (index < Crossword.Instance.quizList.Count - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        ChangeClue(index);
        RemoveHighlights(previousIndex);
        FocusOnWord(index);
        HighlightTiles(index);
        ShowAnswerText(index);
    }

    public void ChangeClue(int index)
    {
        clue.GetComponent<Image>().sprite = Crossword.Instance.quizList[index].WordClue;
        clue.GetComponent<AspectRatioFitter>().aspectRatio =  Crossword.Instance.quizList[index].WordClue.bounds.size.x / Crossword.Instance.quizList[index].WordClue.bounds.size.y;
        clue2.GetComponent<Image>().sprite = Crossword.Instance.quizList[index].WordClue;
        clue2.GetComponent<AspectRatioFitter>().aspectRatio = Crossword.Instance.quizList[index].WordClue.bounds.size.x / Crossword.Instance.quizList[index].WordClue.bounds.size.y;
    }

    public void HighlightTiles(int index)
    {
        foreach (Point point in Crossword.Instance.quizList[index].CharPositions)
        {
            ColorTile(Grid.Instance.Tiles[point], Color.yellow);
        }
    }

    public void RemoveHighlights(int index)
    {
        foreach (Point point in Crossword.Instance.quizList[index].CharPositions)
        {
            ColorTile(Grid.Instance.Tiles[point], Color.white);
        }
    }

    private void ColorTile(Tile tile, Color32 newColor)
    {
        tile.GetComponent<Image>().color = newColor;
    }

    public void FocusOnWord(int index)
    {
        Canvas.ForceUpdateCanvases();

        Point firstPoint = Crossword.Instance.quizList[index].CharPositions[0];
        Point lastPoint = Crossword.Instance.quizList[index].CharPositions.Last();

        float midpointX = (Grid.Instance.Tiles[firstPoint].GetComponent<RectTransform>().localPosition.x + Grid.Instance.Tiles[lastPoint].GetComponent<RectTransform>().localPosition.x) /2;
        float midpointY = (Grid.Instance.Tiles[firstPoint].GetComponent<RectTransform>().localPosition.y + Grid.Instance.Tiles[lastPoint].GetComponent<RectTransform>().localPosition.y) / 2;
        Vector2 childPosition = new Vector2(midpointX, midpointY);

        Vector2 result = new Vector2(0 - childPosition.x, 0 - childPosition.y);

        scrollRect.content.localPosition = result;


    }

    public void ShowAnswerText(int index)
    {
        currentAnswer = "";
        actualAnswer = Crossword.Instance.quizList[index].WordSpelling;

        foreach (Point point in Crossword.Instance.quizList[index].CharPositions)
        {
            if(Grid.Instance.Tiles[point].TileLetter.text == "")
            {
                currentAnswer += " _";
            }
            else
            {
                currentAnswer += Grid.Instance.Tiles[point].TileLetter.text;
            }     
        }

        if (currentAnswer == "")
        {
            for (int i = 0; i < actualAnswer.Length; i++)
            {
                currentAnswer += "_ ";
                answer.text = currentAnswer;
                answer2.text = currentAnswer;
            }
        }
        else
        {
            answer.text = currentAnswer;
            answer2.text = currentAnswer;

            for (int i = 0; i < actualAnswer.Length - currentAnswer.Length; i++)
            {
                answer2.text += " _";
                answer.text += " _";
            }
        }
    }

    public void ShowAnswerPanel()
    {
        answerPanel.SetActive(true);
    }

    public void ButtonPress(string btnPressed)
    {
        actualAnswer = Crossword.Instance.quizList[index].WordSpelling;
        currentAnswer = Crossword.Instance.quizList[index].Answer;


        if (btnPressed != "<" && currentAnswer.Length < actualAnswer.Length)
        {
            Crossword.Instance.quizList[index].Answer += btnPressed;
            currentAnswer = Crossword.Instance.quizList[index].Answer;
        }
        else if (btnPressed == "<" && currentAnswer.Length > 0) //Pressing the delete button
        {
            Crossword.Instance.quizList[index].Answer = currentAnswer.Remove(currentAnswer.Length - 1, 1);
            currentAnswer = Crossword.Instance.quizList[index].Answer;
        }

        answer2.text = currentAnswer;
        answer.text = currentAnswer;

        for (int i = 0; i < actualAnswer.Length - currentAnswer.Length; i++)
        {
            answer2.text += " _";
            answer.text += " _";
        }            
    }

    public void SubmitAnswer()
    {
        currentAnswer = Crossword.Instance.quizList[index].Answer;
        int count = 0;

        foreach (Point point in Crossword.Instance.quizList[index].CharPositions)
        {
            if (count<=currentAnswer.Length-1)
            {
                Grid.Instance.Tiles[point].TileLetter.text = currentAnswer[count].ToString();
                count++;
            }
            else
            {
                Grid.Instance.Tiles[point].TileLetter.text = "";
            }
            
        }

        answerPanel.SetActive(false);
    }


    public void FinishCrossword()
    {

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
