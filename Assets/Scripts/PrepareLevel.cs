using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrepareLevel : MonoBehaviour {

	public GameObject GetReady;
	public GameObject GO;

    void Awake()
    {
        GetComponent<Timer>().timeAvailable = SharedSettings.ConfigTime;
    }

	void Start () {

        GameObject.Find("GUI/LevelName/LevelName").GetComponent<Text>().text = SharedSettings.LevelName[SharedSettings.LoadLevel];
        StartCoroutine(PrepareRoutine());
	
	}

	IEnumerator PrepareRoutine()
	{
		//等待1秒
		yield return new WaitForSeconds(1.0f);

		//显示GetReady
		GetReady.SetActive(true);

		//等待2秒
		yield return new WaitForSeconds(2.0f);
		GetReady.SetActive(false);

		GO.SetActive(true);

		yield return new WaitForSeconds(1.0f);
		GO.SetActive(false);
	}	
}