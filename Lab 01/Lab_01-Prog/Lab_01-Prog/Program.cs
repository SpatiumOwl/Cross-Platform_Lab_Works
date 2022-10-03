namespace Lab_01_Prog
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
    public record Tile (int x, int y);
    public static bool IsValid(Tile tile, int min, int max)
    {
        return tile.x >= min && tile.x <= max && tile.y >= min && tile.y <= max;
    }
    public static List<Tile> GenerateHorseMoves(Tile tile)
    {
        List<Tile> result = new List<Tile>();
        if (IsValid(tile, 1, 10))

    }
}