using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myGame{
	public class CCDisappear : SSAction {
		DiskFactory diskFactory = null;
		ScoreRecorder scoreRecorder = null;
		public static CCDisappear getSSAction(){
			CCDisappear action = ScriptableObject.CreateInstance<CCDisappear> ();
			return action;
		}
			
		public override void Start () {
			diskFactory = DiskFactory.getInstance ();
			scoreRecorder = ScoreRecorder.getInstance ();
		}
	
		public override void Update () {
			Disk tmp = null;  
			foreach (Disk disk in diskFactory.used)  {  
				if (gameobject == disk.gameObject)  {  
					tmp = disk;  
				}  
			}  
			if (tmp != null) {  
				if (gameobject.GetComponent<Disk> ().color == Color.white) {
					scoreRecorder.scoreAdd (1);
				}
				if (gameobject.GetComponent<Disk> ().color == Color.white) {
					scoreRecorder.scoreAdd (2);
				}
				if (gameobject.GetComponent<Disk> ().color == Color.white) {
					scoreRecorder.scoreAdd (3);
				}

				scoreRecorder.scoreAdd (1);
				tmp.gameObject.SetActive(false);  
				diskFactory.free.Add(tmp);  
				diskFactory.used.Remove(tmp);  
			} 
			this.destroy = true;  
			this.callback.SSActionEvent(this);
		}
	
	}
}