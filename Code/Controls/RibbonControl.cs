using System.IO;
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
    }

    public void Update(GameTime iGameTime)
    {
        _importButton.Update(iGameTime);
    }

    public void Draw()
    {
        _importButton.Draw();
    }

    private readonly IGameElement _importButton;

    private void OnImport(GameTime iGameTime)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Image Files|*.jpeg;*.jpg;*.png;*.gif;*.tif";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //todo load imported files
            }
        }
    }
}