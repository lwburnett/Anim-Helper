using System.Collections.Generic;

namespace Utils;

internal struct GridConfiguration
{
    public GridConfiguration(
        int iCellWidth, 
        int iCellHeight, 
        int iNumCellsX, 
        int iNumCellsY, 
        int iMarginX, 
        int iMarginY, 
        int iSpacingX, 
        int iSpacingY)
    {
        CellWidth = iCellWidth;
        CellHeight = iCellHeight;
        NumCellsX = iNumCellsX;
        NumCellsY = iNumCellsY;
        MarginX = iMarginX;
        MarginY = iMarginY;
        SpacingX = iSpacingX;
        SpacingY = iSpacingY;
    }
    
    public int CellWidth { get; }

    public int CellHeight { get; }

    public int NumCellsX { get; }

    public int NumCellsY { get; }

    public int MarginX { get; }

    public int MarginY { get; }

    public int SpacingX { get; }

    public int SpacingY { get; }
}