using UnityEngine;
using System.Collections;

public class MouseControl : MonoBehaviour {

    public static MouseControl Instance;

    Vector2 screenInp;

    bool fire = false;
    bool fire_prev = false;
    bool fire_down = false;
    bool fire_up = false;

    public LineRenderer trail;

    Vector2 start, end;

    Vector3[] trailPositions = new Vector3[10];

    int index;
    int linePart = 0;
    float lineTimer = 1.0f;

    [SerializeField]
    float trail_alpha = 0f;
    int raycastCount = 10;

    //积分
    public int points;

    bool started = false;

    //果汁效果预制品
    public GameObject[] splashPrefab;
    public GameObject[] splashFlatPrefab;

	void Awake () {
        Instance = this;
	}

    void BlowObject(RaycastHit hit)
    {
        if (hit.collider.gameObject.tag != "destroyed")
        {
            //生成切开的水果的部分
            hit.collider.gameObject.GetComponent<ObjectKill>().OnKill();

            //删除切到的水果
            Destroy(hit.collider.gameObject);

            if (hit.collider.tag == "red") index = 0;
            if (hit.collider.tag == "yellow") index = 1;
            if (hit.collider.tag == "green") index = 2;

            //水果泼溅效果
			if (hit.collider.gameObject.tag != "bomb")
			{
	            Vector3 splashPoint = hit.point;
	            splashPoint.z = 4;
	            Instantiate(splashPrefab[index], splashPoint, Quaternion.identity);
	            splashPoint.z += 4;
	            Instantiate(splashFlatPrefab[index], splashPoint, Quaternion.identity);
			}

            //切到炸弹
            if (hit.collider.gameObject.tag != "bomb") points++;
            else points -= 5;
            points = points < 0 ? 0 : points;

            hit.collider.gameObject.tag = "destroyed";
        }
    }
	
	void Update () {

        Vector2 Mouse;

        screenInp.x = Input.mousePosition.x;
        screenInp.y = Input.mousePosition.y;

        fire_down = false;
        fire_up = false;

        fire = Input.GetMouseButton(0);
        if (fire && !fire_prev) fire_down = true;
        if (!fire && fire_prev) fire_up = true;
        fire_prev = fire;

        //控制画线
        Control();

        //设置线段的相应颜色
        Color c1 = new Color(1, 1, 0, trail_alpha);
        Color c2 = new Color(1, 0, 0, trail_alpha);
        trail.startColor = c1;
        trail.endColor = c2;

        if (trail_alpha > 0) trail_alpha -= Time.deltaTime;
	}

    void Control()
    {
        //线段开始
        if (fire_down)
        {
            trail_alpha = 0.5f;

            start = screenInp;
            end = screenInp;

            started = true;

            linePart = 0;
            lineTimer = 0.25f;
            AddTrailPosition();
        }

        //鼠标拖动中
        if (fire && started)
        {
            start = screenInp;

            var a = Camera.main.ScreenToWorldPoint(new Vector3(start.x, start.y, 10));
            var b = Camera.main.ScreenToWorldPoint(new Vector3(end.x, end.y, 10));

            //鼠标（触屏）移动大于0.1后，判定为有效移动，进行一次采样
            if (Vector3.Distance(a, b) > 0.1f)
            {
                linePart++;
                lineTimer = 0.25f;
                AddTrailPosition();
            }

            trail_alpha = 0.75f;

            end = screenInp;
        }

        //线的alpha值大于0.5的时候，可以做射线检测
        if (trail_alpha > 0.5f)
        {
            for (var p = 0; p < 8; p++)
            { 
                for (var i = 0; i < raycastCount; i++)
                {
                    Vector3 s = Camera.main.WorldToScreenPoint(trailPositions[p]);
                    Vector3 e = Camera.main.WorldToScreenPoint(trailPositions[p+1]);
                    Ray ray = Camera.main.ScreenPointToRay(Vector3.Lerp(s, e, i / raycastCount));

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("fruit")))
                    {
                        BlowObject(hit);
                        return;
                    }
                }

            }
        }

        if (trail_alpha <= 0) linePart = 0;

        //根据时间加入一个点
        lineTimer -= Time.deltaTime;
        if (lineTimer <= 0f)
        {
           linePart++;
           AddTrailPosition();
           lineTimer = 0.01f;
        }

        if (fire_up && started) started = false;

        //拷贝线段的数据到linerenderer
        SendTrailPosition();
    }

    void AddTrailPosition()
    {
        if (linePart <= 9)
        {
            for (int i = linePart; i <= 9; i++)
            {
                trailPositions[i] = Camera.main.ScreenToWorldPoint(new Vector3(start.x, start.y, 10));
            }
        }
        else
        {
            for (int ii = 0; ii <= 8; ii++)
            {
                trailPositions[ii] = trailPositions[ii + 1];
            }

            trailPositions[9] = Camera.main.ScreenToWorldPoint(new Vector3(start.x, start.y, 10));
        }
    }

    void SendTrailPosition()
    {
        var index = 0;
        foreach(Vector3 v in trailPositions)
        {
            trail.SetPosition(index, v);
            index++;
        }
    }
}
