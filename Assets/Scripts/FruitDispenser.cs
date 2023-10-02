using UnityEngine;
using System.Collections;

public class FruitDispenser : MonoBehaviour {

    public GameObject[] fruits;
    public GameObject bomb;

    public float z;

    public float powerScale;

    public bool pause = false;
    bool started = false;

    //每个水果发射的计时
    public float timer = 1.75f;

	void Start () {
	
	}
	
	void Update () {

        if (pause) return;

        timer -= Time.deltaTime;

        if (timer <= 0 && !started)
        {
            timer = 0f;
            started = true;
        }

        if (started)
        {
            if (SharedSettings.LoadLevel == 0)
            {
                if (timer <= 0)
                {
                    FireUp();
                    timer = 2.5f;
                }
            }
            else 
            if (SharedSettings.LoadLevel == 1)
            { 
                if (timer <= 0)
                {
                    FireUp();
                    timer = 2.0f;
                }
            }
            else
            if (SharedSettings.LoadLevel == 2)
            {
                if (timer <= 0)
                {
                    FireUp();
                    timer = 1.75f;
                }
            }
            else
            if (SharedSettings.LoadLevel == 3)
            {
                if (timer <= 0)
                {
                    FireUp();
                    timer = 1.5f;
                }
            }
        }	
	}

    void FireUp()
    {
        if (pause) return;

        //每次必有的水果
        Spawn(false);

        if (SharedSettings.LoadLevel == 2 && Random.Range(0, 10) < 2)
        {
            Spawn(true);
        }
        if(SharedSettings.LoadLevel == 3 && Random.Range(0, 10) < 4)
        {
            Spawn(true);
        }

        //炸弹
        if (SharedSettings.LoadLevel == 1 && Random.Range(0, 100) < 10)
        {
            Spawn(true);
        }
        if (SharedSettings.LoadLevel == 2 && Random.Range(0, 100) < 20)
        {
            Spawn(true);
        }
        if (SharedSettings.LoadLevel == 3 && Random.Range(0 ,100) < 30)
        {
            Spawn(true);
        }
    }

    void Spawn(bool isBomb)
    {
        float x = Random.Range(-3.1f, 3.1f);

        z = Random.Range(14f, 19.8f);

        GameObject ins;

        if (!isBomb)
            ins = Instantiate(fruits[Random.Range(0, fruits.Length)], transform.position + new Vector3(x, 0, z), Random.rotation) as GameObject;
        else
            ins = Instantiate(bomb, transform.position + new Vector3(x, 0, z), Random.rotation) as GameObject;

        float power = Random.Range(1.5f, 1.8f) * -Physics.gravity.y * 1.5f * powerScale;
        Vector3 direction = new Vector3(-x * 0.05f * Random.Range(0.3f, 0.8f), 1, 0);
        direction.z = 0f;

        ins.GetComponent<Rigidbody>().velocity = direction * power;
        ins.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * 0.1f, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
