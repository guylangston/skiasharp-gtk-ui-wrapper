using Gtk;
using SkiaSharp;

namespace GL.SkiaSharp.GuiWrapper;

/// <summary>Use generics so that the theme can have well-known/strongly types properties on top of GetPaint(name)</summary>
public abstract class SkiaUI<TTheme> : SkiaUIWithTheme<TTheme> where TTheme: ITheme
{
    public IUIElement? Scene { get; set; }

    protected override Window InitWindow()
    {
        var init = base.InitWindow();
        Scene = InitScene();
        return init;
    }

    protected abstract IUIElement InitScene();
    protected override void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y) => Scene?.DrawFrame(surface, canvas, x, y);
    protected override void Execute(IUICommand cmd) => Scene?.Execute(cmd);
    protected override void HandleButtonPress(Gdk.EventButton evt) => Scene?.HandleButtonPress(evt);
    protected override void HandleKeyPress(Gdk.EventKey evt) => Scene?.HandleKeyPress(evt);
    protected override void Step() => Scene?.Step();
}

