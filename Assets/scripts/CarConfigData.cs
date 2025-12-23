// CarConfigData.cs
using System.Collections.Generic;

// Enum này giúp chúng ta chọn loại xe trong Inspector một cách an toàn
public enum CarType { TopCar, RightCar, LeftCar }

// Lớp này định nghĩa cấu trúc của MỘT profile xe trong file JSON
[System.Serializable]
public class CarProfileData
{
    // Quan trọng: Tên biến phải khớp chính xác với tên khóa trong JSON
    public string carType; 
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    public float outlineEnableMinX;
    public float outlineEnableMaxX;
}

// Lớp này đại diện cho toàn bộ file JSON
[System.Serializable]
public class CarConfigData
{
    public List<CarProfileData> carProfiles;
}