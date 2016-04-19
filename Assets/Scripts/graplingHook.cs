using UnityEngine;
using System.Collections;

public class graplingHook : MonoBehaviour {

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform hook;
    [SerializeField]
    private Material mat;
    [SerializeField]
    private float xScale;
    [SerializeField]
    private float fireRange;
    [SerializeField]
    private bool hitSomething;
    [SerializeField]
    private bool outRanged;
    [SerializeField]
    private float hookSpeed;
    [SerializeField]
    private float forceHook;
    [SerializeField]
    private float forcePull;
    private bool hooked;
    private Rigidbody rigidPlayer;

    // Use this for initialization
    void Start () {
        hooked = false;
        hitSomething = false;
        outRanged = false;
        StartCoroutine(grapler());
        rigidPlayer = player.GetComponent<Rigidbody>();
	}
    
    // Update is called once per frame
    void Update()
    {
        
        //print(hitSomething);
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!outRanged)
            {
                RaycastHit hit = new RaycastHit();
                //Gizmos.DrawRay(hit);
                if(hooked == true)
                {
                    rigidPlayer.AddForce((hook.transform.position - player.transform.position) * (forcePull * 1000) * Time.smoothDeltaTime);
                }
                if (Physics.Raycast(transform.position, transform.forward, out hit, fireRange))
                {
                    OnHitObject(hit);
                    fireRange = 0;
                    //Debug.DrawRay(transform.position, transform.forward);
                    if (!hitSomething)
                    {
                        hook.position = hit.point;
                    }
                    hitSomething = true;
                }
                else
                {
                    if (fireRange <= 20 && !hitSomething)
                    {
                        fireRange += hookSpeed;
                        Debug.Log("test");
                    }
                    else
                    {
                        if (!hitSomething)
                        {
                            fireRange = 0;
                            outRanged = true;
                            StopCoroutine(grapler());
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("test2"); 
            fireRange = 0;
            outRanged = false;
            hooked = false;
            hitSomething = false;
        }
        if (!hitSomething)
        {
            hook.rotation = transform.rotation;
            hook.position = transform.position + transform.forward.normalized * (fireRange);
        }
        else
        {
            //hook.position = transform.position + transform.forward.normalized * (fireRange - 0.5f);
        }
    }

    IEnumerator grapler()
    {
        while (true)
        {
            xScale = Vector3.Distance(transform.position, hook.position);
            GameObject grapple = new GameObject();
            grapple.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            grapple.AddComponent<LineRenderer>();
            //print(xScale);
            mat.mainTextureScale = new Vector2(xScale, 1);
            LineRenderer lineRender = grapple.GetComponent<LineRenderer>();
            lineRender.material = mat;
            lineRender.SetWidth(0.2f, 0.2f);
            lineRender.SetPosition(0, grapple.transform.position);
            lineRender.SetPosition(1, hook.position);
            yield return new WaitForEndOfFrame();
            GameObject.Destroy(grapple);
        }
    }
    void OnHitObject(RaycastHit hit)
    {
        //Rigidbody rigidObject = hit.collider.GetComponent<Rigidbody>();
        GameObject moveableObject = hit.collider.gameObject;
        if (moveableObject.tag == "Moveable")
        {
            moveableObject.GetComponent<Rigidbody>().AddForce((player.transform.position - moveableObject.transform.position) * (forceHook*1000) * Time.smoothDeltaTime);
        }
        else
        {
            hooked = true;
        }
    }
}
