using System.Collections;
using UnityEngine;

public abstract class AdsInitiator<T>: KR.ManagerSingleton<T> where T : MonoBehaviour
{

    protected override void Awake()
	{
        base.Awake();
        Init();
       
	}
    protected IEnumerator Start(){
        yield return new WaitForEndOfFrame();
		KR.Monetization.Init();
            
    }

	public abstract void Init();
}
