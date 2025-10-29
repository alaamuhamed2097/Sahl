namespace Shared.Helpers
{
    // Custom comparer for hierarchical tree view serial numbers
    public class TreeViewSerialComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            var xParts = x.Split('.').Select(int.Parse).ToArray();
            var yParts = y.Split('.').Select(int.Parse).ToArray();

            int minLength = Math.Min(xParts.Length, yParts.Length);

            for (int i = 0; i < minLength; i++)
            {
                int comparison = xParts[i].CompareTo(yParts[i]);
                if (comparison != 0)
                    return comparison;
            }

            // If all compared parts are equal, the shorter one comes first
            return xParts.Length.CompareTo(yParts.Length);
        }
    }
}
