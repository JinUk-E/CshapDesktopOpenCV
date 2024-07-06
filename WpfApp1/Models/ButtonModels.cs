namespace WpfApp1.Models;

public class ButtonModels
{
    public enum ButtonType 
    {
        FlipHorizontal,
        FlipVertical,
        GrayScale,
        Canny,
    }
    public Dictionary<ButtonType, bool> ButtonStatus { get; private set; } = new()
    {
        {ButtonType.FlipHorizontal, false},
        {ButtonType.FlipVertical , false},
        {ButtonType.GrayScale, false},
        {ButtonType.Canny, false}
    };
}