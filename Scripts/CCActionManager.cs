using System.Collections;
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

}