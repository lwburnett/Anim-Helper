using System.Collections.Generic;

namespace Utils;

internal struct GridConfiguration
{
    public GridConfiguration(int iNumX, int iNumY, int iXOffset, int iYOffset, int iWidth, int iHeight, List<int> iCellPadding)
    {
        NumX = iNumX;
        NumY = iNumY;
        XOffset = iXOffset;
        YOffset = iYOffset;
        Width = iWidth;
        Height = iHeight;
        CellPadding = iCellPadding;
    }

    public int NumX { get; }

    public int NumY { get; }

    public int XOffset { get; }

    public int YOffset { get; }

    public int Width { get; }

    public int Height { get; }

    // Top, Right, Bottom, Left
    public List<int> CellPadding { get; }
}