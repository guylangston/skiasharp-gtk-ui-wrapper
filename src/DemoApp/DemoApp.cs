using Gdk;
using Gtk;
using SkiaSharp;
using GL.SkiaSharp.GuiWrapper;

namespace DemoApp;

public class DemoApp : SkiaUI<SkiaUITheme>
{
    protected override SkiaUITheme ThemeFactory()
    {
        var theme = new SkiaUITheme("Roboto", 20);
        return theme;
    }

    IUIElement[]? scenes;
    int sceneIdx = 0;
    protected override IUIElement InitScene()
    {
        scenes = [new Scene1(this), new Scene2(this), new SceneBounce(this)];
        sceneIdx = 0;
        return scenes[0];
    }

    public Gdk.Key? lastKey;
    protected override void HandleKeyPress(EventKey evt)
    {
        if (evt.Key == Gdk.Key.q)
        {
            Application.Quit();
        }
        if (evt.Key == Gdk.Key.Right && scenes != null)
        {
            sceneIdx++;
            if (sceneIdx >= scenes.Length) sceneIdx = 0;
            Scene = scenes[sceneIdx];
        }
        if (evt.Key == Gdk.Key.Left && scenes != null)
        {
            sceneIdx--;
            if (sceneIdx < 0) sceneIdx = scenes.Length-1;
            Scene = scenes[sceneIdx];
        }
        base.HandleKeyPress(evt);
        lastKey = evt.Key;
    }

    protected override void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y)
    {
        base.DrawFrame(surface, canvas, x, y);
        canvas.DrawText( $"{Scene?.GetType().Name}:{sceneIdx} lastKey: {lastKey}",
                    10f, canvas.LocalClipBounds.Bottom - 20 , SKTextAlign.Left,  Theme.DefaultFont, Theme.GetPaint("Black") );
    }

}

