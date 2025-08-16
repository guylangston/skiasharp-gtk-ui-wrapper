using SkiaSharp;

namespace GL.SkiaSharp.GuiWrapper;

public interface IUIElement
{
    void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y);
    void HandleKeyPress(Gdk.EventKey evt);
    void HandleButtonPress(Gdk.EventButton evt);
    void Execute(IUICommand cmd);
    void Step();
}

public interface IUICommand {}

public abstract class UIElementBase<TApp, TTheme> : IUIElement where TApp:SkiaUIWithTheme<TTheme> where TTheme:ITheme
{
    protected UIElementBase(TApp app) { App = app; }

    protected TApp App { get; }
    protected TTheme Theme => App.Theme;
    protected Gtk.Window? Window => App.Window;
    protected int Frame => App.Frame;

    public abstract void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y);
    public virtual void HandleKeyPress(Gdk.EventKey evt) {}
    public virtual void HandleButtonPress(Gdk.EventButton evt) {}
    public virtual void Execute(IUICommand cmd) {}
    public virtual void Step() {}
}
