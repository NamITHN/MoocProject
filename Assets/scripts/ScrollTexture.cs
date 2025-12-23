using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    [Tooltip("Tốc độ di chuyển của texture. Giá trị âm để di chuyển ngược lại.")]
    public float scrollSpeed = -0.5f;

    private Renderer roadRenderer;
    private float offset;

    void Start()
    {
        // Lấy component Renderer của đối tượng này
        roadRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Tính toán giá trị offset mới dựa trên thời gian và tốc độ
        // Time.time là tổng thời gian từ lúc game bắt đầu
        offset = Time.time * scrollSpeed;

        // Lấy offset hiện tại và chỉ thay đổi trục Y (hoặc X, tùy thuộc vào cách bạn UV map texture)
        // Dấu % 1.0f để giữ giá trị offset luôn trong khoảng 0-1, tránh số quá lớn không cần thiết
        float newOffsetY = offset % 1.0f;

        // Áp dụng offset mới cho texture chính của material
        // Sử dụng .material sẽ tạo một bản sao của material, không ảnh hưởng đến các object khác dùng chung material
        roadRenderer.material.mainTextureOffset = new Vector2(0, newOffsetY);
    }
}