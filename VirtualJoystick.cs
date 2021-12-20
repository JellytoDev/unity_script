using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    public RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField,Range(10,150)]
    private float leverRange;

    private bool isInput;
    private Vector2 inputDirection;

    public CharacterMove controller;
    public MoveCamera cameraSpin;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Start()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        Debug.Log("Begin");
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);

        Debug.Log("Drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        Debug.Log("End");
        isInput = false;
        controller.Move(Vector2.zero);
    }

    private void ControlJoystickLever(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition * 3;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange;
    }

    private void InputControlVector()
    {
        controller.Move(inputDirection);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInput)
        {
            InputControlVector();
        }
        else
        {
            controller.CameraToSpin();
        }
    }
}
