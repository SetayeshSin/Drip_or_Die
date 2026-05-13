using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;  
using System.Collections;

public class moveIceCream : MonoBehaviour
{
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Button jump;
    [SerializeField] private Rigidbody rb;
    public Animator animator;
    public Transform CameraTransform;
    

    private bool moveLeftFlag = false;
    private bool moveRightFlag = false;
    private bool moveUpFlag = false;
    private bool doubleJumpFlag = false;

    private int upCount = 0;
    private int jumpCount = 0;

    void Start()
    {
        AddPointerEvents(left,  () => moveLeftFlag  = true, () => moveLeftFlag  = false);
        AddPointerEvents(right, () => moveRightFlag = true, () => moveRightFlag = false);
        jump.onClick.AddListener(moveUp); 
    }
    void FixedUpdate()
    {
      if(Input.GetKey(KeyCode.Home)){
            if(!Collision(-1)){
            Vector3 newPosition= transform.position;
            newPosition.x -= 0.05f  ; 
            transform.position = newPosition; 
            Vector3 newPositionCamera= CameraTransform.position;
            newPositionCamera.x -= 0.04f  ; 
            CameraTransform.position = newPositionCamera; 
         } 
      }
      else if(Input.GetKey(KeyCode.End)){
            if(!Collision(1)){
            Vector3 newPosition= transform.position;
            newPosition.x += 0.05f  ; 
            transform.position = newPosition; 
            Vector3 newPositionCamera= CameraTransform.position;
            newPositionCamera.x += 0.04f  ; 
            if(newPositionCamera.x < 21){
              CameraTransform.position = newPositionCamera; 
            }
          }
      }  
      if(Input.GetKeyDown(KeyCode.PageUp)){
            moveUp();
      }
      if (moveLeftFlag){
         if(!Collision(-1)){
            Vector3 newPosition= transform.position;
            newPosition.x -= 0.05f  ; 
            transform.position = newPosition; 
            Vector3 newPositionCamera= CameraTransform.position;
            newPositionCamera.x -= 0.04f  ; 
            CameraTransform.position = newPositionCamera; 
         }
        }
      else if (moveRightFlag){
          if(!Collision(1)){
            Vector3 newPosition= transform.position;
            newPosition.x += 0.05f  ; 
            transform.position = newPosition; 
            Vector3 newPositionCamera= CameraTransform.position;
            newPositionCamera.x += 0.04f  ; 
            if(newPositionCamera.x < 21){
              CameraTransform.position = newPositionCamera; 
            }
         }
        } 
        if (moveUpFlag){
          Vector3 newPosition= transform.position;
          newPosition.y += 0.2f  ; 
          transform.position = newPosition;
          upCount++;
         }
         if(upCount==15)moveUpFlag = false;
    }
    private void moveUp(){
      if(jumpCount==0){
       ToStatic();
       moveUpFlag = true;
       upCount = 0;
       jumpCount++;
     }
      else if(jumpCount==1){
        ToStatic();
          moveUpFlag = false;
          rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
          rb.useGravity = false;
          StartCoroutine(manageDoubleJump());
      }
    }
    public IEnumerator manageDoubleJump(){
       yield return new WaitForSeconds(0.02f);
       moveUpFlag = true;
       upCount = 0;
       jumpCount++;
       rb.useGravity = true;

    }
    private void AddPointerEvents(Button btn, System.Action onDown, System.Action onUp)
    {
        EventTrigger trigger = btn.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = btn.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry downEntry = new EventTrigger.Entry();
        downEntry.eventID = EventTriggerType.PointerDown;
        downEntry.callback.AddListener((data) => onDown());
        EventTrigger.Entry upEntry = new EventTrigger.Entry();
        upEntry.eventID = EventTriggerType.PointerUp;
        upEntry.callback.AddListener((data) => onUp());
        trigger.triggers.Add(upEntry);
        trigger.triggers.Add(downEntry);  
    }
     private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 newPosition= transform.position;
            Vector3 newPositionCamera= CameraTransform.position;
            if(newPositionCamera.y - newPosition.y > 2.5f || newPositionCamera.y - newPosition.y <0.0f){
              newPositionCamera.y = newPosition.y + 1.0f  ; 
              CameraTransform.position = newPositionCamera;
            }
            
            jumpCount = 0;
            ToDynamic();
        }
    }
     private bool Collision(int x)
    {
        RaycastHit hit;
        if(x>0){
          if (Physics.Raycast(transform.position, transform.right , out hit, 1.0f))
           {
              if (hit.collider.CompareTag("Ground"))
              {
                return true; 
              }
          }
          return false;
        }
        else{
          if (Physics.Raycast(transform.position, -transform.right , out hit, 1.0f))
           {
              if (hit.collider.CompareTag("Ground"))
              {
                return true; 
              }
          }
          return false;
        }
    }
    public void ToStatic(){  
        animator.SetTrigger("static");
    }
    public void ToDynamic(){
        animator.SetTrigger("dynamic");    
     }
}