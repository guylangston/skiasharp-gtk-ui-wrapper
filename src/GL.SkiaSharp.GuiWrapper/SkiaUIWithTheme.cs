using Gtk;
using SkiaSharp;

namespace GL.SkiaSharp.GuiWrapper;

public interface ITheme
{
    SKFont DefaultFont { get; }
    SKFont GetFont(string name, int size);
    SKPaint GetPaint(string name);
}

public abstract class SkiaUIWithTheme : SkiaUI
{
    ITheme? theme = null;
    protected ITheme Theme => theme ?? throw new Exception("Call InitWindow before using Theme");

    protected override Window InitWindow()
    {
        var init = base.InitWindow();
        theme = ThemeFactory();
        return init;
    }

    protected abstract ITheme ThemeFactory();
}

public class SkiaUITheme : ITheme
{
    Dictionary<string, SKFont> fonts = new();
    Dictionary<string, SKPaint> paints = new();

    public SKFont? DefaultFont { get; set; }

    public SKFont GetFont(string name, int size)
    {
        var key = $"{name}_{size}";
        if (fonts.TryGetValue(key, out var hit)) return hit;
        return fonts[key] = new SKFont(SKTypeface.FromFamilyName(name), size);
    }

    public SKPaint GetPaint(string name)
    {
        if (paints.TryGetValue(name, out var hit)) return hit;

        // if (Enum.TryParse<SKColors>(name, out var clr))
        // {
        //     return SetPaint(name, new SKPaint { Color = SKColors.21:08
        // }
    }

    public void SetPaint(string name, SKPaint paint)
    {
        paints[name] = paint;
    }
}


