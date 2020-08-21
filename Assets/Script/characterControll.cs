using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControll : MonoBehaviour {

    float horizontal=0, vertical = 0;
    Animator animator;
    Rigidbody physics;
    public GameObject headCam;
    Vector3 cameraDistance;
    float headRotUpDown = 0,headRotLeftRight=0;
    RaycastHit hit; 
	void Start () {
        animator = GetComponent<Animator>();
        physics = GetComponent<Rigidbody>();
        cameraDistance = headCam.transform.position - transform.position; 
	}
	
	
	void FixedUpdate () {

        Movement();
        Rotation();

        animator.SetFloat("Horizontal",horizontal);
        animator.SetFloat("Vertical", vertical); 
	}
    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 vec = new Vector3(horizontal, 0, vertical);
        vec = transform.TransformDirection(vec);
        physics.position += vec * Time.deltaTime * 3;
    }
    void Rotation()
    {
       
        headCam.transform.position = transform.position + cameraDistance;
        headRotUpDown += Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * -150;
        headRotLeftRight += Input.GetAxis("Mouse X") * Time.fixedDeltaTime * 150;
        headRotUpDown = Mathf.Clamp(headRotUpDown, -20, 20);
        headCam.transform.rotation = Quaternion.Euler(headRotUpDown, headRotLeftRight, transform.eulerAngles.z);
        if (horizontal != 0 || vertical != 0)
        {
            Physics.Raycast(Vector3.zero, headCam.transform.GetChild(0).forward, out hit);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z)), 0.2f);
            Debug.DrawLine(Vector3.zero, hit.point);
        }


    }
}
