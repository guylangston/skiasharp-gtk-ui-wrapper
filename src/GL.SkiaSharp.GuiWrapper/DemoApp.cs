using System.Diagnostics;
using Gdk;
using Gtk;
using SkiaSharp;

namespace GL.SkiaSharp.GuiWrapper;

public class DemoApp : SkiaUI
{
    class Scene1 : UIElementBase<DemoApp>
    {
        public Scene1(DemoApp app) : base(app) { }

        public override void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y)
        {
            var rect = canvas.DeviceClipBounds;
            canvas.Clear();
            canvas.DrawCircle(
                    rect.Width/2,
                    rect.Height/2,
                    rect.Width/2 - 50,
                    Theme.GetPaint("Blue"));
            canvas.DrawText( $"{Window?.Title} (lastKey: {App.lastKey} frame: {App.Frame}, mouse: {x},{y}) {GetType().Name.ToUpper()}",
                    10f, 50f, SKTextAlign.Left,  Theme.DefaultFont, Theme.GetPaint("Black") );
        }
    }
    class Scene2 : UIElementBase<DemoApp>
    {
        public Scene2(DemoApp app) : base(app) { }

        public override void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y)
        {
            var rect = canvas.DeviceClipBounds;
            canvas.Clear();
            canvas.DrawCircle(
                    rect.Width/3,
                    rect.Height/3,
                    rect.Width/3 - 50,
                    Theme.GetPaint("Orange"));
            canvas.DrawText( $"{Window?.Title} (frame: {App.Frame}, mouse: {x},{y}) {GetType().Name.ToUpper()}",
                    10f, 50f, SKTextAlign.Left,  Theme.DefaultFont, Theme.GetPaint("Black") );
        }
    }

    protected override ITheme ThemeFactory()
    {
        var theme = new SkiaUITheme("Roboto", 20);
        return theme;
    }

    IUIElement[]? scenes;
    int sceneIdx = 0;
    protected override IUIElement InitScene()
    {
        scenes = [new Scene1(this), new Scene2(this)];
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
}

