using System.Collections.Generic;
using System.IO;
using System.Linq;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Utils;

namespace Anim_Helper.Controls;

internal class MainControl : IGameElement
{
    public MainControl()
    {
        _ribbon = new RibbonControl(OnNewImagesImported, OnNewFrames, OnReload, OnParserTypeChanged);
        _flipbook = new FlipbookControl();
        _gridParsePreview = new GridParsePreviewControl(OnGridParsePreviewSelectionChanged);
        _sideBar = new SideBarControl(OnGridConfigurationChanged, OnSideBarSelectionChanged);
        _isSideBarVisible = false;
        _isSideBarSelected = false;
        _isGridParsePreviewSelected = false;
        _gridConfiguration = new GridConfiguration(0, 0, 0, 0, 0, 0, 0, 0);
    }

    public void Update(GameTime iGameTime)
    {
        _ribbon.Update(iGameTime);
        
        if (_isSideBarSelected || _isGridParsePreviewSelected)
            _gridParsePreview.Update(iGameTime);
        else
            _flipbook.Update(iGameTime);

        if (_isSideBarVisible)
            _sideBar.Update(iGameTime);
    }

    public void Draw()
    {
        _ribbon.Draw();

        if (_isSideBarSelected || _isGridParsePreviewSelected)
            _gridParsePreview.Draw();
        else
            _flipbook.Draw();

        if (_isSideBarVisible)
            _sideBar.Draw();
    }

    private readonly RibbonControl _ribbon;
    private readonly FlipbookControl _flipbook;
    private readonly GridParsePreviewControl _gridParsePreview;
    private readonly SideBarControl _sideBar;

    private List<ImportedImage> _importedImages;

    private bool _isSideBarVisible;
    private bool _isSideBarSelected;
    private bool _isGridParsePreviewSelected;
    private GridConfiguration _gridConfiguration;

    private void OnNewImagesImported(List<ImportedImage> iImages)
    {
        _importedImages = iImages;

        var frameName = Path.GetFileNameWithoutExtension(_importedImages.FirstOrDefault().Path);

        var frames = _isSideBarVisible ? 
            GridParser.Parse(_importedImages.FirstOrDefault().Texture, frameName, _gridConfiguration) : 
            iImages.Select(i => new Sprite2D(Path.GetFileNameWithoutExtension(i.Path), i.Texture, i.Texture.Bounds)).ToList();
        OnNewFrames(frames);
    }

    private void OnReload()
    {
        OnNewImagesImported(_importedImages);
    }

    private void OnParserTypeChanged(bool iIsGrid)
    {
        _isSideBarVisible = iIsGrid;
        OnReload();
    }

    private void OnGridConfigurationChanged(GridConfiguration iNewConfiguration)
    {
        _gridConfiguration = iNewConfiguration;

        var frameName = Path.GetFileNameWithoutExtension(_importedImages.FirstOrDefault().Path);

        var newFrames = GridParser.Parse(_importedImages.FirstOrDefault().Texture, frameName, _gridConfiguration);
        OnNewFrames(newFrames);
    }

    private void OnNewFrames(List<Sprite2D> iFrames)
    {
        _ribbon.SetPreviewFrames(iFrames);
        _flipbook.SetFrames(iFrames);
        _gridParsePreview.SetImage(iFrames.FirstOrDefault()?.Texture);
    }

    private void OnSideBarSelectionChanged(bool iIsSelected)
    {
        _isSideBarSelected = iIsSelected;
    }

    private void OnGridParsePreviewSelectionChanged(bool iIsSelected)
    {
        _isGridParsePreviewSelected = iIsSelected;
    }
}