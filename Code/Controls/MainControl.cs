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
        _sideBar = new SideBarControl(OnGridConfigurationChanged);
        _isSideBarVisible = false;
        _gridConfiguration = new GridConfiguration(0, 0, 0, 0, 0, 0, 0, 0);
    }

    public void Update(GameTime iGameTime)
    {
        _ribbon.Update(iGameTime);
        _flipbook.Update(iGameTime);

        if (_isSideBarVisible)
            _sideBar.Update(iGameTime);
    }

    public void Draw()
    {
        _ribbon.Draw();
        _flipbook.Draw();

        if (_isSideBarVisible)
            _sideBar.Draw();
    }

    private readonly RibbonControl _ribbon;
    private readonly FlipbookControl _flipbook;
    private readonly SideBarControl _sideBar;

    private List<ImportedImage> _importedImages;

    private bool _isSideBarVisible;
    private GridConfiguration _gridConfiguration;

    private void OnNewImagesImported(List<ImportedImage> iImages)
    {
        _importedImages = iImages;
        var frames = _isSideBarVisible ? 
            GridParser.Parse(_importedImages.FirstOrDefault().Texture, _importedImages.FirstOrDefault().Path, _gridConfiguration) : 
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
        var newFrames = GridParser.Parse(_importedImages.FirstOrDefault().Texture, _importedImages.FirstOrDefault().Path, _gridConfiguration);
        OnNewFrames(newFrames);
    }

    private void OnNewFrames(List<Sprite2D> iFrames)
    {
        _ribbon.SetPreviewFrames(iFrames);
        _flipbook.SetFrames(iFrames);
    }
}