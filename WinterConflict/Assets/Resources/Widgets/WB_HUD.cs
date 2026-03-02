//==========================================( Neverway 2025 )=========================================================//
// Author
//  Liz M.
//
// Contributors
//
//
//====================================================================================================================//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WB_HUD : MonoBehaviour
{
    #region========================================( Variables )======================================================//
    /*-----[ Inspector Variables ]------------------------------------------------------------------------------------*/


    /*-----[ External Variables ]-------------------------------------------------------------------------------------*/


    /*-----[ Internal Variables ]-------------------------------------------------------------------------------------*/
    private Coroutine inflictCorruptionCoroutine;
    private bool hasLightFaded;


    /*-----[ Reference Variables ]------------------------------------------------------------------------------------*/
    public Image lanternFill;

    public Image lanternFlame;
    //public Transform lanternParticles;
    public Color onLanternDrain;
    public Color onLanternSafe;


    #endregion


    #region=======================================( Functions )======================================================= //

    /*-----[ Mono Functions ]-----------------------------------------------------------------------------------------*/


    /*-----[ Internal Functions ]-------------------------------------------------------------------------------------*/
    public void LateUpdate()
    {
    }

    private void OnDestroy()
    {
        //GameInstance.Get<GI_AudioManager>().SetMusicPitch(1f);
    }


    /*-----[ External Functions ]-------------------------------------------------------------------------------------*/


    //public struct LanternParticle
    //{
    //    public Transform transform;
    //    public Vector2 velocity;
    //    public float size;
    //    public float timer;
    //    public float maxTimer;
    //
    //    public void InitParticle(Transform transform)
    //    {
    //        Random.State oldState = Random.state;
    //
    //        Random.InitState(new System.Random().Next());
    //        this.transform = transform;
    //        velocity = Vector2.up * Random.Range(-1, -3);
    //        velocity = Vector2.right * Random.Range(-2, 2);
    //
    //        Random.state = oldState;
    //    }
    //    public void UpdateParticle()
    //    {
    //
    //    }
    //}

    #endregion
}
