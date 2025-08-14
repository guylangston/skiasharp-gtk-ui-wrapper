using Gtk;
using SkiaSharp;

namespace GL.SkiaSharp.GuiWrapper;

public abstract class SkiaUI
{
    public int FPS { get; set; } = 60;
    public int Frame { get; set; }
    protected Window? Window { get; private set; }
    public int ExitCode { get; set; } = 0;
    public string Title { get; set; } = nameof(SkiaUI);
    public (int x, int y) RequestedWindowSize { get; set; } = (400, 300);

    protected virtual Window InitWindow()
    {
        var window = new Window(Title);
        window.SetDefaultSize(RequestedWindowSize.x, RequestedWindowSize.y);
        window.KeyPressEvent    += (o, args) => this.HandleKeyPress(args.Event);
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
}

