using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class TankBrain
    {


        public abstract void OnCollisionEnter(Collision collision);

        public abstract void OnStart(GameObject obj);

        public abstract void Update();

        public abstract void FixedUpdate();

        public abstract void HitBy(GameObject attacker);

    }
}
