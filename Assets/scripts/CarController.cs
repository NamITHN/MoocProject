using System.Collections;
using System.Linq; // Cần thiết để sử dụng hàm .Find()
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class CarController : MonoBehaviour
{
    [Header("Car Identity")]
    [Tooltip("Chọn loại xe cho đối tượng này. Tên này phải khớp với 'carType' trong file JSON.")]
    public CarType thisCarType;

    [Header("Movement Behavior")]
    [Tooltip("Thời gian chờ giữa mỗi lần di chuyển")]
    public float moveInterval = 3.0f;
    [Tooltip("Thời gian để hoàn thành một lần di chuyển")]
    public float moveDuration = 1.5f;

    [Header("Components")]
    [Tooltip("Tự động tìm hoặc bạn có thể kéo thả component Outline vào đây")]
    public Outline outlineComponent;

    // Biến để lưu trữ profile cụ thể cho chiếc xe này sau khi đọc từ JSON
    private CarProfileData myProfile;

    private float initialY;
    private float timer;
    private bool isMoving = false;

    void Start()
    {
        // 1. Tải cấu hình và tìm profile phù hợp cho chiếc xe này
        LoadAndAssignProfile();

        // Nếu không tìm thấy profile, script sẽ bị vô hiệu hóa, dừng thực thi tại đây
        if (myProfile == null)
        {
            this.enabled = false;
            return;
        }

        // 2. Các bước khởi tạo còn lại nếu tìm thấy profile thành công
        initialY = transform.position.y;
        if (outlineComponent == null) { outlineComponent = GetComponent<Outline>(); }
        
        // Bắt đầu chu trình di chuyển
        StartNewRandomMove();
    }

    void Update()
    {
        if (!isMoving)
        {
            timer += Time.deltaTime;
            if (timer >= moveInterval)
            {
                StartNewRandomMove();
            }
        }
    }
    
    /// <summary>
    /// Tải file JSON, phân tích và tìm ra profile phù hợp với 'thisCarType'.
    /// </summary>
    private void LoadAndAssignProfile()
    {
        TextAsset configFile = Resources.Load<TextAsset>("carConfig");

        if (configFile == null)
        {
            Debug.LogError("LỖI: Không tìm thấy file 'carConfig.json' trong thư mục 'Assets/Resources'!");
            return;
        }

        CarConfigData allConfigs = JsonUtility.FromJson<CarConfigData>(configFile.text);

        // Sử dụng Linq để tìm profile có carType (string) khớp với tên của enum (thisCarType)
        myProfile = allConfigs.carProfiles.Find(p => p.carType == thisCarType.ToString());

        if (myProfile == null)
        {
            Debug.LogError("LỖI: Không tìm thấy profile cho loại xe '" + thisCarType + "' trong file carConfig.json!", gameObject);
        }
        else
        {
            Debug.Log("'" + gameObject.name + "' đã tải thành công profile cho: " + myProfile.carType);
        }
    }

    private void StartNewRandomMove()
    {
        timer = 0f;
        
        // Sử dụng các giá trị từ myProfile đã tìm được
        float randomX = Random.Range(myProfile.minX, myProfile.maxX);
        float randomZ = Random.Range(myProfile.minZ, myProfile.maxZ);

        Vector3 targetPosition = new Vector3(randomX, initialY, randomZ);
        StartCoroutine(MoveSmoothlyCoroutine(targetPosition));
    }

    private void UpdateOutlineState()
    {
        float currentX = transform.position.x;
        
        // Sử dụng các giá trị từ myProfile đã tìm được
        if (currentX >= myProfile.outlineEnableMinX && currentX <= myProfile.outlineEnableMaxX)
        {
            outlineComponent.enabled = true;
        }
        else
        {
            outlineComponent.enabled = false;
        }
    }

    private IEnumerator MoveSmoothlyCoroutine(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            t = t * t * (3f - 2f * t); // SmoothStep
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        UpdateOutlineState();
        isMoving = false;
    }
}