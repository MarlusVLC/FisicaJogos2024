using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject theX;
    private Camera camera;
    
    public float timeMove = 5f;
    public float timeCounter;
    public float timeAngle = 5f;
    private bool wasClicked;
    private bool rotated;
    private float angleVelocity;
    float angle;

   // s = s0 + v* t;
   // s - s0 = v*t
   //v = (s - s0)/t
    private Vector3 velocity;    
    private Vector3 target;
    
    
    
    

    private void Start()
    {
        camera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !wasClicked)
        {
            Vector3 newPos = camera.ScreenToWorldPoint(Input.mousePosition);

            theX.gameObject.SetActive(true);
            
            newPos.z = 10;
			theX.transform.position = newPos;
            
            target = (newPos - transform.position);
            
            float newAngle = Mathf.Atan2 ( target.y, target.x )* Mathf.Rad2Deg - 90f;
		    
            angle = transform.rotation.eulerAngles.z;
            float angularDistance = (newAngle - angle);
            
            if ( angularDistance > 180.0f ) angularDistance -= 360.0f;
		    if ( angularDistance < -180.0f ) angularDistance += 360.0f;
            
		    angleVelocity =  angularDistance/timeAngle;
            velocity = (newPos - transform.position)/timeMove;
            velocity.z = 0f;
            
            wasClicked = true;
            rotated = false;
        }

        if (wasClicked && !rotated)
        {
            Rotate();
        }else if (wasClicked)
        {
            Move();
        }
    }

    void Move()
    {
        if (timeCounter < timeMove)
        {
            transform.position += velocity * Time.deltaTime;

            timeCounter += Time.deltaTime;
        }
        else
        {
            timeCounter = 0;
            wasClicked = false;
            rotated = false;
            theX.gameObject.SetActive(false);
        }
    }

    void Rotate()
    {
        if (timeCounter < timeAngle)
        {
            angle += angleVelocity * Time.deltaTime;
            transform.eulerAngles = new Vector3(0,0,angle);
            timeCounter += Time.deltaTime;
        }
        else
        {
            timeCounter = 0;
            rotated = true;
        }
    }
}
