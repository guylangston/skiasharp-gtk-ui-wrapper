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
            canvas.DrawCircle(
                    rect.Width/3,
                    rect.Height/3,
                    rect.Width/3 - 50,
                    Theme.GetPaint("Orange"));
            canvas.DrawText( $"{Window?.Title} (frame: {App.Frame}, mouse: {x},{y}) {GetType().Name.ToUpper()}",
                    10f, 50f, SKTextAlign.Left,  Theme.DefaultFont, Theme.GetPaint("Black") );
        }
    }
    class SceneBounce : UIElementBase<DemoApp>
    {
        List<Ball> balls = new();

        public SceneBounce(DemoApp app) : base(app)
        {
        }

        public class Ball
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float dX { get; set; }
            public float dY { get; set; }
            public float Size { get; set; }
            public SKPaint Clr { get; set; } = new();

            public void Step(SKRect size)
            {
                X += dX;
                Y += dY;
                if (X < 0) { X = 1; dX *= -1; }
                if (Y < 0) { Y = 1; dY *= -1; }
                if (X >= size.Width) { X = size.Width-1; dX *= -1; }
                if (Y >= size.Height) { Y = size.Height-1; dY *= -1; }
            }
        }


        public override void DrawFrame(SKSurface surface, SKCanvas canvas, int x, int y)
        {
            var size = canvas.LocalClipBounds;
            if (balls.Count == 0)
            {
                var r = new Random();
                List<SKPaint> clrs = [Theme.GetPaint("Pink"), Theme.GetPaint("Yellow"), Theme.GetPaint("Cyan"), Theme.GetPaint("Green")];
                SKPaint SampleClr()  => clrs[r.Next(0, clrs.Count)];

                for(int cc=0; cc<100; cc++)
                {
                    balls.Add(new Ball()
                    {
                        X = r.Next(0, (int)size.Width),
                        Y = r.Next(0, (int)size.Height),
                        dX = r.Next(-5, 5),
                        dY = r.Next(-5, 5),
                        Size = r.Next(3, 10),
                        Clr = SampleClr()
                    });
                }
            }

            foreach(var ball in balls)
            {
                ball.Step(size);
                canvas.DrawCircle(ball.X, ball.Y, ball.Size, ball.Clr);
            }
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

