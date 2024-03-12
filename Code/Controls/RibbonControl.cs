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
    public RibbonControl(Action<List<Texture2D>> iOnNewSpritesAction)
    {
        _importButton = new TextButton(Settings.Layout.Ribbon.ImportButtonRect, "Import", OnImport);
        _framePreviews = new List<FramePreviewControl>();
        _onNewSpritesAction = iOnNewSpritesAction;

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
            }
        }
        else
        {
            if (!isDialogCurrentlyDisplayed.Value)
            {
                _importDialogOpen = null;
                _importDialogThread.Join();
                
                _framePreviews.Clear();

                var newSprites = new List<Texture2D>();

                for (var ii = 0; ii < _importDialogResults.Count; ii++)
                {
                    var path = _importDialogResults[ii];

                    var center = new Vector2(
                        Settings.Layout.Ribbon.FrameFirstPosition.X + Settings.Layout.Ribbon.FrameSpacingX * ii,
                        Settings.Layout.Ribbon.FrameFirstPosition.Y);
                    var newFrameControl = new FramePreviewControl(ii, center, path, OnMoveFrame);
                    _framePreviews.Add(newFrameControl);

                    newSprites.Add(newFrameControl.GetSprite());
                }

                _importDialogResults.Clear();

                _onNewSpritesAction(newSprites);
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
    }

    private readonly IGameElement _importButton;
    private readonly List<FramePreviewControl> _framePreviews;
    private readonly Action<List<Texture2D>> _onNewSpritesAction;

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

        _framePreviews.Clear();

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
                    _importDialogResults = openFileDialog.FileNames.ToList();
                }

                _importDialogOpen = false;
            }
        }
    }

    private void OnMoveFrame(int iIndex, bool iRight)
    {
        if ((iIndex == 0 && !iRight) || (iIndex >= _framePreviews.Capacity - 1 && iRight))
            return;

        _requestedMovementIndex = iIndex;
        _requestedMovementIsRight = iRight;
    }
}