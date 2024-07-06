
// Timer 사용을 위한 using
using System.Windows.Threading;

namespace WpfApp1.CameraSetting;

public class CameraTimer
{
    public DispatcherTimer Timer { get; private set; }
    public bool IsInitTimer { get; private set; }
    
    public Camera Camera { get; private set; }
    
    public void Init(int cameraIndex, int height, int width, double intervalMs, EventHandler timerTick)
    {
        Camera = new();
        Camera.Init(cameraIndex, height, width);
        IsInitTimer = InitTimer(intervalMs, timerTick);
    }

    private bool InitTimer(double intervalMs, EventHandler timerTick)
    {
        try
        {
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(intervalMs)
            };
            Timer.Tick += timerTick;
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    //FPS 계산
    public double GetFps() => Timer.Interval.TotalMilliseconds * 0.001d;
}