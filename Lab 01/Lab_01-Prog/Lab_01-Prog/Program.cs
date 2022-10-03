using System.Text;

namespace Lab_01_Prog
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Processing the file");
            try
            {
                ReadInputProduceAndWriteOutput(args);
                Console.WriteLine("Succesfully finished the calculations");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType() + ". " + ex.Message);
            }
        }
        public static void ReadInputProduceAndWriteOutput(string[] args)
        {
            string sourcePath = args[0];
            string destPath = args[1];

            string sourceContents = ReadContentsFromFile(sourcePath);

            var sourceDestList = GetSourceDestinationListFromFile(sourceContents);

            var results = CheckIfPossibleToReachFromList(sourceDestList);

            WriteResultsToFile(results, destPath);
        }
        public static string ReadContentsFromFile(string path)
        {
            StreamReader? sourceFile = new(path);
            if (sourceFile is null)
                throw new ArgumentNullException("File not found");

            string sourceFileContents = sourceFile.ReadToEnd();

            sourceFile.Close();

            return sourceFileContents;
        }
        public static List<KeyValuePair<Tile, Tile>> GetSourceDestinationListFromFile(string content)
        {
            List<KeyValuePair<Tile, Tile>> sourceDest = new List<KeyValuePair<Tile, Tile>>();

            string[] pairsInText = content.Split("\r\n");

            foreach (var textPair in pairsInText)
            {
                if (!ValidTextPair(textPair))
                    throw new ArgumentException("Incorrect pair: \"" + textPair + "\"");
                else
                    sourceDest.Add(ConvertTextToPair(textPair));
            }

            return sourceDest;
        }
        public static bool ValidTextPair(string textPair)
        {
            bool hasCorrectLength = textPair.Length == 6;

            if (!hasCorrectLength)
                return false;

            bool hasCorrectSeparator = textPair.Substring(2, 2) == ", ";
            bool hasValidNumbers = textPair[1] >= 49 && textPair[1] <= 56 && textPair[5] >= 49 && textPair[5] <= 56;
            bool hasValidLetters = textPair[0] >= 97 && textPair[0] <= 104 && textPair[4] >= 97 && textPair[4] <= 104;

            return hasCorrectSeparator && hasValidNumbers && hasValidLetters;
        }
        public static KeyValuePair<Tile, Tile> ConvertTextToPair(string textPair)
        {
            Tile source = new Tile(textPair[0] - 96, textPair[1] - 48);
            Tile destination = new Tile(textPair[4] - 96, textPair[5] - 48);

            return new KeyValuePair<Tile, Tile>(source, destination);
        }
        public static List<int> CheckIfPossibleToReachFromList(List<KeyValuePair<Tile, Tile>> sourceDest)
        {
            List<int> result = new List<int>();

            foreach (var pair in sourceDest)
                result.Add(CheckIfPossibleToReachFromSingle(pair.Key, pair.Value));

            return result;
        }
        public static int CheckIfPossibleToReachFromSingle(Tile position, Tile destination)
        {
            var firstMoves = GenerateValidHorseMovesFromSingle(position);

            if (TilePresentInList(destination, firstMoves))
                return 1;

            var secondMoves = GenerateValidHorseMovesFromList(firstMoves);

            if (TilePresentInList(destination, secondMoves))
                return 2;

            return -1;
        }
        public static bool TilePresentInList(Tile tileToCheck, List<Tile> tiles)
        {
            foreach (var tile in tiles)
                if (tileToCheck == tile)
                    return true;
            return false;
        }
        public static List<Tile> GenerateValidHorseMovesFromList(List<Tile> positions)
        {
            List<Tile> result = new List<Tile>();

            foreach (var position in positions)
                result.AddRange(GenerateValidHorseMovesFromSingle(position));

            return result;
        }
        public static List<Tile> GenerateValidHorseMovesFromSingle(Tile position)
        {
            List<Tile> result = new List<Tile>();

            if (!IsAValidTile(position))
                return result;

            var uncheckedMoves = GenerateHorseMoves(position);
            
            foreach (Tile move in uncheckedMoves)
                if (IsAValidTile(move))
                    result.Add(move);

            return result;
        }
        public static List<Tile> GenerateHorseMoves(Tile currentTile)
        {
            var result = new List<Tile>();

            result.Add(new Tile(currentTile.x + 2, currentTile.y + 1));
            result.Add(new Tile(currentTile.x + 2, currentTile.y - 1));
            result.Add(new Tile(currentTile.x - 2, currentTile.y - 1));
            result.Add(new Tile(currentTile.x - 2, currentTile.y + 1));
            result.Add(new Tile(currentTile.x + 1, currentTile.y + 2));
            result.Add(new Tile(currentTile.x + 1, currentTile.y - 2));
            result.Add(new Tile(currentTile.x - 1, currentTile.y - 2));
            result.Add(new Tile(currentTile.x - 1, currentTile.y + 2));

            return result;
        }
        public static bool IsAValidTile(Tile tile, int min = 1, int max = 8)
        {
            return tile.x >= min && tile.x <= max && tile.y >= min && tile.y <= max;
        }
        public static void WriteResultsToFile(List<int> results, string path)
        {
            StringBuilder builder = new();

            foreach (int result in results)
            {
                if (result == -1)
                    builder.Append("НІ\n");
                else
                    builder.Append(Convert.ToString(result) + "\n");
            }

            StreamWriter? resultsFile = new(path);

            resultsFile.Write(builder);

            resultsFile.Close();
        }
    }
    public record Tile (int x, int y);
}