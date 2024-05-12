using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utils;

namespace Anim_Helper.Utils;

internal static class GridParser
{
    public static List<Sprite2D> Parse(Texture2D iTexture, string iName, GridConfiguration iGridConfiguration)
    {
        var sprites = new List<Sprite2D>();

        for (int yy = 0; yy < iGridConfiguration.NumCellsY; yy++)
        for (int xx = 0; xx < iGridConfiguration.NumCellsX; xx++)
        {
            var thisLeft = iGridConfiguration.MarginX + 2 * xx * iGridConfiguration.SpacingX + xx * iGridConfiguration.CellWidth;
            var thisTop = iGridConfiguration.MarginY + 2 * yy * iGridConfiguration.SpacingY + yy * iGridConfiguration.CellHeight;

            var thisFrameNum = xx + iGridConfiguration.NumCellsX * yy;
            sprites.Add(new Sprite2D($"{iName}_{thisFrameNum}", iTexture, new Rectangle(thisLeft, thisTop, iGridConfiguration.CellWidth, iGridConfiguration.CellHeight)));
        }

        return sprites;
    }
}