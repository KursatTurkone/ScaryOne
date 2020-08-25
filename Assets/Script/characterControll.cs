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
    [Range(1,3)]public float hiz = 0; 
    RaycastHit hit;
    RaycastHit hitFire;

    bool fireControl = false;
    GameObject camera, pos1, pos2;
    public GameObject sight;
    Transform skeleton=null;
    public Vector3 offset;
    public RuntimeAnimatorController withFire;
    public RuntimeAnimatorController withoutFire;

    void Start () {
        animator = GetComponent<Animator>();
        physics = GetComponent<Rigidbody>();
        cameraDistance = headCam.transform.position - transform.position;
        camera = Camera.main.gameObject;
        pos1 = headCam.transform.Find("Pos").gameObject;
        pos2 = headCam.transform.Find("Pos2").gameObject;
        skeleton = animator.GetBoneTransform(HumanBodyBones.Chest);
       
    }
    public void LateUpdate()
    {
        if (fireControl&&hitFire.distance>3) {
            skeleton.LookAt(hitFire.point);
            
            skeleton.rotation = skeleton.rotation * Quaternion.Euler(offset);
        }
       
    }
	void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.LeftShift) && !fireControl)
        {
            hiz *= 2;

            animator.SetBool("RunParam", true); 
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)&& !fireControl)
        {
            hiz /= 2;
            animator.SetBool("RunParam", false);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("JumpParam", true);
          
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("JumpParam", false);

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            sight.SetActive(true); 
            fireControl = true; 
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            sight.SetActive(false);
            fireControl = false;
        }


    }
	
	void FixedUpdate ()
    {

        Movement();
        if (!fireControl)
        {
            animator.runtimeAnimatorController = withoutFire;
            camera.transform.position = Vector3.Lerp(camera.transform.position, pos1.transform.position, 0.1f);
        }
        if (fireControl)
        {
            animator.runtimeAnimatorController = withFire;
            camera.transform.position = Vector3.Lerp(camera.transform.position, pos2.transform.position, 0.1f);
        }
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
        vec.Normalize(); 
        physics.position += vec * Time.fixedDeltaTime * hiz;
    }
    void Rotation()
    {
       
        headCam.transform.position = transform.position + cameraDistance;
        headRotUpDown += Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * -150;
        headRotLeftRight += Input.GetAxis("Mouse X") * Time.fixedDeltaTime * 150;
        headRotUpDown = Mathf.Clamp(headRotUpDown, -20, 20);
        headCam.transform.rotation = Quaternion.Euler(headRotUpDown, headRotLeftRight, transform.eulerAngles.z);
        if (horizontal != 0 || vertical != 0&&!fireControl)
        {
        
            Turning(); 
        }
        else if (fireControl)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            Physics.Raycast(ray, out hitFire);
            Debug.DrawLine(ray.origin, hitFire.point);

            Turning(); 
        }


    }
   void Turning()
    {
        Physics.Raycast(Vector3.zero, headCam.transform.GetChild(0).forward, out hit);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z)), 0.2f);
        Debug.DrawLine(Vector3.zero, hit.point);
    }
    void JumpParamFalse()
    {
        
        animator.SetBool("JumpParam" , false); 

    }
    void JumpAddForce()
    {
        physics.AddForce(0, Time.deltaTime * 10000, 0);
    }
}
