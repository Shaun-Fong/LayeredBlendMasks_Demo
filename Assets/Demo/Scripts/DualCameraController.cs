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
    private bool wasSyncCamerasEnabled = false; // ���ڼ�¼��һ��ͬ��״̬

    void Update()
    {
        // ���ͬ��״̬�Ƿ�ոտ���
        if (syncCameras && !wasSyncCamerasEnabled)
        {
            if (leftCameraParent != null && rightCameraParent != null)
            {
                rightCameraParent.rotation = leftCameraParent.rotation;
            }
        }
        wasSyncCamerasEnabled = syncCameras; // ������һ�ε�ͬ��״̬

        // ����������Ƿ���
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
                // �ж�����Ƿ�����Ļ������
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    isLeftDragging = true;
                    isDragging = false;
                    isRightDragging = false;
                }
                // �ж�����Ƿ�����Ļ���Ұ��
                else
                {
                    isRightDragging = true;
                    isDragging = false;
                    isLeftDragging = false;
                }
            }
        }

        // ����������Ƿ�̧��
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            isLeftDragging = false;
            isRightDragging = false;
        }

        // ���������ͬ������ȫ����ק������������ͷ
        if (isDragging && syncCameras)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaX = currentMousePosition.x - lastMousePosition.x;
            float deltaY = currentMousePosition.y - lastMousePosition.y;

            // ��ת�������ͷ������
            if (leftCameraParent != null)
            {
                leftCameraParent.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime, Space.World);
                leftCameraParent.Rotate(Vector3.right, -deltaY * rotationSpeed * Time.deltaTime, Space.Self);
            }

            // ��ת�ұ�����ͷ������ (ͬ����ߵ���ת)
            if (rightCameraParent != null)
            {
                rightCameraParent.Rotate(Vector3.up, deltaX * rotationSpeed * Time.deltaTime, Space.World);
                rightCameraParent.Rotate(Vector3.right, -deltaY * rotationSpeed * Time.deltaTime, Space.Self);
            }

            lastMousePosition = currentMousePosition;
        }
        // ���û�п���ͬ������������Ұ����ֱ����
        else
        {
            // ������������ק������ת������ͷ������
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
            // ����ұ�������ק������ת������ͷ������
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