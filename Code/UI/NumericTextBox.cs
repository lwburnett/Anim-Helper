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
    public NumericTextBox(Rectangle iBounds, bool iCanBeNegative, Action<int> iOnValueChanged, float iFontScaling = 1f)
    {
        HitBox = iBounds;
        _canBeNegative = iCanBeNegative;
        _onValueChanged = iOnValueChanged;
        _fontScaling = iFontScaling;

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

        if (_isSelected)
        {
            var keyboardState = Keyboard.GetState();
            var pressedKeys = keyboardState.GetPressedKeys();

            var newValues = GetPressedNumericKeyValues(pressedKeys);

            if (pressedKeys.Contains(Keys.Subtract) && _canBeNegative && string.IsNullOrEmpty(_text))
                _text += "-";

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
        }

        base.Update(iGameTime);
    }

    public override void Draw()
    {
        GraphicsHelper.DrawTexture(_isSelected ? _selectedBackgroundTexture : _defaultBackgroundTexture, HitBox.Location.ToVector2());

        if (string.IsNullOrWhiteSpace(_text))
            return;

        var stringDimensions = GraphicsHelper.GetFont().MeasureString(_text) * _fontScaling;
        GraphicsHelper.DrawString(
            _text,
            new Vector2(
                HitBox.X + HitBox.Width - stringDimensions.X - Settings.Layout.TextBox.PaddingX, 
                HitBox.Y + (HitBox.Height - stringDimensions.Y) / 2f),
            Color.Black,
            _fontScaling);
    }

    protected sealed override Rectangle HitBox { get; set; }


    private readonly bool _canBeNegative;
    private readonly Action<int> _onValueChanged;
    private readonly float _fontScaling;

    private readonly Texture2D _defaultBackgroundTexture;
    private readonly Texture2D _selectedBackgroundTexture;

    private bool _isSelected;
    private string _text;
    private int _preEditValue;

    private List<bool> _keyPressBuffer = new() { false, false, false, false, false, false, false, false, false, false };

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
        var downKeys = Enumerable.Repeat(false, 10).ToList();

        foreach (var key in iPressedKeys)
        {
            switch (key)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    downKeys[0] = true;
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    downKeys[1] = true;
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    downKeys[2] = true;
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    downKeys[3] = true;
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    downKeys[4] = true;
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    downKeys[5] = true;
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    downKeys[6] = true;
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    downKeys[7] = true;
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    downKeys[8] = true;
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    downKeys[9] = true;
                    break;
            }
        }

        var pressedKeys = new List<int>();
        for (int ii = 0; ii < 10; ii++)
        {
            if (downKeys[ii] && !_keyPressBuffer[ii])
                _keyPressBuffer[ii] = true;
            else if (!downKeys[ii] && _keyPressBuffer[ii])
            {
                _keyPressBuffer[ii] = false;
                pressedKeys.Add(ii);
            }
        }

        return pressedKeys;
    }
}