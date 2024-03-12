using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;

namespace Anim_Helper.Controls;

internal class RibbonControl : IGameElement
{
    public RibbonControl()
    {
        _importButton = new TextButton(SettingsManager.LayoutSettings.ImportButtonRect, "Import", OnImport);
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
        }
        else
        {
            if (!isDialogCurrentlyDisplayed.Value)
            {
                _importDialogOpen = null;
                _importDialogResults.Clear(); // todo do stuff with
                _importDialogThread.Join();
            }
        }
    }

    public void Draw()
    {
        _importButton.Draw();
    }

    private readonly IGameElement _importButton;

    private readonly object _importDialogMutex;
    private bool? _importDialogOpen;
    private List<string> _importDialogResults;
    private Thread _importDialogThread;

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
                    _importDialogResults = openFileDialog.FileNames.ToList();
                }

                _importDialogOpen = false;
            }
        }
    }
}