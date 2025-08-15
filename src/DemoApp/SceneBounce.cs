using SkiaSharp;
using GL.SkiaSharp.GuiWrapper;

namespace DemoApp;

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


