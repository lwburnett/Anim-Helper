using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Controls;

internal class RibbonControl : IGameElement
{
    public RibbonControl(Action<List<ImportedImage>> iOnNewImportAction, Action<List<Sprite2D>> iOnFramesMoved, Action iOnReload, Action<bool> iOnParserTypeChanged)
    {
        _importButton = new TextButton(Settings.Layout.Ribbon.ImportButtonRect, "Import", OnImport);
        _reloadButton = new TextButton(Settings.Layout.Ribbon.RefreshButtonRect, "Reload", OnReload);
        _parserTypeButton = new ParseTypeButtonControl(Settings.Layout.Ribbon.ParserTypeRect, iOnParserTypeChanged);
        _framePreviews = new List<FramePreviewControl>();
        _onNewImportAction = iOnNewImportAction;
        _onFramesMoved = iOnFramesMoved;
        _onReload = iOnReload;
        _framesSet = false;

        _importDialogMutex = new object();
        _importDialogOpen = null;
        _importDialogResults = new List<string>();
    }

    public void Update(GameTime iGameTime)
    {
        bool? isDialogCurrentlyDisplayed;

        lock (_importDialogMutex)
        {
            isDialogCurrentlyDisplayed = _importDialogOpen;
        }

        if (!isDialogCurrentlyDisplayed.HasValue)
        {
            _importButton.Update(iGameTime);
            _reloadButton.Update(iGameTime);
            _parserTypeButton.Update(iGameTime);

            foreach (var framePreview in _framePreviews)
            {
                framePreview.Update(iGameTime);
            }

            if (_requestedMovementIndex.HasValue && _requestedMovementIsRight.HasValue)
            {
                var sourceIndex = _requestedMovementIndex.Value;
                var destinationIndex = _requestedMovementIsRight.Value ? _requestedMovementIndex.Value + 1 : _requestedMovementIndex.Value - 1;

                var sourceFrame = _framePreviews[sourceIndex];
                var destinationFrame = _framePreviews[destinationIndex];

                _framePreviews[sourceIndex] = destinationFrame;
                _framePreviews[destinationIndex] = sourceFrame;

                var newSourceCenter = new Vector2(
                    Settings.Layout.Ribbon.FrameFirstPosition.X + Settings.Layout.Ribbon.FrameSpacingX * destinationIndex,
                    Settings.Layout.Ribbon.FrameFirstPosition.Y);

                var newDestinationCenter = new Vector2(
                    Settings.Layout.Ribbon.FrameFirstPosition.X + Settings.Layout.Ribbon.FrameSpacingX * sourceIndex,
                    Settings.Layout.Ribbon.FrameFirstPosition.Y);

                sourceFrame.Move(destinationIndex, newSourceCenter);
                destinationFrame.Move(sourceIndex, newDestinationCenter);

                _requestedMovementIndex = null;
                _requestedMovementIsRight = null;

                var newFrames = _framePreviews.Select(fp => fp.GetSprite()).ToList();
                _onFramesMoved(newFrames);
            }
        }
        else
        {
            if (!isDialogCurrentlyDisplayed.Value)
            {
                _importDialogOpen = null;
                _importDialogThread.Join();

                if (_importDialogResults.Any())
                {
                    var newTextures = GetImagesFromPaths(_importDialogResults);
                    _importDialogResults.Clear();

                    _onNewImportAction(newTextures);
                }
            }
        }
    }

    public void Draw()
    {
        _importButton.Draw();

        foreach (var framePreview in _framePreviews)
        {
            framePreview.Draw();
        }

        if (_framesSet)
        {
            _reloadButton.Draw();
            _parserTypeButton.Draw();
        }
    }

    public void SetPreviewFrames(List<Sprite2D> iFrames)
    {
        var newFramePreviews = new List<FramePreviewControl>();

        for (var ii = 0; ii < iFrames.Count; ii++)
        {
            var center = new Vector2(
                Settings.Layout.Ribbon.FrameFirstPosition.X + Settings.Layout.Ribbon.FrameSpacingX * ii,
                Settings.Layout.Ribbon.FrameFirstPosition.Y);
            var newFrameControl = new FramePreviewControl(ii, center, iFrames[ii], OnMoveFrame);
            newFramePreviews.Add(newFrameControl);
        }

        _framePreviews.Clear();
        _framePreviews.AddRange(newFramePreviews);
        _framesSet = true;
    }

    private readonly IGameElement _importButton;
    private readonly IGameElement _reloadButton;
    private readonly IGameElement _parserTypeButton;
    private readonly List<FramePreviewControl> _framePreviews;
    private readonly Action<List<ImportedImage>> _onNewImportAction;
    private readonly Action<List<Sprite2D>> _onFramesMoved;
    private readonly Action _onReload;
    private bool _framesSet;

    private readonly object _importDialogMutex;
    private bool? _importDialogOpen;
    private List<string> _importDialogResults;
    private Thread _importDialogThread;

    private int? _requestedMovementIndex;
    private bool? _requestedMovementIsRight;

    private void OnImport(GameTime iGameTime)
    {
        bool isDialogCurrentlyDisplayed;

        lock (_importDialogMutex)
            isDialogCurrentlyDisplayed = _importDialogOpen.HasValue;

        if (isDialogCurrentlyDisplayed)
            return;

        _importDialogOpen = true;
        _importDialogResults.Clear();

        _importDialogThread = new Thread(RunImportDialog);
        _importDialogThread.SetApartmentState(ApartmentState.STA);
        _importDialogThread.Start();
    }

    private void RunImportDialog()
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Image Files|*.jpeg;*.jpg;*.png;*.gif;*.tif";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            lock (_importDialogMutex)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _framePreviews.Clear();
                    _importDialogResults = openFileDialog.FileNames.ToList();
                }

                _importDialogOpen = false;
            }
        }
    }

    private void OnReload(GameTime iGameTime)
    {
        _onReload();
    }

    private void OnMoveFrame(int iIndex, bool iRight)
    {
        if ((iIndex == 0 && !iRight) || (iIndex >= _framePreviews.Capacity - 1 && iRight))
            return;

        _requestedMovementIndex = iIndex;
        _requestedMovementIsRight = iRight;
    }

    private List<ImportedImage> GetImagesFromPaths(List<string> iPaths) => iPaths.Select(p => new ImportedImage(p, Texture2D.FromFile(GraphicsHelper.GetGraphicsDevice(), p))).ToList();
}