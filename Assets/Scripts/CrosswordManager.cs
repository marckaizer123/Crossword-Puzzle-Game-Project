using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Globals;


public class CrosswordManager : Singleton<CrosswordManager>
{
    [SerializeField]
    private int initWordCount;

    [SerializeField]
    private int maxAttempts;

    private List<KeyValuePair<string, Sprite>> masterList = new List<KeyValuePair<string, Sprite>>(); //Contains the words and clues that the crossword would build from.

    private List<Word> quizList = new List<Word>(); //Contains all the words that have been already been placed into the wordMatrix, as well as their corresponding sprite and positions.

    private List<Direction> directions = new List<Direction>();

    private System.Random rnd;

    private char[,] wordMatrix; // wordMatrix that simulates the grid and holds the letters.



    // Start is called before the first frame update
    void Start()
    {
        directions.Add(Direction.Down);
        directions.Add(Direction.Right);
        directions.Add(Direction.None);

        GetMasterList();

        
    }

    public void GetMasterList() //To do: add a switch case for different categories of crossword puzzles.
    {
        string category = "Flags";

        Sprite[] clues = Resources.LoadAll<Sprite>(category) as Sprite[];

        foreach (Sprite sprite in clues)
        {
            masterList.Add(new KeyValuePair<string, Sprite>(sprite.name, sprite));
        }

        PlaceWordsOnGrid(masterList);
    }

    private void PlaceWordsOnGrid(List<KeyValuePair<string, Sprite>> masterList)
    {
        try
        {
            quizList.Clear();
            wordMatrix = new char[GameManager.Instance.GridCellCount, GameManager.Instance.GridCellCount];
            rnd = new System.Random(System.DateTime.Now.Millisecond);
            Direction direction;
            int x, y;
            long attempts;
            bool success;
            string word;

            /**/
            for (int i = 0; i < masterList.Count; i++)
            {
                attempts = 0;
                success = false;
                word = masterList[i].Key;

                do
                {
                    direction = GetDirection(rnd, 3);
                    x = GetRandomAxis(rnd, GameManager.Instance.GridCellCount);
                    y = GetRandomAxis(rnd, GameManager.Instance.GridCellCount);
                    success = PlaceTheWord(direction, x, y, masterList[i].Key, masterList[i].Value, i, ref attempts);
                    if (attempts > maxAttempts)
                    {
                        AddWordToList(word, masterList[i].Value, -1, -1, Direction.None, attempts, true);
                        break;
                    }


                }
                while (!success);
            }



        }
        catch (System.Exception e)
        {
            
            Debug.Log($"An error occurred in 'PlaceWordsOnTheBoard()' method of the 'GameEngine' class. Error msg: {e.Message}");
        }
    }

    /// <summary>
    /// Randomly generate direction - ACROSS (RIGHT), or DOWN.
    /// </summary>
    /// <param name="Rnd"></param>
    /// <param name="Max"></param>
    /// <returns></returns>
    private Direction GetDirection(System.Random Rnd, int Max)
    {
        switch (Rnd.Next(1, Max))   // Generate a random number between 1 and Max - 1; So if Max = 9, it will generate a random direction between 1 and 8.
        {
            case 1: if (directions.Find(p => p.Equals(Direction.Down)) == Direction.Down) return Direction.Down; break;
            case 2: if (directions.Find(p => p.Equals(Direction.Right)) == Direction.Right) return Direction.Right; break;
            default: return Direction.None;
        }
        return Direction.None;
    }

    /// <summary>
    /// Generates random X or Y axis.
    /// </summary>
    /// <param name="Rnd"></param>
    /// <param name="Max"></param>
    /// <returns></returns>
    private int GetRandomAxis(System.Random Rnd, int Max)
    {
        return Rnd.Next(Max);   // Generates a number from 0 up to the grid size.
    }



    /// <summary>
    /// This method first checks if there is a valid overlap with any existing letter in the same cell of the wordMatrix.
    /// It also makes sure the first few words don't overlap to make them sparse around the wordMatrix.
    /// After the first few scattered words, it makes sure all the subsequent letters are overlapped with existing words.
    /// Then it checks legitimate cross-through with an existing word.
    /// If all passes, then it places the word in the collection.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="word"></param>
    /// <param name="wordClue"></param>
    /// <param name="currentWordCount"></param>
    /// <param name="attempts"></param>
    /// <returns></returns>
    private bool PlaceTheWord(Direction direction, int x, int y, string word, Sprite wordClue, int currentWordCount, ref long attempts)
    {
        try
        {
            attempts++;
            bool placeAvailable = true, overlapped = false;
            switch (direction)
            {
                case Direction.Right:
                    for (int i = 0, xx = x; i < word.Length; i++, xx++) // First we check if the word can be placed in the array. For this it needs blanks there or the same letter (of another word) in the cell.
                    {
                        if (xx >= GameManager.Instance.GridCellCount) return false;  // Falling outside the grid. Hence placement unavailable.
                        if (wordMatrix[xx, y] != '\0') // If there is a character on that square then:
                        {
                            if (wordMatrix[xx, y] != word[i])   // If there is an overlap, then we see if the characters match. If matches, then it can still go there.
                            {
                                placeAvailable = false;
                                break;
                            }
                            else overlapped = true;
                        }
                    }

                    if (!placeAvailable)
                        return false;

                    // The first few words should be placed without overlapping.
                    if (currentWordCount < initWordCount && overlapped)
                        return false;

                    // If overlapping didn't occur after the maximum allowed non-overlapping words for the first few runs (e.g. first 5 words)
                    // then trigger a re-position attempt (by returning false to the calling method which will in turn trigger another search until
                    // an overlapping position is found.)
                    else if (currentWordCount >= initWordCount && !overlapped)
                        return false;

                    // If it is a right-direction, then there should not be any character in the adjacent cell to the left of the beginning of this word.
                    // E.g.: If CAT is to be placed, then it cannot be placed with another word CLASS as C L A S S C A T
                    bool leftFree = LeftCellFreeForRightDirectedWord(x, y, word);

                    // If it is a right-direction, then check if it can cross through another word. E.g.: if CAT is the current word,
                    // then check if there is a valid crossing through existing words on the board - VICINITY, STICK:
                    // V
                    // I   S
                    // C A T
                    // I   I
                    // N   C
                    // I   K
                    // T
                    // Y
                    bool topFree = TopCellFreeForRightDirectedWord(x, y, word);

                    // If it is a right-direction, then check if it can cross through another word. E.g.: if CAT is the current word,
                    // then check if there is a valid crossing through existing words on the board - VICINITY, STICK:
                    // V
                    // I   S
                    // C A T
                    // I   I
                    // N   C
                    // I   K
                    // T
                    // Y
                    bool bottomFree = BottomCellFreeForRightDirectedWord(x, y, word);

                    // If it is a right-direction, then there should not be any character in the adjacent cell to the right of the ending of this word.
                    // E.g.: If CAT is to be placed, then it cannot be placed with another word BUS as C A T B U S
                    bool rightMostFree = RightMostCellFreeForRightDirectedWord(x, y, word);

                    // If cells that need to be free are not free, then this word cannot be placed there.
                    if (!leftFree || !topFree || !bottomFree || !rightMostFree) return false;

                    // If all the cells are blank, or a non-conflicting overlap is available, then this word can be placed there. So place it.
                    for (int i = 0, j = x; i < word.Length; i++, j++)
                        wordMatrix[j, y] = word[i];
                    AddWordToList(word, wordClue, x, y, direction, attempts, false);
                    return true;
                case Direction.Down:
                    for (int i = 0, yy = y; i < word.Length; i++, yy++)     // First we check if the word can be placed in the array. For this it needs blanks there or the same letter (of another word) in the cell.
                    {
                        if (yy >= GameManager.Instance.GridCellCount) return false;      // Falling outside the grid. Hence placement unavailable.
                        if (wordMatrix[x, yy] != '\0')
                        {
                            if (wordMatrix[x, yy] != word[i])                   // If there is an overlap, then we see if the characters match. If matches, then it can still go there.
                            {
                                placeAvailable = false;
                                break;
                            }
                            else overlapped = true;
                        }
                    }

                    if (!placeAvailable)
                        return false;

                    // The first few words should be placed without overlapping.
                    if (currentWordCount < initWordCount && overlapped)
                        return false;

                    // If overlapping didn't occur after the maximum allowed non-overlapping words for the first few runs (e.g. first 5 words)
                    // then trigger a re-position attempt (by returning false to the calling method which will in turn trigger another search until
                    // an overlapping position is found.)
                    else if (currentWordCount >= initWordCount && !overlapped)
                        return false;

                    // If it is a right-direction, then check if it can cross through another word. E.g.: If STICK the current word, see if there
                    // is a valid crossing through existing words on the board - CAT, SKID:
                    //     S
                    // C A T
                    //     I
                    //     C
                    //   S K I D
                    leftFree = LeftCellFreeForDownDirectedWord(x, y, word);

                    // If it is a down-direction, then there should not be any character in the adjacent cell to the right of the beginning of this word.
                    // E.g.: If CAT is to be placed downwards, then it cannot be placed with another word CLASS as
                    // C C L A S S
                    // A
                    // T
                    bool rightFree = RightCellFreeForDownDirectedWord(x, y, word);

                    // If it is a down-direction, then there should not be any character in the adjacent cell to the top of the beginning of this word.
                    // E.g.: If CAT is to be placed downwards, then it cannot be placed with another word CLASS as
                    // C L A S S
                    // C
                    // A
                    // T
                    topFree = TopCellFreeForDownDirectedWord(x, y, word);

                    // If it is a down-direction, then there should not be any character in the adjacent cell to the bottom of the end of this word.
                    // E.g.: If CAT is to be placed downwards, then it cannot be placed with another word CLASS as                        
                    // C
                    // A
                    // T
                    // C L A S S
                    bool bottomMostBottomFree = BottomMostBottomCellFreeForDownDirectedWord(x, y, word);

                    // If cells that need to be free are not free, then this word cannot be placed there.
                    if (!leftFree || !rightFree || !topFree || !bottomMostBottomFree) return false;

                    // If all the cells are blank, or a non-conflicting overlap is available, then this word can be placed there. So place it.
                    for (int i = 0, j = y; i < word.Length; i++, j++)
                        wordMatrix[x, j] = word[i];
                    AddWordToList(word, wordClue, x, y, direction, attempts, false);
                    return true;
            }
            return false;   // Otherwise continue with a different place and index.
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in 'PlaceTheWords()' method of the 'GameEngine' class. Error msg: {e.Message}");
            return false;   // Otherwise continue with a different place and index.
        }
    }


    /// <summary>
    /// This method checks if any left-side characters of the word to be placed is a valid overlap from an existing ACROSS word on the board.
    /// E.g.: CART is to be placed downwards. It checks valid overlaps with existing right-directed words - ARC, PARTY.
    ///     A R C
    ///         A
    ///     P A R T Y
    ///         T
    /// </summary>
    /// <param name="x">Intended x-position of the word to be placed</param>
    /// <param name="y">Intended y-position of the word to be placed</param>
    /// <param name="word">The word to be placed. E.g.: CART</param>
    /// <returns></returns>
    bool LeftCellFreeForDownDirectedWord(int x, int y, string word)
    {
        try
        {
            if (x == 0) return true;
            bool isValid = true;
            if (x > 0)
            {
                for (int i = 0; i < word.Length; y++, i++)
                {
                    if (wordMatrix[x - 1, y] != '\0')
                        isValid = LegitimateOverlapOfAnExistingWord(x, y, word, Direction.Left);
                    if (!isValid) break;
                }
            }
            return isValid;
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in the LeftCellFreeForDownDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
            return false;
        }
    }



    /// <summary>
    /// This method checks if any there is any character to the left cell of the current word to place.
    /// E.g.: CAT is to be placed ACROSS, then there cannot be a letter to the left of CAT.
    /// ****************   P |__|
    /// **************** |__|  C   A   T
    /// ****************   A |__|
    /// </summary>
    /// <param name="x">Intended x-position of the word to be placed</param>
    /// <param name="y">Intended y-position of the word to be placed</param>
    /// <param name="word">The word to be placed. E.g.: CART</param>
    /// <returns></returns>
    bool LeftCellFreeForRightDirectedWord(int x, int y, string word)
    {
        try
        {
            if (x == 0) return true;
            if (x - 1 >= 0)
                return wordMatrix[x - 1, y] == '\0';
            return false;
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in the LeftCellFreeForRightDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// This method checks if any right-side characters of the word to be placed is a valid overlap from an existing ACROSS word on the board.
    /// E.g.: CART is to be placed downwards. It checks valid overlaps with existing right-directed words - ARC, PARTY.
    ///     A R C
    ///         A
    ///     P A R T Y
    ///         T
    /// </summary>
    /// <param name="x">Intended x-position of the word to be placed</param>
    /// <param name="y">Intended y-position of the word to be placed</param>
    /// <param name="word">The word to be placed. E.g.: CART</param>
    /// <returns></returns>
    bool RightCellFreeForDownDirectedWord(int x, int y, string word)
    {
        try
        {
            if (x == GameManager.Instance.GridCellCount) return true;
            bool isValid = true;
            if (x + 1 < GameManager.Instance.GridCellCount)
            {
                for (int i = 0; i < word.Length; y++, i++)
                {
                    if (wordMatrix[x + 1, y] != '\0')
                        isValid = LegitimateOverlapOfAnExistingWord(x, y, word, Direction.Right);
                    if (!isValid) break;
                }
            }
            return isValid;
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in the RightCellFreeForDownwardWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// This method checks if any there is any character to the left cell of the current word to place.
    /// E.g.: CAT is to be placed ACROSS, then there cannot be a letter to the right of CAT.
    /// ****************       |__|  P
    /// **************** C   A   T  |__|
    /// ****************       |__|
    /// </summary>
    /// <param name="x">Intended x-position of the word to be placed</param>
    /// <param name="y">Intended y-position of the word to be placed</param>
    /// <param name="word">The word to be placed. E.g.: CART</param>
    /// <returns></returns>
    bool RightMostCellFreeForRightDirectedWord(int x, int y, string word)
    {
        try
        {
            if (x + word.Length == GameManager.Instance.GridCellCount) return true;
            if (x + word.Length < GameManager.Instance.GridCellCount)
                return wordMatrix[x + word.Length, y] == '\0';
            return false;
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in the RightMostCellFreeForRightDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// If it is a DWON word, then there should not be any character in the adjacent cell to the top of the beginning of this word.
    /// E.g.: If CAT is to be placed downwards, then the top cell should be free:
    /// |___| C L A S S
    ///   C
    ///   A
    ///   T
    /// </summary>
    /// <param name="x">Intended x-position of the word to be placed</param>
    /// <param name="y">Intended y-position of the word to be placed</param>
    /// <param name="word">The word to be placed. E.g.: CART</param>
    /// <returns></returns>
    bool TopCellFreeForDownDirectedWord(int x, int y, string word)
    {
        try
        {
            if (y == 0) return true;
            if (y - 1 >= 0)
                return wordMatrix[x, y - 1] == '\0';
            return false;
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in the TopCellFreeForDownDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// This method checks if any top-side characters of the word to be placed is a valid overlap from an existing DOWB word on the board.
    /// E.g.: CART is to be placed ACROSS. It checks valid overlaps with existing DOWN words - ARC, PARTY.
    ///       A   P
    ///       R   A
    ///       C A R T
    ///           T
    ///           Y
    /// </summary>
    /// <param name="x">Intended x-position of the word to be placed</param>
    /// <param name="y">Intended y-position of the word to be placed</param>
    /// <param name="word">The word to be placed. E.g.: CART</param>
    /// <returns></returns>
    bool TopCellFreeForRightDirectedWord(int x, int y, string word)
    {
        try
        {
            if (y == 0) return true;
            bool isValid = true;
            if (y - 1 >= 0)
            {
                for (int i = 0; i < word.Length; x++, i++)
                {
                    if (wordMatrix[x, y - 1] != '\0')
                        isValid = LegitimateOverlapOfAnExistingWord(x, y, word, Direction.Up);
                    if (!isValid) break;
                }
            }
            return isValid;
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in the TopCellFreeForRightDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// This method checks if any top-side characters of the word to be placed is a valid overlap from an existing DOWB word on the board.
    /// E.g.: CART is to be placed ACROSS. It checks valid overlaps with existing DOWN words - SCOOP, PARTY.
    ///           P
    ///       S   A
    ///       C A R T
    ///       O   T
    ///       O   Y
    ///       P
    /// </summary>
    /// <param name="x">Intended x-position of the word to be placed</param>
    /// <param name="y">Intended y-position of the word to be placed</param>
    /// <param name="word">The word to be placed. E.g.: CART</param>
    /// <returns></returns>
    bool BottomCellFreeForRightDirectedWord(int x, int y, string word)
    {
        try
        {
            if (y == GameManager.Instance.GridCellCount) return true;
            bool isValid = true;
            if (y + 1 < GameManager.Instance.GridCellCount)
            {
                for (int i = 0; i < word.Length; x++, i++)
                {
                    if (wordMatrix[x, y + 1] != '\0')
                        isValid = LegitimateOverlapOfAnExistingWord(x, y, word, Direction.Down);
                    if (!isValid) break;
                }
            }
            return isValid;
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in the BottomCellFreeForRightDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// If it is a DOWN word, then there should not be any character in the adjacent cell to the bottom of the end of this word.
    /// E.g.: If CAT is to be placed downwards, then the bottom cell should be free:        
    ///   C
    ///   A
    ///   T
    /// |___| C L A S S
    /// </summary>
    /// <param name="x">Intended x-position of the word to be placed</param>
    /// <param name="y">Intended y-position of the word to be placed</param>
    /// <param name="word">The word to be placed. E.g.: CART</param>
    bool BottomMostBottomCellFreeForDownDirectedWord(int x, int y, string word)
    {
        try
        {
            if (y + word.Length == GameManager.Instance.GridCellCount) return true;
            if (y + word.Length < GameManager.Instance.GridCellCount)
                return wordMatrix[x, y + word.Length] == '\0';
            return false;
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in the BottomMostBottomCellFreeForDownDirectedWord() method of the 'GameEngine' class. x = {x}, y = {y}, word: {word}.\n\nError msg: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// This function checks if a word that is already to the left of the word rightfully passes through the word to be placed.
    /// </summary>
    /// <param name="x">Intended x-position of the word to be placed</param>
    /// <param name="y">Intended y-position of the word to be placed</param>
    /// <param name="direction">The direction to search for from the (x, y)</param>
    /// <returns></returns>
    bool LegitimateOverlapOfAnExistingWord(int x, int y, string word, Direction direction)
    {
        char[] chars = new char[GameManager.Instance.GridCellCount];
        int originalX = x, originalY = y;
        try
        {
            switch (direction)
            {
                case Direction.Left:
                    while (--x >= 0)
                        if (wordMatrix[x, y] == '\0') break;                                // First walk towards the left until you reach the beginning of the word that is already on the board.
                    ++x;

                    for (int i = 0; x < GameManager.Instance.GridCellCount && i < GameManager.Instance.GridCellCount; x++, i++) // Now walk towards right until you reach the end of the word that is already on the board.
                    {
                        if (wordMatrix[x, y] == '\0') break;
                        chars[i] = wordMatrix[x, y];
                    }

                    string str = new string(chars);
                    str = str.Trim('\0');
                    Word wordOnBoard = (Word)quizList.Find(a => a.WordSpelling == str);  // See if the characters form a valid word that is already on the board.
                    if (wordOnBoard == null) return false;                              // If this is not a word on the board, then this must be some random characters, hence not a legitimate word, hence this is a wrong placement.
                    if (wordOnBoard.WordDirection == Direction.Down) return false;      // If the word on the board is in parallel to the word on to be placed, then also this is a wrong placement as two words cannot be placed side by side in the same direction.
                    if (wordOnBoard.WordPosition.X + wordOnBoard.WordSpelling.Length == originalX) return false; // The word on the board ended just before the x-cordinate for the current word to place. Hence illegitimate.
                    return true;                                                        // Else, passed all validation checks for a legitimate overlap, hence return true.
                case Direction.Right:
                    while (--x >= 0)
                        if (wordMatrix[x, y] == '\0') break;                                // First walk towards the left until you reach the beginning of the word that is already on the board.
                    ++x;

                    for (int i = 0; x < GameManager.Instance.GridCellCount && i < GameManager.Instance.GridCellCount; x++, i++) // Now walk towards right until you reach the end of the word that is already on the board.
                    {
                        if (wordMatrix[x, y] == '\0') break;
                        chars[i] = wordMatrix[x, y];
                    }

                    str = new string(chars);
                    str = str.Trim('\0');
                    wordOnBoard = (Word)quizList.Find(a => a.WordSpelling == str);     // See if the characters form a valid word that is already on the board.
                    if (wordOnBoard == null) return false;                                      // If this is not a word on the board, then this must be some random characters, hence not a legitimate word, hence this is a wrong placement.
                    if (wordOnBoard.WordDirection == Direction.Down) return false;              // If the word on the board is in parallel to the word on to be placed, then also this is a wrong placement as two words cannot be placed side by side in the same direction.
                    if (wordOnBoard.WordPosition.X == originalX + 1) return false;                           // The word on the board starts right after the x-cordinate for the current word to place. Hence illegitimate.
                    return true;                                                                // Else, passed all validation checks for a legitimate overlap, hence return true.
                case Direction.Up:
                    while (--y >= 0)
                        if (wordMatrix[x, y] == '\0') break;                                        // First walk upwards until you reach the beginning of the word that is already on the board.
                    ++y;

                    for (int i = 0; y < GameManager.Instance.GridCellCount && i < GameManager.Instance.GridCellCount; y++, i++) // Now walk downwards until you reach the end of the word that is already on the board.
                    {
                        if (wordMatrix[x, y] == '\0') break;
                        chars[i] = wordMatrix[x, y];
                    }

                    str = new string(chars);
                    str = str.Trim('\0');
                    wordOnBoard = (Word)quizList.Find(a => a.WordSpelling == str);     // See if the characters form a valid word that is already on the board.
                    if (wordOnBoard == null) return false;                                      // If this is not a word on the board, then this must be some random characters, hence not a legitimate word, hence this is a wrong placement.
                    if (wordOnBoard.WordDirection == Direction.Right) return false;             // If the word on the board is in parallel to the word on to be placed, then also this is a wrong placement as two words cannot be placed side by side in the same direction.
                    if (wordOnBoard.WordPosition.Y + wordOnBoard.WordSpelling.Length == originalY) return false;     // The word on the board starts right below the y-cordinate for the current word to place. Hence illegitimate.
                    return true;                                                                // Else, passed all validation checks for a legitimate overlap, hence return true.
                case Direction.Down:
                    while (--y >= 0)
                        if (wordMatrix[x, y] == '\0') break;                                        // First walk upwards until you reach the beginning of the word that is already on the board.
                    ++y;

                    for (int i = 0; y < GameManager.Instance.GridCellCount && i < GameManager.Instance.GridCellCount; y++, i++) // Now walk downwards until you reach the end of the word that is already on the board.
                    {
                        if (wordMatrix[x, y] == '\0') break;
                        chars[i] = wordMatrix[x, y];
                    }

                    str = new string(chars);
                    str = str.Trim('\0');
                    wordOnBoard = (Word)quizList.Find(a => a.WordSpelling == str);     // See if the characters form a valid word that is already on the board.
                    if (wordOnBoard == null) return false;                                      // If this is not a word on the board, then this must be some random characters, hence not a legitimate word, hence this is a wrong placement.
                    if (wordOnBoard.WordDirection == Direction.Right) return false;             // If the word on the board is in parallel to the word on to be placed, then also this is a wrong placement as two words cannot be placed side by side in the same direction.
                    if (wordOnBoard.WordPosition.Y == originalY + 1) return false;                           // The word on the board starts right after the x-cordinate for the current word to place. Hence illegitimate.
                    return true;                                                                // Else, passed all validation checks for a legitimate overlap, hence return true.
            }
            return false;
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in the LegitimateOverlapOfAnExistingWord() method of the 'GameEngine' class.\n\n" +
                            $"Original x = {originalX}, Original y = {originalY}, x = {x}, y = {y}, word: {word}, Direction: {direction}." +
                            $"\n\nError msg: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Keeps the word and details in the collection.
    /// </summary>
    /// <param name="word"></param>
    /// <param name="wordClue"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="direction"></param>
    /// <param name="attempts"></param>
    /// <param name="failedMaxAttempts"></param>
    private void AddWordToList(string wordSpelling, Sprite wordClue, int x, int y, Direction direction, long attempts, bool failedMaxAttempts)
    {
        try
        {
            Word word = new Word();

            word.WordSpelling = wordSpelling;
            word.WordClue = wordClue;
            word.WordPosition = new Point(x, y);
            word.WordDirection = direction;
            word.AttemptsCount = attempts;
            word.FailedMaxAttempts = failedMaxAttempts;

            quizList.Add(word);
            DebugCrossword();
            //DebugList();

            
        }
        catch (System.Exception e)
        {
            Debug.Log($"An error occurred in 'AddWordToList()' method of the 'GameEngine' class. Error msg: {e.Message}");
        }
    }


    private void DebugCrossword()
    {
        string row;
        for (int x = 0; x < GameManager.Instance.GridCellCount; x++)
        {
            row = "";
            for (int y = 0; y < GameManager.Instance.GridCellCount; y++)
            {
                row += wordMatrix[x, y];    
            }
            Debug.Log(row);
        }
    }

    private void DebugList()
    {
        foreach (var word in quizList)
        {
            Debug.Log(word.WordSpelling + " " +  word.WordPosition.X + " " + word.WordPosition.Y);
        }
    }

    private void PlaceWordsOnScreen()
    {
        foreach (Word word in quizList)
        {
            if (!word.FailedMaxAttempts)
            {

            }
        }
    }




    
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
