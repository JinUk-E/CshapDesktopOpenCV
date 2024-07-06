// OpenCV 사용을 위한 using
using OpenCvSharp;

namespace WpfApp1.CameraSetting;

public class Camera
{
   public VideoCapture Capture { get; private set; }
   public bool IsInitCam { get; private set; }
   
   public void Init(int cameraIndex, int height, int width)
   {
      IsInitCam = InitCamera(cameraIndex, height, width);
   }
   
   private bool InitCamera(int cameraIndex, int height, int width)
   {
      try
      {
         Capture = new VideoCapture(cameraIndex);
         Capture.FrameWidth = width;
         Capture.FrameHeight = height;
         return true;
      }
      catch
      {
         return false;
      }
   }
   
}