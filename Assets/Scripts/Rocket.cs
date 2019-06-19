using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Camera mainCamera;
    Rigidbody rigidBody;
    AudioSource audioSource;
    ParticleSystem particleSystemFlame;
    Vector3 collisionPoint;
    [SerializeField]
    float thrust = 30;
    [SerializeField]
    float rotationThrust = 60;

    Vector3 mainCameraStartPosition;

    bool cameraMustZoomIn = false;
    bool cameraMustZoomOut = false;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCameraStartPosition = mainCamera.transform.position;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        particleSystemFlame = GetComponentInChildren<ParticleSystem>();
        //particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (cameraMustZoomIn)
        {
            Vector3 targetPosition = transform.TransformPoint(new Vector3(0, 5, -10));
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, collisionPoint - new Vector3(0,0,15), Time.deltaTime * 2);
            if (Vector3.Distance(mainCamera.transform.position, collisionPoint) <= 16)
            {
                //for (int i = 0; i < 5; i++)
                //{
                //    float XScale = transform.localScale.x;
                //    float YScale = transform.localScale.y;
                //    float ZScale = transform.localScale.z;

                //    XScale = Mathf.MoveTowards(XScale, XScale*1.3F, 0.5f);
                //    ZScale = Mathf.MoveTowards(ZScale, ZScale*1.3F, 0.5f);

                //    transform.localScale = new Vector3(XScale, YScale, ZScale);
                //}
                cameraMustZoomIn = false;
            }
        }
        else if (cameraMustZoomOut)
        {
            Vector3 targetPosition = mainCameraStartPosition;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 2);
            if (Vector3.Distance(mainCamera.transform.position, targetPosition) <= 1)
            {
                cameraMustZoomOut = false;
            }
        }
        ProessInput();
    }

    private void ProessInput()
    {
        Thrust();
        Rotate();

    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!particleSystemFlame.isPlaying)
                particleSystemFlame.Play();
            if (!audioSource.isPlaying)
                audioSource.Play();
            rigidBody.AddRelativeForce(Vector3.up * thrust);
        }
        else
        {
            if (particleSystemFlame.isPlaying)
                particleSystemFlame.Stop();
            if (audioSource.isPlaying)
                audioSource.Stop();

        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            //Quaternion deltaRotation = Quaternion.Euler(Vector3.forward * Time.deltaTime * rotationThrust);
            //rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
            transform.Rotate(Vector3.forward * Time.deltaTime * rotationThrust, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //Quaternion deltaRotation = Quaternion.Euler(Vector3.back * Time.deltaTime * rotationThrust);
            //rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);

            transform.Rotate(Vector3.back * Time.deltaTime * rotationThrust, Space.World);

        }
        rigidBody.freezeRotation = false;
    }
    // private Vector3 velocity = Vector3.zero;

    private void OnCollisionEnter(Collision collision)
    {
        if (!cameraMustZoomIn && collision.gameObject.tag == "Obstacle")
        {
            cameraMustZoomIn = true;
            collisionPoint = collision.contacts[0].point;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            if (!cameraMustZoomIn)
                cameraMustZoomIn = true;
            if (cameraMustZoomOut)
                cameraMustZoomOut = false;
            collisionPoint = collision.contacts[0].point;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!cameraMustZoomOut)
            cameraMustZoomOut = true;
    }
}
