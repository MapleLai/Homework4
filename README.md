# 飞碟游戏规则介绍
飞碟有3种，分值由低到高为白、灰、黑，分值越高飞行速度越快。游戏一共有5轮，每轮射出10个飞碟，随着轮数的增加，3种飞碟所占的比例将会改变，当第5轮的10个飞碟全部击毁或飞出视界，游戏结束。
# 游戏界面
+ 游戏开始界面
![开始](https://raw.githubusercontent.com/MapleLai/Homework4/master/Screenshot/%E5%BC%80%E5%A7%8B.png)
+ 游戏过程界面
![过程](https://raw.githubusercontent.com/MapleLai/Homework4/master/Screenshot/%E8%BF%87%E7%A8%8B.png)
+ 游戏结束界面
![结束](https://raw.githubusercontent.com/MapleLai/Homework4/master/Screenshot/%E7%BB%93%E6%9D%9F.png)
# 代码
+ Disk类  
保存飞碟的基本信息
<pre>using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myGame{
	public class Disk : MonoBehaviour {
		public Vector3 size;
		public Color color;
		public float speed;
		public Vector3 direction;
	}
}</pre>
+ DiskFactory类  
管理飞行中和闲置中的飞碟的类，需要时随机生成一种飞碟提供给场记。
<pre>using System.Collections;
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
			//根据轮数产生随机数，随机数范围代表3种飞碟的比例。
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
}</pre>
+ ISSActionCallback类  
动作接口基类
<pre>using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace myGame{
	public enum SSActionEventType : int { Started, Competeted }

	public interface ISSActionCallback  {  
		void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,  
			int intParam = 0, string strParam = null, Object objectParam = null);  
	}  
}</pre>
+ SSActionManager类  
动作管理器基类
<pre>using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myGame{
	public class SSActionManager : MonoBehaviour {  
		private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();  
		private List<SSAction> waitingAdd = new List<SSAction>();  
		private List<int> waitingDelete = new List<int>();  

		// Use this for initialization  
		void Start()  {  }  

		// Update is called once per frame  
		protected void Update()  {  
			foreach (SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;  
			waitingAdd.Clear();  

			foreach (KeyValuePair<int, SSAction> kv in actions)  {  
				SSAction ac = kv.Value;  
				if (ac.destroy)  {  
					waitingDelete.Add(ac.GetInstanceID());  
				}  
				else if (ac.enable)  {  
					ac.Update();  
				}  
			}  

			foreach (int key in waitingDelete)  {  
				SSAction ac = actions[key]; actions.Remove(key); DestroyObject(ac);  
			}  
			waitingDelete.Clear();  
		}  

		public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)  {  
			action.gameobject = gameobject;  
			action.transform = gameobject.transform;  
			action.callback = manager;  
			waitingAdd.Add(action);  
			action.Start();  
		}  
	} 
}</pre>
+ CCActionManager类 
动作管理者，给准备发射的飞碟添加“飞行”动作，以及当飞行中的飞碟被点击时添加“消失”动作。
<pre>using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myGame{
	public class CCActionManager : SSActionManager, ISSActionCallback  {  
		private static CCActionManager _instance = null;
		public CCFlyAction flyAction;
		public CCDisappear disappear;

		// Use this for initialization  
		protected void Start () {   
		}  

		// Update is called once per frame  
		protected new void Update () {
			if (Input.GetMouseButtonDown (0) ) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					Debug.Log ("click");

					disappear = CCDisappear.getSSAction ();
					this.RunAction (hit.collider.gameObject, disappear, this);
				}
			}

			base.Update();
			
		}

		protected void Awake(){
			_instance = this;
		}

		public static CCActionManager getInstance(){
			return _instance;
		}

		public void addFlyAction(GameObject gameobject){
			flyAction = CCFlyAction.getSSAction ();
			this.RunAction (gameobject, flyAction, this);
		}

		public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,  
			int intParam = 0, string strParam = null, Object objectParam = null)  {    
		}  
	}

}</pre>
+ SSAction类  
动作基类
<pre>using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myGame {
	public class SSAction : ScriptableObject  {  
		public bool enable = true;  
		public bool destroy = false;  

		public GameObject gameobject  { get; set; }  
		public Transform transform { get; set; }  
		public ISSActionCallback callback { get; set; }  

		protected SSAction() { }  

		public virtual void Start()  {  
			throw new System.NotImplementedException();  
		}  

		public virtual void Update()  {  
			throw new System.NotImplementedException();  
		}  
	}
}</pre>
