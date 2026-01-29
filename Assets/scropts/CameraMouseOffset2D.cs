using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraMouseOffset2D : MonoBehaviour
{
    public Transform player;
    public Camera mainCam;
    public float maxOffset = 3f;
    public float smooth = 5f;

    private CinemachineCamera cmCam;
    private CinemachinePositionComposer composer;

    void Awake()
    {
        cmCam = GetComponent<CinemachineCamera>();
        composer = cmCam.GetComponent<CinemachinePositionComposer>();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector2 dir = mouseWorld - player.position;
        Vector2 offset = Vector2.ClampMagnitude(dir, maxOffset);

        composer.TargetOffset = Vector3.Lerp(
            composer.TargetOffset,
            offset,
            Time.deltaTime * smooth
        );
    }
}
