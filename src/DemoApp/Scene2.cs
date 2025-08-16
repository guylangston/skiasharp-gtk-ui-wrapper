using SkiaSharp;
using GL.SkiaSharp.GuiWrapper;

namespace DemoApp;

class Scene2 : UIElementBase<DemoApp, SkiaUITheme>
{
    public Scene2(DemoApp app) : base(app) { }

    public override void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y)
    {
        var rect = canvas.DeviceClipBounds;
        canvas.DrawCircle(
                rect.Width/3,
                rect.Height/3,
                rect.Width/3 - 50,
                Theme.GetPaint("Orange"));
        canvas.DrawText( $"{Window?.Title} (frame: {App.Frame}, mouse: {x},{y}) {GetType().Name.ToUpper()}",
                10f, 50f, SKTextAlign.Left,  Theme.DefaultFont, Theme.GetPaint("Black") );
    }
}


