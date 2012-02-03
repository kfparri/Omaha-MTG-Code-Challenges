using System.Collections.Generic;
using OmahaMTG.Challenge.Challenges;

namespace OmahaMTG.Challenge.WordSearchImplementation
{
    public class WordSearchImplementation : IWordSearchChallenge
    {
        public string AuthorNotes
        {
            get
            {
                return string.Empty;
            }
        }

        public IEnumerable<FoundWord> SolveWordSearch(Puzzle puzzle)
        {
            ushort puzzleSize = (ushort)puzzle.Board.GetLength(0);
            FoundWord tempWord;

            //Capital letters in ascii are 65 more than the index of the array e.g. A == char[0] == A.toascii - 65;
            List<FoundWord> returnValue = new List<FoundWord>();
            
            //this is a two dimensional array of list of ushorts, each row represents the row of the board and each column
            // represents a letter in the alphabet, the array will be the location(s) of that letter in the row.
            List<ushort>[,] lookupTable = new List<ushort>[puzzleSize, 26];

            //build the lookup table...
            for (ushort i = 0; i < puzzleSize; i++)
            {
                for (ushort temp = 0; temp < 26; temp++)
                    lookupTable[i, temp] = new List<ushort>();

                for (ushort j = 0; j < puzzleSize; j++)
                {
                    lookupTable[i, getIndex(puzzle.Board[i, j])].Add(j);
                }
            }


            //now search through the words in the puzzle...
            for (ushort index = 0; index < puzzle.Words.Count; index++)
            {
                //first turn the string into a character array
                char[] arrSearch = puzzle.Words[index].ToCharArray();
                ushort lookupIndex = getIndex(arrSearch[0]);
                bool wordFound = false;

                for (ushort rowSearch = 0; rowSearch < puzzleSize; rowSearch++)
                {
                    if (wordFound)
                        break;

                    tempWord = new FoundWord();
                    tempWord.Word = puzzle.Words[index];

                    if (lookupTable[rowSearch, lookupIndex].Count != 0)                              
                    {
                        //here we look around the letter for the next letter in the word.
                        foreach (ushort item in lookupTable[rowSearch, lookupIndex])
                        {
                            if (checkWord(rowSearch, item, arrSearch, tempWord, puzzle, puzzleSize) != null)
                            {
                                returnValue.Add(tempWord);
                                tempWord = null;
                                wordFound = true;
                                break;
                            }
                        }
                    }
                }
            }

            return returnValue;
        }

        private FoundWord checkWord(ushort row, ushort firstCol, char[] word, FoundWord found, Puzzle pzl, ushort pzSize)
        {
            //This will take the work and compare it to the values around it to see if the next letter in the puzzle is part of the word.
            //since we know that the first letter will match, we must now look in all 8 directions for the next letter
            //starting with the top left area see if there is enough space for the word this is determined by subtracting the length of the word from the row and colum
            // for the top, determine if the word is less than the column number
            // for the top right, add the word length to the column and subtract from the row and ensure the row number is not 0 and see if those numbers are less than the puzzle dimensions.
            // for the left, check to see if the first letter column is more than the world length
            // for the right, check to see if the length is less than the remaining space on the right.
            // for the botton right, add the length of the word to both the row and colum
            // for the bottom, add length to the row
            // for the bottom left add to the row and subtract from the column

            ushort len = (ushort)word.Length;
            bool blnFound = false;
            int lastRow, lastCol;

            //first, check top left...
            if (row + 1 - len >= 0 && firstCol + 1 - len >= 0)
            {
                //it is possible for the word to go this way, search it
                // [val -1, val -1]
                blnFound = true;
                lastCol = firstCol;
                lastRow = row;

                for (ushort i = 1; i < len; i++)
                {
                    lastRow = row - i;
                    lastCol = firstCol - i;                 

                    if (pzl.Board[lastRow, lastCol] != word[i])
                    {
                        blnFound = false;
                        break;
                    }
                }

                if (blnFound)
                {
                    // we have found the word! fill the foundWord
                    found.StartingColumn = firstCol;
                    found.StartingRow = row;
                    found.EndingColumn = lastCol; //firstCol - len - 1;
                    found.EndingRow = lastRow; //row - len - 1;
                    return found; // true;
                }
            }

            if (row + 1 >= len) //top
            {
                blnFound = true;
                lastCol = firstCol;
                lastRow = row;

                for (ushort i = 1; i < len; i++)
                {
                    lastRow = row - i;               

                    if (pzl.Board[lastRow, lastCol] != word[i])
                    {
                        blnFound = false;
                        break;
                    }
                }

                if (blnFound)
                {
                    found.StartingRow = row;
                    found.StartingColumn = firstCol;
                    found.EndingRow = lastRow;
                    found.EndingColumn = lastCol;
                    return found; // true;
                }
            }

            if (row + 1 - len >= 0 && firstCol + len <= pzSize) // top right
            {
                blnFound = true;
                lastCol = firstCol;
                lastRow = row;

                for (ushort i = 1; i < len; i++)
                {
                    lastCol = firstCol + i;
                    lastRow = row - i;

                    if (pzl.Board[lastRow, lastCol] != word[i])
                    {
                        blnFound = false;
                        break;
                    }
                }

                if (blnFound)
                {
                    found.StartingColumn = firstCol;
                    found.StartingRow = row;
                    found.EndingColumn = lastCol; // firstCol + len;
                    found.EndingRow = lastRow; // row - len;
                    return found; // true;
                }
            }

            if (firstCol + 1 >= len) // left
            {
                blnFound = true;
                lastCol = firstCol;
                lastRow = row;

                for (ushort i = 1; i < len; i++)
                {
                    lastCol = firstCol - i;

                    if (pzl.Board[lastRow, lastCol] != word[i])
                    {
                        blnFound = false;
                        break;
                    }
                }

                if (blnFound)
                {
                    found.StartingRow = row;
                    found.StartingColumn = firstCol;
                    found.EndingColumn = lastCol;
                    found.EndingRow = lastRow;
                    return found; // true;
                }
            }

            if (firstCol + len <= pzSize) //right
            {
                blnFound = true;
                lastCol = firstCol;
                lastRow = row;

                for (ushort i = 1; i < len; i++)
                {
                    lastCol = firstCol + i;

                    if (pzl.Board[lastRow, lastCol] != word[i])
                    {
                        blnFound = false;
                        break;
                    }
                }

                if (blnFound)
                {
                    found.StartingRow = row;
                    found.StartingColumn = firstCol;
                    found.EndingColumn = lastCol; // firstCol + len;
                    found.EndingRow = lastRow; // row;
                    return found; // true;
                }
            }

            if (row + len <= pzSize && firstCol + len <= pzSize) // bottom right
            {
                blnFound = true;
                lastCol = firstCol;
                lastRow = row;

                for (ushort i = 1; i < len; i++)
                {
                    lastRow = row + i;
                    lastCol = firstCol + i;

                    if (pzl.Board[lastRow, lastCol] != word[i])
                    {
                        blnFound = false;
                        break;
                    }
                }

                if (blnFound)
                {
                    found.StartingRow = row;
                    found.StartingColumn = firstCol;
                    found.EndingColumn = lastCol; // firstCol + len;
                    found.EndingRow = lastRow; // row + len;
                    return found; // true;
                }
            }

            if (row + len <= pzSize) //bottom 
            {
                blnFound = true;
                lastCol = firstCol;
                lastRow = row;

                for (ushort i = 1; i < len; i++)
                {
                    lastRow = row + i;

                    if (pzl.Board[lastRow, lastCol] != word[i])
                    {
                        blnFound = false;
                        break;
                    }
                }

                if (blnFound)
                {
                    found.StartingRow = row;
                    found.StartingColumn = firstCol;
                    found.EndingColumn = lastCol;
                    found.EndingRow = lastRow;
                    return found; // true;
                }
            }

            if (row + len <= pzSize && firstCol + 1 - len >= 0) // bottom left
            {
                blnFound = true;
                lastCol = firstCol;
                lastRow = row;

                for (ushort i = 1; i < len; i++)
                {
                    lastRow = row + i;
                    lastCol = firstCol - i;

                    if (pzl.Board[lastRow, lastCol] != word[i])
                    {
                        blnFound = false;
                        break;
                    }
                }

                if (blnFound)
                {
                    found.StartingRow = row;
                    found.StartingColumn = firstCol;
                    found.EndingColumn = lastCol; // firstCol - len;
                    found.EndingRow = lastRow; // row - len;
                    return found; // true;
                }
            }

            return null; //false;
            
        }

        private ushort getIndex(char value)
        {
            int i = (int)value - 65;
            return (ushort)i;
        }
    }
}
