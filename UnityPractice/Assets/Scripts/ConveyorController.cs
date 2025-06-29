using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 컨베이어 벨트의 동작을 제어하는 스크립트
/// </summary>
public class ConveyorController : MonoBehaviour
{

    [Header("컨베이어 설정")]
    public List<Transform> products = new List<Transform>(); // 이동시킬 제품
    public float speed = 1.0f; // 기본 속도
    public Transform startPosition; // 시작 위치
    public Transform endPosition;   // 끝 위치 (확장용)

    [Header("움직임 패턴")]
    public MovementPattern movementPattern = MovementPattern.Linear;
    public float amplitude = 1.0f;
    public float frequency = 1.0f;

    [Header("UI/상태")]
    public bool isRunning = false;

    [Header("디버깅")]
    public bool showTrajectory = false;
    public Color trajectoryColor = Color.red;
    private List<Vector3> trajectoryPoints = new List<Vector3>(); // 궤적 포인트들

    // 제품 원래 색상 저장용
    private Color productOriginalColor;

    // 움직임 패턴 열거형
    public enum MovementPattern { Linear, Sine, Circular, Zigzag, Spiral }

    private void Start()
    {
        foreach (Transform product in products)
        {


            if (product != null)
            {
                Renderer renderer = product.GetComponent<Renderer>();
                if (renderer != null)
                    productOriginalColor = renderer.material.color; // 시작할 때 색상 저장
            }
        }
    }

    private void Update()
    {
        if (isRunning)
        {

            foreach (Transform product in products)
            {
                product.Translate(Vector3.right * speed * Time.deltaTime);
                // 컨베이어가 동작 중이고 제품이 있을 때만 이동
                if (product.position.x >= endPosition.position.x)
                {
                    Vector3 resetPos = startPosition.position;
                    resetPos.y = product.position.y;
                    resetPos.z = product.position.z;
                    product.position = resetPos;

                    Renderer renderer = product.GetComponent<Renderer>();
                    if (renderer != null)
                        renderer.material.color = productOriginalColor;
                    //// 선택된 패턴에 따라 움직임 적용
                    //switch (movementPattern)
                    //{
                    //    case MovementPattern.Linear: MoveLinear(); break;
                    //    case MovementPattern.Sine: MoveSine(); break;
                    //    case MovementPattern.Circular: MoveCircular(); break;
                    //    case MovementPattern.Zigzag: MoveZigzag(); break;
                    //    case MovementPattern.Spiral: MoveSpiral(); break;
                    //}
                }
            }
        }
            RecordTrajectory();
    }

    // 직선 이동
    private void MoveLinear()
    {
        foreach (Transform product in products)
        {
            if (isRunning && product != null)
            {
                product.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }

    // 사인파 움직임
    private void MoveSine()
    {
        // X축으로 전진
        foreach (Transform product in products)
        {
            if (isRunning && product != null)
            {
                product.Translate(Vector3.right * speed * Time.deltaTime);
            }

            // Y축으로 사인파 움직임
            float sineValue = Mathf.Sin(Time.time * frequency) * amplitude;
            Vector3 currentPos = product.position;
            currentPos.y = startPosition.position.y + sineValue;
            product.position = currentPos;
        }

        
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
        foreach (Transform product in products)
        {
            Vector3 newPosition = new Vector3(x, product.position.y, z);
            product.position = newPosition;
        }
    }

    // 지그재그 움직임
    private void MoveZigzag()
    {
        foreach (Transform product in products)
        {
            // X축으로 전진
            product.Translate(Vector3.right * speed * Time.deltaTime);

            // Z축으로 지그재그 움직임
            float zigzagValue = Mathf.Sin(Time.time * frequency * 2f) * amplitude;
            Vector3 currentPos = product.position;
            currentPos.z = startPosition.position.z + zigzagValue;
            product.position = currentPos;
        }
    }

    private void MoveSpiral()
    {
        foreach (Transform product in products)
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
        foreach (Transform product in products)
        {
            if (product != null && startPosition != null)
            {
                // Empty GameObject의 위치로 제품 이동
                product.position = startPosition.position;

                // 색상 복구
                Renderer renderer = product.GetComponent<Renderer>();
                if (renderer != null)
                    renderer.material.color = productOriginalColor; // 원래 색상으로 복구

                Debug.Log("제품 위치 및 색상 초기화!");
            }
            else
            {
                Debug.LogWarning("Product 또는 Start Position이 설정되지 않았습니다!");
            }
        }
    }

    // 궤적 기록
    private void RecordTrajectory()
    {
        foreach (Transform product in products)
        {
            if (showTrajectory)
            {
                trajectoryPoints.Add(product.position);

                // 궤적 포인트 수 제한 (메모리 절약)
                if (trajectoryPoints.Count > 1000)
                {
                    trajectoryPoints.RemoveAt(0);
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (!showTrajectory || trajectoryPoints.Count < 2) return;
        Gizmos.color = trajectoryColor;
        for (int i = 1; i < trajectoryPoints.Count; i++)
            Gizmos.DrawLine(trajectoryPoints[i - 1], trajectoryPoints[i]);
    }

}