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
    string answerString;

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
        ChangeAnswerText(index);
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
        ChangeAnswerText(index);
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

    //Make this better.
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

    public void ChangeAnswerText(int index)
    {
        answerString = "";

        if (Crossword.Instance.quizList[index].Answer == "")
        {
            for (int i = 0; i < Crossword.Instance.quizList[index].WordSpelling.Length; i++)
            {
                answerString += "_ ";
                answer.text = answerString;
                answer2.text = answerString;
            }
        }
        else
        {
            answer.text = Crossword.Instance.quizList[index].Answer;
            answer2.text = Crossword.Instance.quizList[index].Answer;
        }
    }

    public void ShowAnswerPanel()
    {
        answerPanel.SetActive(true);
    }

    public void ButtonPress(string btnPressed)
    {

        if(Crossword.Instance.quizList[index].Answer == "")
        {
            answer2.text = "";
        }
        else
            answer2.text = Crossword.Instance.quizList[index].Answer;

        if (btnPressed != "<" && Crossword.Instance.quizList[index].Answer.Length < Crossword.Instance.quizList[index].WordSpelling.Length)
        {
            answer2.text += btnPressed;
            Crossword.Instance.quizList[index].Answer = answer2.text;
        }
        else if(btnPressed == "<" && answer2.text.Length>0)
        {
            answer2.text = answer2.text.Remove(answer2.text.Length - 1, 1);
            Crossword.Instance.quizList[index].Answer = answer2.text;
        }
            
    }

    public void SubmitAnswer()
    {
        if (true)
        {

        }


        answer.text = answer2.text;
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
