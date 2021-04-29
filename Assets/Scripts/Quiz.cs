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

    public void ShowAnswerPanel()
    {
        answerPanel.SetActive(true);
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
        UpdateAnswerText(index);
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
        UpdateAnswerText(index);
    }

    public void ChangeClue(int index)
    {
        clue.GetComponent<Image>().sprite = Crossword.Instance.quizList[index].WordClue;
        clue.GetComponent<AspectRatioFitter>().aspectRatio =  Crossword.Instance.quizList[index].WordClue.bounds.size.x / Crossword.Instance.quizList[index].WordClue.bounds.size.y;
        clue2.GetComponent<Image>().sprite = Crossword.Instance.quizList[index].WordClue;
        clue2.GetComponent<AspectRatioFitter>().aspectRatio = Crossword.Instance.quizList[index].WordClue.bounds.size.x / Crossword.Instance.quizList[index].WordClue.bounds.size.y;
    }

    public void FocusOnWord(int index)
    {
        Canvas.ForceUpdateCanvases();

        Point firstPoint = Crossword.Instance.quizList[index].CharPositions[0];
        Point lastPoint = Crossword.Instance.quizList[index].CharPositions.Last();

        float midpointX = (Grid.Instance.Tiles[firstPoint].GetComponent<RectTransform>().localPosition.x + Grid.Instance.Tiles[lastPoint].GetComponent<RectTransform>().localPosition.x) / 2;
        float midpointY = (Grid.Instance.Tiles[firstPoint].GetComponent<RectTransform>().localPosition.y + Grid.Instance.Tiles[lastPoint].GetComponent<RectTransform>().localPosition.y) / 2;
        Vector2 childPosition = new Vector2(midpointX, midpointY);

        Vector2 result = new Vector2(0 - childPosition.x, 0 - childPosition.y);

        scrollRect.content.localPosition = result;
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

    public void ButtonPress(string btnPressed)
    {
        UpdateAnswerFromGrid();
        actualAnswer = Crossword.Instance.quizList[index].WordSpelling;
        currentAnswer = Crossword.Instance.quizList[index].Answer;


        if (btnPressed != "<" && currentAnswer.Length < actualAnswer.Length)
        {
            Crossword.Instance.quizList[index].Answer += btnPressed;
            UpdateGridAnswer();
        }
        else if (btnPressed == "<" && currentAnswer.Length > 0) //Pressing the delete button
        {
            Crossword.Instance.quizList[index].Answer = currentAnswer.Remove(currentAnswer.Length - 1, 1);
            UpdateGridAnswer();
        }
        else
        {
            AudioManager.Instance.PlaySFX("Error");
        }

        UpdateAnswerText(index);
    }

    public void UpdateAnswerText(int index)
    {
        UpdateAnswerFromGrid();
        currentAnswer = Crossword.Instance.quizList[index].Answer;
        actualAnswer = Crossword.Instance.quizList[index].WordSpelling;

        if (currentAnswer.Equals(""))
        {
            for (int i = 0; i < actualAnswer.Length; i++)
            {
                if(i<actualAnswer.Length - 1)
                    currentAnswer += "_ ";
                else
                    currentAnswer += "_";
                
                answer.text = currentAnswer;
                answer2.text = currentAnswer;
            }
        }
        else
        {
            answer.text = "";
            answer2.text = "";
            for (int i = 0; i < currentAnswer.Length; i++)
            {
                if(currentAnswer[i].Equals(' ') && i<currentAnswer.Length - 1)
                {
                    answer.text += " _";
                    answer2.text += " _";
                }
                else if(currentAnswer[i].Equals(" ") && i == currentAnswer.Length - 1)
                {
                    answer.text += "_";
                    answer2.text += "_";
                }
                else
                {
                    answer.text += currentAnswer[i].ToString();
                    answer2.text += currentAnswer[i].ToString();
                }              
            }

            for (int i = 0; i < actualAnswer.Length - currentAnswer.Length; i++)
            {
                if (i < actualAnswer.Length - currentAnswer.Length - 1)
                {
                    answer2.text += " _";
                    answer.text += " _";
                }
                else
                {
                    answer2.text += "_";
                    answer.text += "_";
                }
            }
        }
    }

    private void UpdateGridAnswer()
    {
        currentAnswer = Crossword.Instance.quizList[index].Answer;
        int count = 0;

        foreach (Point point in Crossword.Instance.quizList[index].CharPositions)
        {
            if (count <= currentAnswer.Length - 1)
            {
                Grid.Instance.Tiles[point].TileLetter.text = currentAnswer[count].ToString();
                count++;
            }
            else
            {
                Grid.Instance.Tiles[point].TileLetter.text = "";
            }
        }
    }

    private void UpdateAnswerFromGrid()
    {

        string word = "";
        foreach (Point point in Crossword.Instance.quizList[index].CharPositions)
        {
            if (Grid.Instance.Tiles[point].TileLetter.text.Equals(""))
            {
                word += " ";
            }
            else
            {
                word += Grid.Instance.Tiles[point].TileLetter.text;
            }
        }
        if(word.Trim().Length == 0)
        {
            Crossword.Instance.quizList[index].Answer = "";
        }

        else
        {
            Crossword.Instance.quizList[index].Answer = word.TrimEnd();
        }
    }

    public void SubmitAnswer()
    {
        AudioManager.Instance.PlaySFX("Submit");
        answerPanel.SetActive(false);
    }

    [SerializeField]
    private GameObject scorePanel;
    [SerializeField]
    private Text scoreText;

    private void FinishCrossword()
    {
        AudioManager.Instance.PlaySFX("Finish");
        int score = 0;
        foreach (Word word in Crossword.Instance.quizList)
        {
            for (int i = 0; i < word.Answer.Length; i++)
            {
                if (word.Answer[i].Equals(word.WordSpelling[i]))
                {
                    score += 1;
                }
            }   
        }

        score = score * 100;
        scoreText.text = score.ToString();
        scorePanel.SetActive(true);
    }
}
