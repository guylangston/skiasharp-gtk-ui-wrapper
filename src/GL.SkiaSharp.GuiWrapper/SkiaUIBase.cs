using Gtk;
using SkiaSharp;

namespace GL.SkiaSharp.GuiWrapper;

public interface IUICommand {}
public interface IUIElement
{
    void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y);
    void HandleKeyPress(Gdk.EventKey evt);
    void HandleButtonPress(Gdk.EventButton evt);
    void Execute(IUICommand cmd);
    void Step();
}
public abstract class UIElementBase<T> : IUIElement where T:SkiaUIWithTheme
{
    protected UIElementBase(T app)
    {
        App = app;
    }

    protected T App { get; }
    protected ITheme Theme => App.Theme;
    protected Gtk.Window? Window => App.Window;
    protected int Frame => App.Frame;

    public abstract void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y);
    public virtual void HandleKeyPress(Gdk.EventKey evt) {}
    public virtual void HandleButtonPress(Gdk.EventButton evt) {}
    public virtual void Execute(IUICommand cmd) {}
    public virtual void Step() {}
}


public abstract class SkiaUIBase : IUIElement
{
    public int FPS { get; set; } = 60;
    public int Frame { get; set; }
    public Window? Window { get; private set; }
    public int ExitCode { get; set; } = 0;
    public string Title { get; set; } = nameof(SkiaUI);
    public (int x, int y) RequestedWindowSize { get; set; } = (400, 300);

    [GLib.ConnectBefore]
    void KeyPress(object sender, KeyPressEventArgs args) => this.HandleKeyPress(args.Event);

    protected virtual Window InitWindow()
    {
        var window = new Window(Title);
        window.SetDefaultSize(RequestedWindowSize.x, RequestedWindowSize.y);
        window.KeyPressEvent    += KeyPress;
        window.ButtonPressEvent += (o, args) => this.HandleButtonPress(args.Event);
        window.DeleteEvent      += (o, args) => this.OnMainWindowClose();
        return window;
    }

    protected virtual void OnMainWindowClose()
    {
        Application.Quit();
    }

    public int Run()
    {
        Application.Init();
        Window = InitWindow();

        var interval = (uint)(1000 / FPS); // ~16ms

        var drawingArea = new DrawingArea();
        GLib.Timeout.Add(interval, () => {
            Step();
            drawingArea.QueueDraw();
            return true; // Continue calling
        });

        // List system fonts using the `fc-list : family` command will print all available font family names.
        drawingArea.Drawn += (o, args) =>
        {
            var cr = args.Cr;
            using var surface = SKSurface.Create(new SKImageInfo(Window.AllocatedWidth, Window.AllocatedHeight));
            var canvas = surface.Canvas;

            // Get the current mouse location
            Gdk.Display.Default.GetPointer(out var x, out var y);

            DrawFrame(surface, canvas, x, y);

            using var img = surface.Snapshot();
            using var data = img.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = new MemoryStream(data.ToArray());
            var pixbuf = new Gdk.Pixbuf(stream);
            Gdk.CairoHelper.SetSourcePixbuf(cr, pixbuf, 0, 0);
            cr.Paint();
            Frame++;
        };

        Window.Add(drawingArea);
        Window.ShowAll();
        Application.Run();
        return ExitCode;
    }

    protected abstract void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y);
    protected abstract void HandleKeyPress(Gdk.EventKey evt);
    protected abstract void HandleButtonPress(Gdk.EventButton evt);
    protected abstract void Execute(IUICommand cmd);
    protected abstract void Step();

    void IUIElement.DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y) { DrawFrame(surface, canvas, x, y); }
    void IUIElement.HandleKeyPress(Gdk.EventKey evt) { HandleKeyPress(evt); }
    void IUIElement.HandleButtonPress(Gdk.EventButton evt) { HandleButtonPress(evt); }
    void IUIElement.Execute(IUICommand cmd) { Execute(cmd); }
    void IUIElement.Step() { Step(); }
}

