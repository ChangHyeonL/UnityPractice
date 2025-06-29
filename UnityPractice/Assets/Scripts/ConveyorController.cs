using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 컨베이어 벨트의 동작을 제어하는 스크립트
/// 제품을 일정 속도로 이동시키고 시작/정지 기능을 제공
/// </summary>
public class ConveyorController : MonoBehaviour
{
    [Header("컨베이어 설정")]
    public Transform product; // 이동시킬 제품
    public float speed = 1.0f; // 기본 속도
    public Transform startPosition; // 시작 위치 (Empty GameObject)
    public Transform endPosition;   // 끝 위치 (Empty GameObject) - 향후 확장용

    [Header("움직임 패턴")]
    public MovementPattern movementPattern = MovementPattern.Linear; // 움직임 패턴
    public float amplitude = 1.0f; // 진폭 (사인파용)
    public float frequency = 1.0f; // 주파수 (사인파용)
    
    [Header("테스트용")]
    public bool isRunning = false; // Inspector에서 수동으로 제어

    [Header("물리 설정")]
    public bool usePhysics = false; // 물리 시스템 사용 여부
    public float friction = 0.1f; // 마찰력
    public float gravity = 9.81f; // 중력
    
    [Header("충돌 감지")]
    public bool enableCollisionDetection = false; // 충돌 감지 활성화
    public LayerMask collisionLayers = -1; // 충돌 감지할 레이어

    [Header("디버깅")]
    public bool showDebugInfo = false; // 디버그 정보 표시
    public bool showTrajectory = false; // 궤적 표시
    public Color trajectoryColor = Color.red; // 궤적 색상
    
    private List<Vector3> trajectoryPoints = new List<Vector3>(); // 궤적 포인트들

    // 움직임 패턴 열거형
    public enum MovementPattern
    {
        Linear,     // 직선 이동
        Sine,       // 사인파 움직임
        Circular,   // 원형 움직임
        Zigzag,      // 지그재그 움직임
        Spiral      // 스파이럴 움직임
    }

    private void Start()
    {
        Debug.Log("SimpleConveyorController 시작!");
        Debug.Log("Inspector에서 isRunning을 체크하면 제품이 움직입니다.");
    }

    private void Update()
    {
        // 컨베이어가 동작 중이고 제품이 있을 때만 이동
        if(isRunning && product != null)
        {
            // 물리 시스템 사용 여부에 따라 분기
            if (usePhysics)
            {
                MoveWithPhysics();
            }
            else
            {
                // 선택된 패턴에 따라 움직임 적용
                switch (movementPattern)
                {
                    case MovementPattern.Linear:
                        MoveLinear();
                        break;
                    case MovementPattern.Sine:
                        MoveSine();
                        break;
                    case MovementPattern.Circular:
                        MoveCircular();
                        break;
                    case MovementPattern.Zigzag:
                        MoveZigzag();
                        break;
                    case MovementPattern.Spiral:
                        MoveSpiral();
                        break;
                }
            }
            
            // 디버깅 기능
            RecordTrajectory();
            ShowDebugInfo();
        }
    }

    // 직선 이동
    private void MoveLinear()
    {
        product.Translate(Vector3.right * speed * Time.deltaTime);
    }

    // 사인파 움직임
    private void MoveSine()
    {
        // X축으로 전진
        product.Translate(Vector3.right * speed * Time.deltaTime);
        
        // Y축으로 사인파 움직임
        float sineValue = Mathf.Sin(Time.time * frequency) * amplitude;
        Vector3 currentPos = product.position;
        currentPos.y = startPosition.position.y + sineValue;
        product.position = currentPos;
    }

    // 원형 움직임
    private void MoveCircular()
    {
        // 중심점 (컨베이어 중앙)
        Vector3 center = transform.position;
        
        // 시간에 따른 각도 계산
        float angle = Time.time * speed * 50f; // 속도를 각도로 변환
        
        // 원형 궤도 계산
        float radius = amplitude;
        float x = center.x + Mathf.Cos(angle) * radius;
        float z = center.z + Mathf.Sin(angle) * radius;
        
        // 제품 위치 설정
        Vector3 newPosition = new Vector3(x, product.position.y, z);
        product.position = newPosition;
    }

    // 지그재그 움직임
    private void MoveZigzag()
    {
        // X축으로 전진
        product.Translate(Vector3.right * speed * Time.deltaTime);
        
        // Z축으로 지그재그 움직임
        float zigzagValue = Mathf.Sin(Time.time * frequency * 2f) * amplitude;
        Vector3 currentPos = product.position;
        currentPos.z = startPosition.position.z + zigzagValue;
        product.position = currentPos;
    }

    private void MoveSpiral()
    {
        float angle = Time.time * frequency;
        Vector3 currentPos = product.position;

        // X축 전진
        currentPos.x += speed * Time.deltaTime;

        // y축 사인파
        currentPos.y = startPosition.position.y + Mathf.Sin(angle) * amplitude;

        // z축 코사인파
        currentPos.z = startPosition.position.z + Mathf.Cos(angle) * amplitude;

        // 제품 위치 업데이트
        product.position = currentPos;
    }

    // 물리 기반 움직임
    private void MoveWithPhysics()
    {
        if (product == null) return;
        
        Rigidbody rb = product.GetComponent<Rigidbody>();
        if (rb == null) return;
        
        // 컨베이어 힘 적용
        Vector3 conveyorForce = Vector3.right * speed;
        
        // 마찰력 적용
        Vector3 frictionForce = -rb.linearVelocity * friction;
        
        // 중력 적용
        Vector3 gravityForce = Vector3.down * gravity;
        
        // 총 힘 계산
        Vector3 totalForce = conveyorForce + frictionForce + gravityForce;
        
        // 힘 적용
        rb.AddForce(totalForce, ForceMode.Force);
    }

    // 충돌 감지
    private void OnTriggerEnter(Collider other)
    {
        if (!enableCollisionDetection) return;
        
        // 레이어 체크
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            Debug.Log($"충돌 감지: {other.name}");
            
            // 충돌 시 정지
            if (isRunning)
            {
                StopConveyor();
                Debug.Log("충돌로 인해 컨베이어가 정지되었습니다.");
            }
        }
    }

    // Inspector에서 테스트할 수 있는 함수들
    [ContextMenu("Start Conveyor")]
    public void StartConveyor()
    {
        isRunning = true;
        Debug.Log("컨베이어 시작!");
    }

    [ContextMenu("Stop Conveyor")]
    public void StopConveyor()
    {
        isRunning = false;
        Debug.Log("컨베이어 정지!");
    }

    [ContextMenu("Reset Conveyor")]
    public void ResetConveyor()
    {
        if (product != null && startPosition != null)
        {
            // Empty GameObject의 위치로 제품 이동
            product.position = startPosition.position;
            Debug.Log("제품 위치 초기화!");
        }
        else
        {
            Debug.LogWarning("Product 또는 Start Position이 설정되지 않았습니다!");
        }
    }

    // 궤적 기록
    private void RecordTrajectory()
    {
        if (product != null && showTrajectory)
        {
            trajectoryPoints.Add(product.position);
            
            // 궤적 포인트 수 제한 (메모리 절약)
            if (trajectoryPoints.Count > 1000)
            {
                trajectoryPoints.RemoveAt(0);
            }
        }
    }

    // 디버그 정보 표시
    private void ShowDebugInfo()
    {
        if (!showDebugInfo || product == null) return;
        
        Vector3 velocity = Vector3.zero;
        if (product.GetComponent<Rigidbody>() != null)
        {
            velocity = product.GetComponent<Rigidbody>().linearVelocity;
        }
        
        Debug.Log($"제품 위치: {product.position}");
        Debug.Log($"제품 속도: {velocity}");
        Debug.Log($"현재 패턴: {movementPattern}");
        Debug.Log($"컨베이어 속도: {speed}");
    }

    // Scene 뷰에서 궤적 그리기
    private void OnDrawGizmos()
    {
        if (!showTrajectory || trajectoryPoints.Count < 2) return;
        
        Gizmos.color = trajectoryColor;
        
        for (int i = 1; i < trajectoryPoints.Count; i++)
        {
            Gizmos.DrawLine(trajectoryPoints[i - 1], trajectoryPoints[i]);
        }
    }

    // 궤적 초기화
    [ContextMenu("Clear Trajectory")]
    public void ClearTrajectory()
    {
        trajectoryPoints.Clear();
        Debug.Log("궤적이 초기화되었습니다.");
    }
}