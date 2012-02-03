using System;
using System.Collections.Generic;
using OmahaMTG.Challenge.Challenges;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OmahaMTG.Challenge.WordSearchImplementation;

namespace WordSearchTest
{
    ///<summary>
    ///This is a test class for WordSearchImplementationTest and is intended
    ///to contain all WordSearchImplementationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WordSearchImplementationTest
    {
        private TestContext testContextInstance;
        ///<summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        ///<summary>
        ///A test for SolveWordSearch
        ///</summary>
        [TestMethod()]
        public void SolveWordSearchTest()
        {
            Random rand = new Random();
            IWordSearchChallenge target = new WordSearchImplementation();
            //This is the original test case creation
            //Puzzle testPuzzle = Puzzle.CreatePuzzle(rand.Next(100, 1000), rand.Next(50, 300));
            Puzzle testPuzzle = Puzzle.CreatePuzzle(1000, 300);
            //I hav created a simple small puzzel to test against
            //Puzzle testPuzzle = Puzzle.CreatePuzzle(50, 25);

            /*char[,] arr = new char[,] { {'A', 'B', 'C', 'D' }, 
                                        {'E', 'F', 'G', 'H' }, 
                                        {'I', 'J', 'K', 'L' }, 
                                        {'M', 'N', 'O', 'P' }};
             * */
            //List<string> tempLst = new List<string>();
            //tempLst.Add("");

            //testPuzzle.Board = arr;     
            //testPuzzle.Words = tempLst;
            IEnumerable<FoundWord> actual = null;
            actual = target.SolveWordSearch(testPuzzle);

            PuzzleResults puzzleResult = testPuzzle.GetResults(actual);

            Assert.AreEqual(((List<FoundWord>)puzzleResult.MatchedWords).Count, testPuzzle.Words.Count);
        }
    }
}
