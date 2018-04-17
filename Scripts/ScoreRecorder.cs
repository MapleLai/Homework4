using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myGame{
	public class ScoreRecorder : MonoBehaviour {
		private static ScoreRecorder _instance = null;
		int score = 0;
		int round = 0;

		// Use this for initialization
		void Start () {
		}
	
		// Update is called once per frame
		void Update () {
		}

		void Awake () {
			_instance = this;
		}

		public static ScoreRecorder getInstance(){
			return _instance;
		}

		public void addRound(){
			round++;
		}

		public int getRound(){
			return round;
		}

		public void setRound(int _round){
			round = _round;
		}

		public void scoreAdd(int _score){
			score+=_score;
		}

		void OnGUI(){
			GUI.Label (new Rect (300, 10, 100, 50), "Round:"+round.ToString());
			GUI.Label (new Rect (500, 10, 100, 50), "Score:"+score.ToString());
		}

	}
}