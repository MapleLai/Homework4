using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myGame{
	public class SceneController : MonoBehaviour {
		private static SceneController _instance = null;
		private DiskFactory diskFactory = null;
		private ScoreRecorder scoreRecorder = null;
		private CCActionManager actionManager = null;
		private int diskNumber = 0;
		private int isStart = 0;
		private float time = 0;

		// Use this for initialization
		void Start () {
			diskFactory = DiskFactory.getInstance ();
			scoreRecorder	= ScoreRecorder.getInstance (); 
			actionManager = CCActionManager.getInstance ();
		}
	
		// Update is called once per frame
		void Update () {
			if (diskNumber>0 && isStart == 1 && time > 1f) {
				diskNumber--;
				flyDisk();
				time = 0;
			}
			else {
				time += Time.deltaTime;	
			}

			if (diskNumber == 0 && isStart == 1) {
				isStart = 2;
			} 
			else if (diskNumber == 0 && isStart == 2) {
				scoreRecorder.addRound ();
				if (scoreRecorder.getRound () > 5) {
					isStart = -1;
					scoreRecorder.setRound (0);
				} 
				else {
					diskNumber = 10;
					isStart = 1;
				}
			}

		}

		void Awake () {
			_instance = this;
		}

		public static SceneController getInstance(){
			return _instance;
		}

		void OnGUI(){
			if (isStart == 0) {
				if (GUI.Button (new Rect (380, 150, 100, 50), "Start")) {
					isStart = 1;
					diskNumber = 10;
					scoreRecorder.setRound (1);
				}
			} 
			else if (isStart == -1 && diskFactory.used.Count == 0) {
				if (GUI.Button (new Rect (380, 150, 100, 50), "Game Over")) {
					isStart = 0;
				}
			}
		}

		private void flyDisk(){
			GameObject disk = diskFactory.getDisk(scoreRecorder.getRound());
			disk.transform.position = new Vector3 (-12, -4, 0); 
			actionManager.addFlyAction (disk);
		}

	}
}