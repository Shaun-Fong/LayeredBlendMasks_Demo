using UnityEngine;

public class DualCameraController : MonoBehaviour
{
    public Transform leftCameraParent;
    public Transform rightCameraParent;
    public float rotationSpeed = 100.0f;
    public bool syncCameras = false;
    public bool SyncCameras
    {
        get
        {
            return syncCameras;
        }
        set
        {
            syncCameras = value;
        }
    }

    private bool isDragging = false;
    private bool isLeftDragging = false;
    private bool isRightDragging = false;
    private Vector2 lastMousePosition;
    private bool wasSyncCamerasEnabled = false; // 用于记录上一次同步状态

    void Update()
    {
        // 检测同步状态是否刚刚开启
        if (syncCameras && !wasSyncCamerasEnabled)
        {
            if (leftCameraParent != null && rightCameraParent != null)
            {
                rightCameraParent.rotation = leftCameraParent.rotation;
            }
        }
        wasSyncCamerasEnabled = syncCameras; // 更新上一次的同步状态

        // 检测鼠标左键是否按下
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            if (syncCameras)
            {
                isDragging = true;
                isLeftDragging = false;
                isRightDragging = false;
            }
            else
            {
                // 判断鼠标是否在屏幕的左半边
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    isLeftDragging = true;
                    isDragging = false;
                    isRightDragging = false;
                }
                // 判断鼠标是否在屏幕的右半边
                else
                {
                    isRightDragging = true;
                    isDragging = false;
                    isLeftDragging = false;
                }
            }
        }

        // 检测鼠标左键是否抬起
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            isLeftDragging = false;
            isRightDragging = false;
        }

        // 如果开启了同步，则全屏拖拽控制两个摄像头
        if (isDragging && syncCameras)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaX = currentMousePosition.x - lastMousePosition.x;
            float deltaY = currentMousePosition.y - lastMousePosition.y;

            // 旋转左边摄像头父物体
            if (leftCameraParent != null)
            {
                leftCameraParent.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime, Space.World);
                leftCameraParent.Rotate(Vector3.right, -deltaY * rotationSpeed * Time.deltaTime, Space.Self);
            }

            // 旋转右边摄像头父物体 (同步左边的旋转)
            if (rightCameraParent != null)
            {
                rightCameraParent.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime, Space.World);
                rightCameraParent.Rotate(Vector3.right, -deltaY * rotationSpeed * Time.deltaTime, Space.Self);
            }

            lastMousePosition = currentMousePosition;
        }
        // 如果没有开启同步，则根据左右半屏分别控制
        else
        {
            // 如果左边正在拖拽，则旋转左摄像头父物体
            if (isLeftDragging)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                float deltaX = currentMousePosition.x - lastMousePosition.x;
                float deltaY = currentMousePosition.y - lastMousePosition.y;

                if (leftCameraParent != null)
                {
                    leftCameraParent.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime, Space.World);
                    leftCameraParent.Rotate(Vector3.right, -deltaY * rotationSpeed * Time.deltaTime, Space.Self);
                }

                lastMousePosition = currentMousePosition;
            }
            // 如果右边正在拖拽，则旋转右摄像头父物体
            else if (isRightDragging)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                float deltaX = currentMousePosition.x - lastMousePosition.x;
                float deltaY = currentMousePosition.y - lastMousePosition.y;

                if (rightCameraParent != null)
                {
                    rightCameraParent.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime, Space.World);
                    rightCameraParent.Rotate(Vector3.right, -deltaY * rotationSpeed * Time.deltaTime, Space.Self);
                }

                lastMousePosition = currentMousePosition;
            }
        }
    }
}