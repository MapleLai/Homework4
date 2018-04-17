using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myGame{
	public class CCFlyAction : SSAction {
		DiskFactory diskFactory = null;

		public static CCFlyAction getSSAction(){
			CCFlyAction action = ScriptableObject.CreateInstance<CCFlyAction> ();
			return action;
		}
			
		public override void Start () {
			diskFactory = DiskFactory.getInstance();
		}

		public override void Update () {
			transform.Translate(gameobject.GetComponent<Disk>().direction * 
													gameobject.GetComponent<Disk>().speed * 
													Time.deltaTime); 

			if (this.transform.position.x < -12 ||
					this.transform.position.x > 12 || 
				this.transform.position.y < -6 || 
				this.transform.position.y > 6) {
				Disk tmp = null;  
				foreach (Disk disk in diskFactory.used)  {  
					if (gameobject == disk.gameObject)  {  
						tmp = disk;  
					}  
				}  
				if (tmp != null) {  
					tmp.gameObject.SetActive(false);  
					diskFactory.free.Add(tmp);  
					diskFactory.used.Remove(tmp);  
				} 
				this.destroy = true;  
				this.callback.SSActionEvent(this);  
			}

		}

	}
}