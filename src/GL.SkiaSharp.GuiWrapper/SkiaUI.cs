using Gtk;
using SkiaSharp;

namespace GL.SkiaSharp.GuiWrapper;

public abstract  class SkiaUI : SkiaUIWithTheme
{
    public IUIElement? Scene { get; set; }

    protected abstract IUIElement InitScene();

    protected override void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y)
    {
        if (Scene != null) Scene.DrawFrame(surface, canvas, x, y);
    }

    protected override void Execute(IUICommand cmd)
    {
        if (Scene != null) Scene.Execute(cmd);
    }

    protected override void HandleButtonPress(Gdk.EventButton evt)
    {
        if (Scene != null) Scene.HandleButtonPress(evt);
    }

    protected override void HandleKeyPress(Gdk.EventKey evt)
    {
        if (Scene != null) Scene.HandleKeyPress(evt);
    }

    protected override Window InitWindow()
    {
        var init = base.InitWindow();
        Scene = InitScene();
        return init;
    }
}



