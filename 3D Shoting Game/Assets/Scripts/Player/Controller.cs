using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, upSpeed, downSpeed, rotSensivility;
    [SerializeField]
    float gravity;
    [SerializeField]
    private float moveSmoothTime, rotSmoothTime;

    public GameObject enemy;

    private bool isLocked = true, isJumped = false;
    private float rotX = 0;
    private float rotY = 0;

    private SmoothFloat svelX, svelY, svelZ, srotX, srotY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        svelX = new SmoothFloat();
        svelY = new SmoothFloat();
        svelZ = new SmoothFloat();

        srotX = new SmoothFloat();
        srotY = new SmoothFloat();
    }

    private void Update()
    {
        CharacterRotate();
        ChangeLockState();
    }

    private void CharacterRotate()
    {
        if (!isLocked) return;

        rotX -= Input.GetAxisRaw("Mouse Y") * rotSensivility * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -40, 40);

        rotY += Input.GetAxisRaw("Mouse X") * rotSensivility * Time.deltaTime;

        var nexX = srotX.Update(rotX, rotSmoothTime);
        var nexY = srotY.Update(rotY, rotSmoothTime);

        transform.eulerAngles = new Vector3(nexX, nexY, 0);
    }

    private void ChangeLockState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isLocked = !isLocked;
            if (isLocked) Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
        }
    }

    private void LateUpdate()
    {
        CharacterMove();
        CharacterJump();
    }

    private void CharacterMove()
    {
        //float y = 0;
        //if (Input.GetKey(KeyCode.Space)) y = upSpeed;
        //if (Input.GetKey(KeyCode.LeftShift)) y = -downSpeed;

        var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        var vel = dir * moveSpeed;
        //vel.y = y;
        vel *= Time.fixedDeltaTime; // 입력한 값에 따른 이동 위치 설정
        vel = transform.TransformDirection(vel);

        var nexX = svelX.Update(vel.x, moveSmoothTime);
        //var nexY = svelY.Update(vel.y, moveSmoothTime);
        var nexZ = svelZ.Update(vel.z, moveSmoothTime);

        transform.position += new Vector3(nexX, 0, nexZ);
    }

    void CharacterJump()
    {
        if (isJumped) return;

        if(InputManager.PressJump())
        {
            isJumped = true;
            StartCoroutine(ExJump());
        }
        else if (InputManager.PressDescend())
        {
            isJumped = true;
            StartCoroutine(ExDescend());
        }
    }

    IEnumerator ExJump()
    {
        float jumpSpeed = upSpeed;

        while (true)
        {
            Debug.Log("ExJump");

            transform.position += new Vector3(0, jumpSpeed * Time.deltaTime, 0);

            jumpSpeed -= gravity;

            if (jumpSpeed <= 0) break;

            yield return null;
        }
        isJumped = false;
    }

    IEnumerator ExDescend()
    {
        float descendSpeed = 0;

        while (true)
        {
            transform.position += new Vector3(0, descendSpeed * Time.deltaTime, 0);

            descendSpeed -= gravity;

            if (descendSpeed <= -downSpeed) break;

            yield return null;
        }
        isJumped = false;
    }

    public class SmoothFloat
    {
        private float _current;
        private float _stock;

        public float Update(float target, float smoothTime)
        {
            return _current = Mathf.SmoothDamp(_current, target, ref _stock, smoothTime);
        }
    }
}
