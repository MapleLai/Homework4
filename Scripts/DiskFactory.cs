using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myGame{
	public class DiskFactory : MonoBehaviour {
		private static DiskFactory _instance = null ;
		public List<Disk> used = new List<Disk> ();
		public List<Disk> free = new List<Disk> ();


		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		void Awake () {
			_instance = this;
		}

		public static DiskFactory getInstance(){
			return _instance;
		}

		public GameObject getDisk(int round){
			GameObject newDisk;
			if (free.Count > 0) {
				newDisk = free [0].gameObject;
				free.Remove (free [0]);
			}
			else {
				newDisk =Instantiate(Resources.Load("Prefabs/disk"), Vector3.zero, Quaternion.identity) as GameObject;   
				newDisk.AddComponent<Disk>();
			}

			float ran = 0f;
			switch (round) {
			case 1:
				ran = UnityEngine.Random.Range (0f, 1f);
				break;
			case 2:
				ran = UnityEngine.Random.Range (0.5f, 2f);
				break;
			case 3:
				ran = UnityEngine.Random.Range (0.8f, 2.5f);
				break;
			case 4:
				ran = UnityEngine.Random.Range (1f, 2.5f);
				break;
			case 5:
				ran = UnityEngine.Random.Range (2f, 3f);
				break;
			default:
				break;
			}

			if (ran < 1f) {
				newDisk.GetComponent<Disk>().color = Color.white;
				newDisk.GetComponent<Disk>().speed = 4f; 
				newDisk.GetComponent<Renderer>().material.color = Color.white;
			}
			else if (ran < 2f) {
				newDisk.GetComponent<Disk>().color = Color.gray;
				newDisk.GetComponent<Disk>().speed = 6f; 
				newDisk.GetComponent<Renderer>().material.color = Color.gray;
			}
			else if (ran < 3f) {
				newDisk.GetComponent<Disk>().color = Color.black;
				newDisk.GetComponent<Disk>().speed = 8f; 
				newDisk.GetComponent<Renderer>().material.color = Color.black;
			}
			  
			newDisk.GetComponent<Disk>().direction = new Vector3(UnityEngine.Random.Range(0f, 1f), 
																													 UnityEngine.Random.Range(0f, 1f), 
																													 0);  
			used.Add(newDisk.GetComponent<Disk>());
			newDisk.SetActive (true);
			return newDisk;
		}

	}
}