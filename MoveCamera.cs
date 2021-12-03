using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform target= null;
    public Vector3 offset;

    float rx;
    float ry;
    public float rotSpeed = 60;

    public float dist = 10.0f;
    public float height = 5.0f;
    public float smoothRate = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (target == null)
        {
            if(sceneName != "clothing_store")
            {
                target = GameObject.Find("boy_re(Clone)").transform;
            }
            
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MoveLookAt()
    {

        transform.position = target.position + offset;

        //메인카메라가 바라보는 방향입니다.
        Vector3 dir = transform.localRotation * Vector3.forward;
        //카메라가 바라보는 방향으로 팩맨도 바라보게 합니다.
        target.transform.localRotation = transform.localRotation;
        //팩맨의 Rotation.x값을 freeze해놓았지만 움직여서 따로 Rotation값을 0으로 세팅해주었습니다.
        target.transform.localRotation = new Quaternion(0, target.transform.localRotation.y, 0, target.transform.localRotation.w);
        //바라보는 시점 방향으로 이동합니다.
        gameObject.transform.Translate(dir * 0.1f * Time.deltaTime);
    }

    public void CameraSpin()
    {
        
        //1. 마우스 입력 값을 이용한다.
        float mx = Input.GetAxis("Mouse X"); //게임창에서 마우스를 왼쪽 오른쪽으로 이동할때 마다 (왼 -음수 : 오른 +양수)
        float my = Input.GetAxis("Mouse Y"); //게임창에서 마우스를 왼쪽 오른쪽으로 이동할때 마다 (아래 -음수 : 위 +양수)

        rx += rotSpeed * my * Time.deltaTime;
        ry += rotSpeed * mx * Time.deltaTime;

        //rx 회전 각을 제한 (화면 밖으로 마우스가 나갔을때 x축 회전 덤블링 하듯 계속 도는 것을 방지)
        rx = Mathf.Clamp(rx, -80, 80);
        //x을 돌리는 이유 x축이 이동이 아니라 x축을 회전 해서 위아래 보는 방향은 x축이여야 한다.

        //2. 회전을 한다.
        transform.eulerAngles = new Vector3(-rx, ry, 0);
    }
}
