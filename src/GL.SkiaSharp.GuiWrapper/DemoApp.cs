using System.Diagnostics;
using Gtk;
using SkiaSharp;

namespace GL.SkiaSharp.GuiWrapper;

public class DemoApp : SkiaUIWithTheme
{

    protected override void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y)
    {
        Debug.Assert(Window != null);

        canvas.Clear(SKColors.White);
        canvas.DrawCircle(
                Window.AllocatedWidth/2,
                Window.AllocatedHeight/2,
                Window.AllocatedWidth/2 - 50,
                Theme.GetPaint("blue"));
        canvas.DrawText( $"{Window.Title} (frame: {Frame}, mouse: {x},{y})",
                10f, 50f, SKTextAlign.Left,  Theme.DefaultFont, Theme.GetPaint("black") );
    }

    protected override void HandleButtonPress(Gdk.EventButton evt)
    {
    }

    protected override void HandleKeyPress(Gdk.EventKey evt)
    {
        if (evt.Key == Gdk.Key.q) Application.Quit();
    }

    protected override ITheme ThemeFactory()
    {
        var theme = new SkiaUITheme();
        theme.DefaultFont = theme.GetFont("Roboto", 20);
        theme.SetPaint("black", new SKPaint { Color = SKColors.Black });
        return theme;
    }
}

