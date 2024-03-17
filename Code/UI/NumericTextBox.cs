using System;
using System.Collections.Generic;
using System.Linq;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Anim_Helper.UI;

internal class NumericTextBox : MouseSensitiveElementBase
{
    public NumericTextBox(Rectangle iBounds, bool iCanBeNegative, Action<int> iOnValueChanged)
    {
        HitBox = iBounds;
        _canBeNegative = iCanBeNegative;
        _onValueChanged = iOnValueChanged;

        var backgroundDataSize = iBounds.Width * iBounds.Height;
        var backgroundDefaultColorData = new Color[backgroundDataSize];
        var backgroundSelectedColorData = new Color[backgroundDataSize];

        for (var ii = 0; ii < backgroundDataSize; ii++)
        {
            backgroundDefaultColorData[ii] = Settings.Colors.TextBoxDefault;
            backgroundSelectedColorData[ii] = Settings.Colors.TextBoxSelected;
        }

        _defaultBackgroundTexture = GraphicsHelper.CreateTexture(backgroundDefaultColorData, iBounds.Width, iBounds.Height);
        _selectedBackgroundTexture = GraphicsHelper.CreateTexture(backgroundSelectedColorData, iBounds.Width, iBounds.Height);

        _isSelected = false;
        _text = "0";
        _preEditValue = 0;
    }

    public override void Update(GameTime iGameTime)
    {
        var mouseState = Mouse.GetState();
        if (_isSelected && mouseState.LeftButton == ButtonState.Pressed && !IsOverlappingWithMouse(mouseState.Position))
        {
            HandleNewValue(_text, _preEditValue);
            _isSelected = false;
        }
        
        var keyboardState = Keyboard.GetState();
        var pressedKeys = keyboardState.GetPressedKeys();

        var newValues = GetPressedNumericKeyValues(pressedKeys);

        foreach (var newValue in newValues)
        {
            _text += newValue.ToString();
        }

        if (pressedKeys.Contains(Keys.Back) && !string.IsNullOrEmpty(_text))
        {
            _text = _text.Substring(0, _text.Length - 1);
        }

        if (pressedKeys.Contains(Keys.Enter))
            HandleNewValue(_text, _preEditValue);

        base.Update(iGameTime);
    }

    public override void Draw()
    {
        GraphicsHelper.DrawTexture(_isSelected ? _selectedBackgroundTexture : _defaultBackgroundTexture, HitBox.Location.ToVector2());

        if (string.IsNullOrWhiteSpace(_text))
            return;

        var stringDimensions = GraphicsHelper.GetFont().MeasureString(_text);
        GraphicsHelper.DrawString(
            _text,
            new Vector2(
                HitBox.X + HitBox.Width - stringDimensions.X - Settings.Layout.TextBox.PaddingX, 
                HitBox.Y + (HitBox.Height - stringDimensions.Y) / 2f),
            Color.Black);
    }

    protected sealed override Rectangle HitBox { get; set; }

    private readonly bool _canBeNegative;
    private readonly Action<int> _onValueChanged;

    private readonly Texture2D _defaultBackgroundTexture;
    private readonly Texture2D _selectedBackgroundTexture;

    private bool _isSelected;
    private string _text;
    private int _preEditValue;

    protected override void OnRelease(GameTime iGameTime)
    {
        _isSelected = true;

        base.OnRelease(iGameTime);
    }

    private void HandleNewValue(string iValue, int iFallbackValue)
    {
        if (!int.TryParse(iValue, out var parsedValue))
            parsedValue = iFallbackValue;

        if (parsedValue < 0 && !_canBeNegative)
            parsedValue = Math.Max(iFallbackValue, 0);

        _text = parsedValue.ToString();
        _preEditValue = parsedValue;

        _onValueChanged(parsedValue);
    }

    private List<int> GetPressedNumericKeyValues(Keys[] iPressedKeys)
    {
        var values = new List<int>();

        foreach (var key in iPressedKeys)
        {
            switch (key)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    values.Add(0);
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    values.Add(1);
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    values.Add(2);
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    values.Add(3);
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    values.Add(4);
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    values.Add(5);
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    values.Add(6);
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    values.Add(7);
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    values.Add(8);
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    values.Add(9);
                    break;
            }
        }

        return values;
    }
}