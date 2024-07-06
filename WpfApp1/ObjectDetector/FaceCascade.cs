using OpenCvSharp;

namespace WpfApp1.ObjectDetector;

public class FaceCascade
{
    private const String FrontFaceModel = @"..\..\..\data\haarcascades\haarcascade_frontalface_alt2.xml";
    public CascadeClassifier _cascade { get; }
    
    public FaceCascade() => _cascade = new CascadeClassifier();

    public String LoadPath() => FrontFaceModel;
    
    public Rect[] Detect(Mat frame) => _cascade.DetectMultiScale(frame, 1.1, 3);
}