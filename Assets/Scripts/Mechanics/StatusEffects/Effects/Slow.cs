using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slow", menuName = "Scriptable Objects/Status Effects/Slow")]
public class Slow : StatusData
{
    public override void Apply(StatusController c)
    {
       // throw new System.NotImplementedException();
    }

    public override void Remove(StatusController controller)
    {
        //throw new System.NotImplementedException();
    }
}
