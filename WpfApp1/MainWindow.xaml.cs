using System.Windows;
using OpenCvSharp;
using WpfApp1.CameraSetting;
using WpfApp1.Models;
using WpfApp1.ObjectDetector;
using Point = OpenCvSharp.Point;

namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : System.Windows.Window
{
    private readonly Mat _frame = new();
    private readonly CameraTimer _cameraTimer = new();
    private readonly ButtonModels _buttonModels = new();
    
    public MainWindow() => InitializeComponent();

    private void windows_loaded(object sender, RoutedEventArgs e) => CameraInit();
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => CameraRelease();
    
    
    private void ChangeFlipHorizontal(object sender, RoutedEventArgs e) => ButtonClicker(ButtonModels.ButtonType.FlipHorizontal);
    private void ChangeFlipVertical(object sender, RoutedEventArgs e) => ButtonClicker(ButtonModels.ButtonType.FlipVertical);
    private void ChangeGrayScale(object sender, RoutedEventArgs e) => ButtonClicker(ButtonModels.ButtonType.GrayScale);
    private void ChangeCanny(object sender, RoutedEventArgs e) => ButtonClicker(ButtonModels.ButtonType.Canny);

    // 카메라 초기화
    private void CameraInit()
    {
        _cameraTimer.Init(0, (int)Cam1.Height, (int)Cam1.Width, 0.01, TimerTick);
        if (!_cameraTimer.Camera.IsInitCam || !_cameraTimer.IsInitTimer) return;
        _cameraTimer.Timer.Start();
    }
    
    // 카메라 해제
    private void CameraRelease()
    {
        // 종료시 스트림 해제
        _cameraTimer.Timer.Stop();
        _cameraTimer.Camera.Capture.Release();
    }
    
    // 타이머 이벤트
    private void TimerTick(object sender, EventArgs e)
    {
        _cameraTimer.Camera.Capture.Read(_frame);
        if (_frame.Empty()) return;
        var result = _frame.Clone();
        
        // 버튼 상태에 따라 결과값 변경
        if (_buttonModels.ButtonStatus[ButtonModels.ButtonType.FlipHorizontal])
            Cv2.Flip(result, result, FlipMode.Y);
        if (_buttonModels.ButtonStatus[ButtonModels.ButtonType.FlipVertical])
            Cv2.Flip(result, result, FlipMode.X);
        if (_buttonModels.ButtonStatus[ButtonModels.ButtonType.GrayScale])
            Cv2.CvtColor(result, result, ColorConversionCodes.BGR2GRAY);
        if (_buttonModels.ButtonStatus[ButtonModels.ButtonType.Canny])
        {
            var temp = result.Clone();
            Cv2.Canny(temp, result, 50, 150);
        }
        
        // 얼굴 인식
     
        var faceCascade = new FaceCascade();
        if(faceCascade._cascade.Load(faceCascade.LoadPath()))
        {
            var faces = faceCascade.Detect(result);
            foreach (var face in faces)
            {
                Cv2.Rectangle(result, face, Scalar.Red, 2);
            }
        }
        
        // set FPS
        _cameraTimer.GetFps();
        // 화면에 FPS 그리기
        Cv2.PutText(result, $"FPS: {_cameraTimer.GetFps():F2}", new Point(10, 20), HersheyFonts.HersheySimplex, 0.5, Scalar.White);
      
        // 읽어온 Mat 데이터를 Bitmap 데이터로 변경 후 컨트롤에 그려줌
        Cam1.Source = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(result);
    }
    
    // 버튼 클릭 이벤트
    private void ButtonClicker(ButtonModels.ButtonType buttonType) =>
        _buttonModels.ButtonStatus[buttonType] = !_buttonModels.ButtonStatus[buttonType];
    
}
