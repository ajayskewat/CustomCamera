using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCamera : MonoBehaviour {

    Transform trans;
    Vector3 pos;
    Vector3 rot;    
    Vector3 downPos;
    Vector3 panPos;    
    Vector3 prevPos;    
    Vector3 vec;    

    float panMovement = 10F;   
    float movementSpeed = 0.1F;
    float zMovementSpeed = 5F;    
    float rotationalSpeed = 0.5F;    

    float _x;
    float _y;
    float _z;
    
    float focusSpeed = 20;
    bool focusSelected = false;
    bool mouseClick = false;
    
    public Texture2D panText, rotateTexture;
    public Transform selectedTarget;
    void Start () {
        trans = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {        

        if(!EventSystem.current.IsPointerOverGameObject())
        {
            GetInput();
            MovementCamera();
        }

        if (focusSelected)
            FocusToSelection();

    }

    void MovementCamera()
    {
        if( pos != Vector3.zero)
        {
            trans.Translate(pos, Space.Self);
        }      
    }
    void GetInput()
    {
        mouseClick = false;
        if ( Input.GetKeyDown(KeyCode.F))
        {
            selectedTarget = RuntimeGizmos.TransformGizmo.target;

            if (selectedTarget != null)            
                focusSelected = true;
        }
        

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movementSpeed = 0.5F;
        }
        else
            movementSpeed = 0.1F;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            print("W pressed.....................");
            pos = new Vector3(0, 0,1 * movementSpeed);
        }
        else
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            pos = new Vector3(0,0, -1 * movementSpeed);
        }
        else
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            pos = new Vector3(1 * movementSpeed, 0, 0);
        }
        else
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            pos = new Vector3(-1 * movementSpeed, 0, 0);
        }
        else
        if (Input.GetMouseButtonDown(2))
        {
            downPos = Input.mousePosition;
            panPos = trans.position;
        }
        else
        if (Input.GetMouseButton(2))
        {
            _x = downPos.x - Input.mousePosition.x;
            _y = downPos.y - Input.mousePosition.y;          
            vec = new Vector3( Mathf.Clamp(_x, -1.0F * panMovement, 1.0F * panMovement),
                Mathf.Clamp(_y, -1.0F * panMovement, 1.0F  *panMovement), 0);           
            if ( vec != prevPos)
            {
                trans.Translate(vec, Space.Self);
                prevPos = vec;
                downPos = Input.mousePosition;
            }
            mouseClick = true;
            Cursor.SetCursor(panText, Vector2.zero, CursorMode.Auto);
        }
        else
        if (Input.mouseScrollDelta.y > 0)
        {
             pos = new Vector3(0, 0, zMovementSpeed * 1);        }
        else
        if (Input.mouseScrollDelta.y < 0)
        {
           pos = new Vector3(0, 0,  zMovementSpeed * -1);
        }
        else
            pos = Vector3.zero;

        if (Input.GetMouseButtonDown(1))
        {
            downPos = Input.mousePosition;
            rot = trans.eulerAngles;
        }
        else
        if (Input.GetMouseButton(1))
        {
            Vector3 pos1 = Vector3.zero;
            _x =  (downPos.x - Input.mousePosition.x) * -1;
            _y = (downPos.y - Input.mousePosition.y) ;
            
            pos1 = new Vector3( _y * rotationalSpeed, _x * rotationalSpeed, 0);// Input.mousePosition.normalized.z * mouseSpeed);
            trans.eulerAngles = rot + pos1;
            mouseClick = true;
            Cursor.SetCursor( rotateTexture, Vector2.zero, CursorMode.Auto);
        }

        if(mouseClick == false)
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        

    }

    void FocusToSelection()
    {
        if( RuntimeGizmos.TransformGizmo.target != null )
        {
            Focus();
        }
    }

    void Focus()
    {
        if( (  trans.position - selectedTarget.position ).sqrMagnitude < 450 )
        {
            focusSelected = false;
        }
        else
        {
           if(selectedTarget != null)
            {
                trans.position = Vector3.Lerp(trans.position, selectedTarget.position, focusSpeed * Time.deltaTime);
                trans.LookAt(selectedTarget);
            }
            else
                focusSelected = false;
        }

    }
}
