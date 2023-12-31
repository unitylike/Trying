using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpSpeed = 20f;
    public Rigidbody2D rigid;
    public enum Type { blue, red };
    public Type playerType;
    //public float maxSpeed = 1f;
    public bool isground = true;
    public float h;

    string itemtag;
    public ItemManager im;
    public float speedchange = 1.2f;

    public float[] maxtime_item;
    public float time_item;
    public timegauge timegauge;
    public timegauge timegauge_background;

    public bool abletochange = true;

    public bool ismove = true;

    // Start is called before the first frame update
    void Start()
    {
        //if(playerType == Type.blue)
        //{ }
        ismove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerType == Type.red && ismove)
        {
            if (Input.GetKeyDown(KeyCode.W) && isground)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
                isground = false;
            }
            if (Input.GetKey(KeyCode.A))
            {
                h = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                h = 1f;
            }
            if (!(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D)))
            {
                h = 0f;
            }
            rigid.velocity = new Vector2(h * moveSpeed, rigid.velocity.y);
        }
        else if (playerType == Type.blue && ismove)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && isground)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);
                isground = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                h = -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                h = 1f;
            }
            if (!(Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.RightArrow)))
            {
                h = 0f;
            }
            rigid.velocity = new Vector2(h * moveSpeed, rigid.velocity.y);
        }

        if(time_item > 0f)
        {
            time_item -= Time.deltaTime;
            timegauge.time = time_item;
            timegauge_background.time = time_item;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            itemtag = collision.gameObject.tag.ToString();
            im.isitemremain = false;
            Destroy(collision.gameObject);
            //Debug.Log("!");

            if (itemtag == "faster")
            {
                moveSpeed *= speedchange;
                //Debug.Log("1");
                //StartCoroutine(wait(maxtime_item[0], "faster", 0));
                StartCoroutine(wait(0, "faster"));
            }
            if (itemtag == "slower")
            {
                moveSpeed *= (1 / speedchange);
                //Debug.Log("2");
                //StartCoroutine(wait(maxtime_item[1], "slower", 1));
                StartCoroutine(wait(1, "slower"));
            }
            if (itemtag == "change" && abletochange)
            {
                abletochange = false;
                //im.ischange = true;
                StartCoroutine(wait(2, "change"));
                //StartCoroutine(waitforchange(1f));
            }
            if(itemtag == "stop" && (im.stopstate == "none2"))
            {
                im.stopstate = playerType.ToString();
                StartCoroutine(wait(3, "stop"));
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            isground = true;
        }
        //if (collision.gameObject.CompareTag("faster"))
        /*if (itemtag == "faster")
        {
            moveSpeed *= speedchange;
            Debug.Log("1");
            StartCoroutine(wait(3f, "faster"));
        }
        if (itemtag == "slower")
        {
            moveSpeed *= (1/speedchange);
            Debug.Log("2");
            StartCoroutine(wait(3f, "slower"));
        }*/
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isground = false;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isground = true;
        }
    }

    //IEnumerator wait(float time, string type, int i)
    IEnumerator wait(int i, string type)
    {
        time_item = maxtime_item[i];
        timegauge.maxtime = maxtime_item[i];
        timegauge_background.maxtime = maxtime_item[i];
        yield return new WaitForSeconds(maxtime_item[i]);
        time_item = 0f;
        //StopCoroutine("wait");
        if (type == "faster")
        {
            moveSpeed *= (1/speedchange);
            //Debug.Log(type);
        }
        else if(type == "slower")
        {
            moveSpeed *= speedchange;
            //Debug.Log(type);
        }
        else if(type == "change")
        {
            abletochange = true;
            im.ischange = true;
        }
        else if(type == "stop")
        {
            im.stopstate = "none";
        }
    }
    /*IEnumerator waitforchange(float f)
    {
        yield return new WaitForSeconds(f);
        abletochange = true;
        im.ischange = true;
    }*/
}

