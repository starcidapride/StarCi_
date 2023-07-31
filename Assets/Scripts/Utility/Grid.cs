using System;
using System.Collections.Generic;
using UnityEngine;
public class Cell
{
    public float Width { get; set; }
    public float Height { get; set; }
    public Vector2 Center { get; set; }

}
public class Position
{
    public int Row { get; set; }
    public int Column { get; set; }
}
public class Grid
{
    public static Dictionary<Position, Cell> SplitSpriteIntoGrids(Transform spriteObject, int numRows, int numCols)
    {
        if (numRows < 1 || numCols < 1)
        {
            throw new ArgumentOutOfRangeException("The number of rows and columns must not be less than 1");
        }

        var rect = spriteObject.GetComponent<RectTransform>();

        var width = rect.rect.width;
        var height = rect.rect.height;

        var cellWidth = width / numCols;
        var cellHeight = height / numRows;

        var map = new Dictionary<Position, Cell>();

        for (var i = 0; i < numRows; i++)
        {
            for (var j = 0; j < numCols; j++)
            {
                var center = new Vector2()
                {
                    x = (-width + (2 * j + 1) * cellWidth) / 2,
                    y = (height - (2 * i + 1) * cellHeight) / 2
                };

                var cell = new Cell()
                {
                    Center = center,
                    Width = cellWidth,
                    Height = cellHeight
                };
                var cellPosition = new Position()
                {
                    Row = i,
                    Column = j
                }
                ;
                map.Add(cellPosition, cell);
            }
        }

        return map;
    }

    public static Dictionary<Position, Cell> SplitSpriteIntoGridsWithMaxWidth(Transform spriteObject, int numRows, int numCols, int maxWidth, bool directionReversed = false)
    {
        if (numRows < 1 || numCols < 1)
        {
            throw new ArgumentOutOfRangeException("The number of rows and columns must not be less than 1");
        }

        var rect = spriteObject.GetComponent<RectTransform>();

        var width = rect.rect.width;
        var height = rect.rect.height;

        float cellWidth;

        if (maxWidth < width / numCols)
        {
            cellWidth = maxWidth;
        }
        else
        {
            cellWidth = (width - maxWidth) / (numCols - 1);
        }

        Debug.Log(cellWidth);

        var cellHeight = height / numRows;

        var map = new Dictionary<Position, Cell>();

        for (var i = 0; i < numRows; i++)
        {
            int startJ, endJ, increment;

            if (directionReversed)
            {
                startJ = numCols - 1;
                endJ = -1;
                increment = -1;
            }
            else
            {
                startJ = 0;
                endJ = numCols;
                increment = 1;
            }

            for (var j = startJ; j != endJ; j += increment)
            {
                var center = new Vector2()
                {
                    x = (numCols - (2 * j + 1)) * cellWidth / 2,
                    y = (height - (2 * i + 1) * cellHeight) / 2
                };

                var cell = new Cell()
                {
                    Center = center,
                    Width = cellWidth,
                    Height = cellHeight
                };
                var cellPosition = new Position()
                {
                    Row = i,
                    Column = j
                };
                map.Add(cellPosition, cell);
            }
        }

        return map;
    }

    public static Dictionary<int, Cell> SplitSpriteIntoIndexedGridsWithMaxWidth(Transform spriteObject, int numRows, int numCols, int maxWidth, bool directionReversed = false)
{
    var cells = SplitSpriteIntoGridsWithMaxWidth(spriteObject, numRows, numCols, maxWidth, directionReversed);
    var map = new Dictionary<int, Cell>();

    foreach (var cell in cells)
    {
        var position = cell.Key;
        var index = position.Column + position.Row * numCols;
        map.Add(index, cell.Value);
    }
    return map;
}

public static Dictionary<int, Cell> SplitSpriteIntoIndexedGrids(Transform spriteObject, int numRows, int numCols)
{
    var cells = SplitSpriteIntoGrids(spriteObject, numRows, numCols);
    var map = new Dictionary<int, Cell>();

    foreach (var cell in cells)
    {
        var position = cell.Key;
        var index = position.Column + position.Row * numCols;
        map.Add(index, cell.Value);
    }

    return map;
}



}
