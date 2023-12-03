

using System.Diagnostics;
using System.Text;

namespace Day3
{
    public static class Day3SchematicCalculator
    {
        public static int GetSumPartNumbers(string input)
        {
            var map = Parse(input);

            var parts = new List<Part>();
            // for each row of the map we'll look for special characters and then check for parts close to it
            
            for (int rowNum = 0; rowNum < map.Count; rowNum++)
            {
                List<char>? row = map[rowNum];
                for (int colNum = 0; colNum < row.Count; colNum++)
                {
                    char c = row[colNum];
                    if (!char.IsDigit(c) && c != '.')
                    {
                        CheckAndAdd(rowNum, colNum, parts, map);
                    }
                }
            }
            var inter = parts.DistinctBy(x => new { x.RowNum, x.ColNum, x.PartNumber });
            return inter.Select(x => x.PartNumber).Sum();
        }

        public static int GetSumGearNumbers(string input)
        {
            var map = Parse(input);
            var gears = new List<Gear>();
            for (int rowNum = 0; rowNum < map.Count; rowNum++)
            {
                List<char>? row = map[rowNum];
                for (int colNum = 0; colNum < row.Count; colNum++)
                {
                    char c = row[colNum];
                    if (c == '*')
                    {
                        CheckAndAddGearRatio(rowNum, colNum, gears, map);
                    }
                }
            }

            return gears.Sum(x => x.Ratio);
        }

        private static void CheckAndAddGearRatio(int rowNum, int colNum, List<Gear> gears, List<List<char>> map)
        {
            var partsAroundGear = new List<Part>();
            // Left
            CheckAddInternal(rowNum, colNum - 1, partsAroundGear, map);
            // Right                                        
            CheckAddInternal(rowNum, colNum + 1, partsAroundGear, map);
            // Up                                           
            CheckAddInternal(rowNum - 1, colNum, partsAroundGear, map);
            // Down                                         
            CheckAddInternal(rowNum + 1, colNum, partsAroundGear, map);

            // Diagonals
            CheckAddInternal(rowNum - 1, colNum - 1, partsAroundGear, map);
            CheckAddInternal(rowNum - 1, colNum + 1, partsAroundGear, map);
            CheckAddInternal(rowNum + 1, colNum - 1, partsAroundGear, map);
            CheckAddInternal(rowNum + 1, colNum + 1, partsAroundGear, map);
            var distinct = partsAroundGear.DistinctBy(x => new { x.RowNum, x.ColNum, x.PartNumber }).ToList();
            if (distinct.Count == 2)
            {
                gears.Add(new Gear
                {
                    ColNum = colNum,
                    RowNum = rowNum,
                    Ratio = distinct[0].PartNumber * distinct[1].PartNumber
                });
            }
        }

        private static void CheckAndAdd(int rowNum, int colNum, List<Part> parts, List<List<char>> map)
        {
            // Left
            CheckAddInternal(rowNum, colNum - 1, parts, map);
            // Right                                        
            CheckAddInternal(rowNum, colNum + 1, parts, map);
            // Up                                           
            CheckAddInternal(rowNum - 1, colNum, parts, map);
            // Down                                         
            CheckAddInternal(rowNum + 1, colNum, parts, map);

            // Diagonals
            CheckAddInternal(rowNum-1, colNum - 1, parts, map);
            CheckAddInternal(rowNum-1, colNum + 1, parts, map);
            CheckAddInternal(rowNum+1, colNum - 1, parts, map);
            CheckAddInternal(rowNum+1, colNum + 1, parts, map);
        }

        private static void CheckAddInternal(int rowNum, int colNum, List<Part> parts, List<List<char>> map)
        {
            if (rowNum < 0 || rowNum >= map.Count)
            {
                return;
            }
            if (colNum < 0 || colNum >= map[rowNum].Count)
            {
                return;
            }
            var c = map[rowNum][colNum];
            if (c == '.')
            {
                return;
            }
            if (char.IsDigit(c))
            {
                parts.Add(ExpandDigits(rowNum, colNum, map));
            }
        }

        private static Part ExpandDigits(int rowNum, int colNum, List<List<char>> map)
        {
            // search left and right
            var minCol = colNum;
            while (minCol > 0 && char.IsDigit(map[rowNum][minCol - 1]))
            {
                minCol--;
            }

            var maxCol = colNum;
            while (maxCol < map[rowNum].Count - 1 && char.IsDigit(map[rowNum][maxCol + 1]))
            {
                maxCol++;
            }

            // now add all the digits to a string and convert to int
            var digits = new StringBuilder();
            for (int i = minCol; i <= maxCol; i++)
            {
                digits.Append(map[rowNum][i]);
            }
            return new Part { ColNum = minCol, RowNum = rowNum, PartNumber = int.Parse(digits.ToString()) };
        }

        private static List<List<char>> Parse(string input)
        {
            var map = new List<List<char>>();
            foreach (var line in input.Split(Environment.NewLine))
            {
                var mapRow = new List<char>();
                map.Add(mapRow);
                foreach (char c in line)
                {
                    mapRow.Add(c);
                }
            }
            return map;
        }


    }

    [DebuggerDisplay("{RowNum}:{ColNum} = {PartNumber}")]
    internal class Part
    {
        public int PartNumber { get; set; }
        public int ColNum { get; set; }
        public int RowNum { get; set; }
    }

    [DebuggerDisplay("{RowNum}:{ColNum} = {GearRatio}")]
    internal class Gear
    {
        public int Ratio { get; set; }
        public int ColNum { get; set; }
        public int RowNum { get; set; }
    }
}
