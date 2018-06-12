using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EaAdsInitiator<T>: KR.ManagerSingleton<T> where T : MonoBehaviour
{

    protected override void Awake()
	{
        base.Awake();
        Init();
       
	}
    protected IEnumerator Start(){
        yield return new WaitForEndOfFrame();
        EaMonetization.Init();
            
    }

	public abstract void Init();
}
