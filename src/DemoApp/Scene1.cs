using SkiaSharp;
using GL.SkiaSharp.GuiWrapper;

namespace DemoApp;

class Scene1 : UIElementBase<DemoApp, SkiaUITheme>
{
    public Scene1(DemoApp app) : base(app) { }

    public override void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y)
    {
        var rect = canvas.DeviceClipBounds;
        canvas.DrawCircle(
                rect.Width/2,
                rect.Height/2,
                rect.Width/2 - 50,
                Theme.GetPaint("Blue"));
        canvas.DrawText( $"{Window?.Title} (lastKey: {App.lastKey} frame: {App.Frame}, mouse: {x},{y}) {GetType().Name.ToUpper()}",
                10f, 50f, SKTextAlign.Left,  Theme.DefaultFont, Theme.GetPaint("Black") );
    }
}


