using UnityEngine;

namespace Definition
{
    public class Damage
    {
        // ----- variables -----
        private GameObject subject;
        public GameObject Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private float attackPower;
        public float AttackPower
        {
            get { return attackPower; }
            set { attackPower = value; }
        }

        // -----Method-----
    }

    public enum Camp
    {
        PLAYER = 0,
        ENEMY,
    }
}