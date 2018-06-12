using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KR
{
	public class PoolInitiator : KR.ManagerSingleton<PoolInitiator>
	{

		protected override void Awake()
		{
			base.Awake();
			Pool.Init();
		}

	}
}
