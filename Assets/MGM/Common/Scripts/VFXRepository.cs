using System;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.VFX;

public sealed class VFXRepository
{


    private static List<GameObject> VFXList = new List<GameObject>();

    public class SpawnedVFX
    {

        public bool FirstParticuleSpawned;
        public GameObject VFX;

        public SpawnedVFX(GameObject _VFX) {
            VFX = _VFX;
        }


    }

    public static List<SpawnedVFX> VFXPlaying = new List<SpawnedVFX>();

    const string EventName = "start";
    const string VFX_List = "VFX Prefabs";

    private static Transform GetOrCreateVFXHierarchy()
    {
        GameObject go = GameObject.Find(VFX_List);
        go = go  == null ? new GameObject(VFX_List) : go;

        return go.transform;
    }

    public static int AddVFX(GameObject vfx)
    {
        vfx.transform.parent = GetOrCreateVFXHierarchy();
        VFXList.Add(vfx);
        return VFXList.IndexOf(vfx);
    }

    public static void PlayVfxAt(int vfxIndex, LocalToWorld ltw)
    {
        GameObject vfx = GameObject.Instantiate(VFXList[vfxIndex]);
        vfx.gameObject.transform.position = ltw.Position;
        vfx.gameObject.transform.rotation = Quaternion.LookRotation(ltw.Forward,ltw.Right);
        vfx.GetComponent<VisualEffect>().SendEvent(EventName);
        VFXPlaying.Add(new SpawnedVFX(vfx));
    }

}