using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class VirtualJoystick : MonoBehaviour
{
    [Header("JOYSTICK Parameters")]
    [SerializeField] private Joystick joystick;
    
    [Header("Mobile Buttons")]
    [SerializeField] private GameObject jumpButton;
    [SerializeField] private GameObject leftPadButton;
    [SerializeField] private GameObject fireButton;
    
    

    private Vector2 _snappedInput =  Vector2.zero;
    
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

#if  UNITY_ANDROID
    private void Start()
    {
        print("Android");
        leftPadButton.SetActive(true);
        fireButton.SetActive(true);
        jumpButton.SetActive(true);
        
    }
#endif

    private void Update()
    {
        
        Vector3 move = new Vector3(joystick.Horizontal, 0 , joystick.Vertical).normalized;

         _controller.Move(move * (Time.deltaTime * 5f));

    }
   
}
