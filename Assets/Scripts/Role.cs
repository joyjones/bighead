using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Role : MonoBehaviour
	{
		public Head SubHead
		{
			get { return headObj.GetComponentInChildren<Head> (); }
		}

		public bool IsHitting
		{
			get { return elapsedHitTime >= 0; }
		}

        public bool IsMoving
        {
            get { return moveDirection.x != 0 || moveDirection.z != 0; }
        }

		public Vector3 FaceDirection
		{
			get { return faceDirection; }
		}

		// Use this for initialization
		void Start ()
		{
			controller = GetComponent<CharacterController>();
			headObj = transform.Find ("Head").gameObject;
            animator = GetComponent<Animator>();
		}

		private void Update()
		{
			if (elapsedHitTime >= 0)
			{
				elapsedHitTime += Time.deltaTime;
				if (elapsedHitTime > 0.1F)
					ShowHit (false);
			}
			Logic ();

            animator.SetFloat("moveSpeed", IsMoving ? moveDirection.magnitude : 0);
//            var info = animator.GetCurrentAnimatorStateInfo(0);
//            if (info.IsName("BeHit"))
//            {
//                
//            }
		}

		protected virtual void Logic()
		{
			moveDirection.y -= gravity * Time.deltaTime;
			if (!(faceDirection.x == 0 && faceDirection.y == 0))
				transform.rotation = Quaternion.LookRotation (new Vector3(faceDirection.x, 0, faceDirection.z));
			controller.Move (moveDirection * Time.deltaTime);
		}

		protected virtual void ShowHit(bool hit)
		{
			headObj.transform.rotation = transform.rotation;
			var dir = faceDirection;
			dir.y = 0;
			var pos = transform.position;
			pos.y += 1.4F;
			if (hit)
				headObj.transform.position = pos + dir * 0.3F;
			else
				headObj.transform.position = pos;
			elapsedHitTime = hit ? 0 : -1;
			if (hit)
			{
				foreach (var role in SubHead.CollidingRoles)
				{
					role.HitBody(this);
				}
			}
            animator.SetBool("hitting", hit);
		}

		public void HitBody(Role oppRole)
		{
			string oppName = oppRole.name;
			Debug.Log (oppName + " hitted me!");
			var dir = oppRole.FaceDirection;
            controller.Move(dir * 1);
            animator.SetTrigger("behit");
		}

		public float speed = 6.0F;
		public float jumpSpeed = 8.0F;
		public float gravity = 20.0F;

		protected CharacterController controller;
		protected Vector3 moveDirection = Vector3.zero;
		protected Vector3 faceDirection = Vector3.zero;
		protected GameObject headObj;

		private float elapsedHitTime = -1;
        private Animator animator;

		void OnCollisionEnter(Collision collision)
		{
			Debug.Log ("OnCollisionEnter:" + collision.ToString ());
		}

		void OnCollisionExit(Collision collision)
		{
			Debug.Log ("OnCollisionExit:" + collision.ToString ());
		}

		void OnCollisionStay(Collision collision)
		{
			Debug.Log ("OnCollisionStay:" + collision.ToString ());
		}

		void OnTriggerEnter(Collider collider)
		{
			//		Debug.Log("开始接触");
		}

		void OnTriggerExit(Collider collider)
		{
			//		Debug.Log("接触结束");
		}

		void OnTriggerStay(Collider collider)
		{
			//		Debug.Log("接触持续中");
		}
	}
}