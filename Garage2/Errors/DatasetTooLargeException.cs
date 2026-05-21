namespace Garage2.Errors;

public class DatasetTooLargeException(int actualSize, int maxSize)
    : ArgumentException($"Dataset too large: {actualSize}. Max size: {maxSize}")
{
    private readonly int _actualSize = actualSize;
    private readonly int _maxSize = maxSize;
}
