using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Head : MonoBehaviour
	{
		public Role ParentRole
		{
			get;
			private set;
		}

		public Role[] CollidingRoles
		{
			get { return collidingRoles.ToArray(); }
		}

		private List<Role> collidingRoles = new List<Role>();

		// Use this for initialization
		void Start ()
		{
			ParentRole = transform.parent.gameObject.GetComponentInChildren<Player> () as Role;
			if (ParentRole == null)
				ParentRole = transform.parent.gameObject.GetComponentInChildren<Enemy> () as Role;
		}
		
		// Update is called once per frame
		void Update ()
		{
		}

		void OnTriggerEnter(Collider collider)
		{
			if (collider.gameObject.name == "Head")
			{
				var p = collider.gameObject.transform.parent;
				if (p != null)
				{
					var role = p.gameObject.GetComponentInChildren<Role> ();
					if (role != null && !collidingRoles.Contains (role))
					{
						collidingRoles.Add(role);
						if (ParentRole.IsHitting)
							role.HitBody (ParentRole);
					}
				}
			}
		}

		void OnTriggerExit(Collider collider)
		{
			var p = collider.gameObject.transform.parent;
			if (p != null)
			{
				var role = p.gameObject.GetComponentInChildren<Role>();
				if (role != null)
					collidingRoles.Remove(role);
			}
		}

		void OnTriggerStay(Collider collider)
		{
		}
	}
}