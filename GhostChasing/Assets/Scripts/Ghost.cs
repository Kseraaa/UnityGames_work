using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject chaseTarget;

    public bool isOnPatrol;
    public bool isOnChase;

    [SerializeField] private float chaseSpeed = 1f;
    [SerializeField] private float chaseRotateSpeed = 1f;
    [SerializeField] private float stopOffset = 1f;

    [SerializeField] private float fieldOfViewAngle = 90f; // มุมมอง FOV ของ Ghost
    [SerializeField] private float viewDistance = 10f; // ระยะทางมองเห็นของ Ghost

    private bool _isChaseTargetActive;
    
    // Start is called before the first frame update
    void Start()
    {
        CheckActive();
        isOnPatrol = true;
        isOnChase = false;
    }
    private void CheckActive()
    {
        _isChaseTargetActive = chaseTarget != null;
        
        Debug.Log(_isChaseTargetActive? $"CHASE TARGET IS {chaseTarget.name}":"NO CHASE TARGET");
    }

    void LateUpdate()
    {
        if (!_isChaseTargetActive)
        {
            return;
        }
        
        // ตรวจสอบ FOV และเปลี่ยนโหมดตาม
        if (InFieldOfView(chaseTarget.transform.position))
        {
            isOnChase = true;
            isOnPatrol = false;
        }
        else
        {
            isOnChase = false;
            isOnPatrol = true;
        }

        // เรียกใช้เมท็อดตามโหมดปัจจุบัน
        if (isOnChase)
        {
            GhostChase();
        }
        else if (isOnPatrol)
        {
            GhostPatrol();
        }
    }

    private bool InFieldOfView(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        float angle = Vector3.Angle(directionToTarget, transform.forward);
        
        // ตรวจสอบว่ามุมระหว่าง Ghost และ Target อยู่ใน FOV หรือไม่
        if (angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToTarget, out hit, viewDistance))
            {
                if (hit.collider.gameObject == chaseTarget)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void GhostChase()
    {
        if (Vector3.Distance(chaseTarget.transform.position, transform.position) < stopOffset)
        {
            return;
        }
        var lookAtPosition = 
            Quaternion.LookRotation(chaseTarget.transform.position - transform.position);

        transform.rotation =
            Quaternion.Slerp(transform.rotation, lookAtPosition, chaseRotateSpeed * Time.deltaTime);
        
        transform.Translate(0,0, chaseSpeed * Time.deltaTime);
    }

    private void GhostPatrol()
    {
        // ใส่โค้ดสำหรับระบบ Patrol ที่นี่
    }
}