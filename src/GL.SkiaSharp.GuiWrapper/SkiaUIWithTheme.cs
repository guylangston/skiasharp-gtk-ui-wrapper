using System.Reflection;
using Gtk;
using SkiaSharp;

namespace GL.SkiaSharp.GuiWrapper;

public interface ITheme
{
    SKFont DefaultFont { get; }
    SKFont GetFont(string name, int size);
    SKPaint GetPaint(string name, SKPaint? defaultIfNotFound = null);
}

public abstract class SkiaUIWithTheme : SkiaUIBase
{
    ITheme? theme = null;
    public ITheme Theme => theme ?? throw new Exception("Call InitWindow before using Theme");

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

    public SkiaUITheme(string defFont, int defSize)
    {
        DefaultFont = GetFont(defFont, defSize);
    }

    public SKFont DefaultFont { get; set;}

    public SKFont GetFont(string name, int size)
    {
        var key = $"{name}_{size}";
        if (fonts.TryGetValue(key, out var hit)) return hit;
        return fonts[key] = new SKFont(SKTypeface.FromFamilyName(name), size);
    }

    public SKPaint GetPaint(string name, SKPaint? defaultIfNotFound = null)
    {
        if (paints.TryGetValue(name, out var hit)) return hit;

        var type = typeof(SKColors);
        foreach(var item in type.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (item.Name == name)
            {
                return SetPaint(name, new SKPaint { Color = (SKColor)item.GetValue(null) } );
            }
        }
        throw new KeyNotFoundException(name);
    }


    public SKPaint SetPaint(string name, SKPaint paint) => paints[name] = paint;
}


