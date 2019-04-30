using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


[Serializable]
public struct EntityRef : IComponentData
{
   public Entity entity;
}
