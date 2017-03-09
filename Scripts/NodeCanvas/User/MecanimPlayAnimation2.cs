using UnityEngine;
//
//namespace NodeCanvas.Actions{
//	
//	//[ScriptName("Play Animation BACKUP")]
//	//[ScriptCategory("Mecanim")]
//	[AgentType(typeof(Animator))]
//	public class MecanimPlayAnimation2 : ActionTask{
//		
//		public int layerIndex;
//		[RequiredField]
//		public string stateName;
//		[SliderField(0,1)]
//		public float transitTime = 0.25f;
//		
//		public bool waitUntilFinish;
//		
//		[GetFromAgent]
//		private Animator animator;
//		private AnimatorStateInfo info;
//		
//		private bool played;
//		
//		protected override string info{
//			get {return "Mec.PlayAnimation '" + stateName + "'";}
//		}
//		
//		protected override void OnExecute(){
//			played = false;
//			animator.CrossFade(stateName, transitTime, layerIndex);
//		}
//		
//		protected override void OnUpdate(){
//			
//			info = animator.GetCurrentAnimatorStateInfo(layerIndex);
//			
//			if (waitUntilFinish){
//				
//				if (info.IsName(stateName))
//				{ 
//					played = true;
//					Debug.Log(string.Format("{0} >= {1} {2}", elapsedTime, (info.length * (1 / animator.speed)), elapsedTime >= info.length));
//					if(elapsedTime >= (info.length * (1 / animator.speed))) {
//						EndAction();				
//					}
//				} else if (played) {
//					EndAction();
//				}
//				
//			} else {			
//				if (elapsedTime >= transitTime)
//					EndAction();
//			}
//		}
//	}
//}