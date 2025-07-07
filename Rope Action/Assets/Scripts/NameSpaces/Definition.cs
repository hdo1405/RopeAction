using UnityEngine;

namespace Definition
{
    /// <summary>
    /// 사칙연산 가능한 자료형만 사용하세요
    /// </summary>
    //public class Stat<T> where T : System.Numerics.INumber<T>
    //{
    //    T baseStat;
    //    T add;
    //    float multiple;
    //    float lastMultiple;
    //    T lastAdd;

    //    T FinalStat()
    //    {
    //        return (baseStat * multiple + add) * lastMultiple + lastAdd;
    //    }
    //}

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

        // -----Constructor-----
        /// <summary>
        /// 기본을 대입형식으로 만들어놓음 -> new로 만들때 무조건 값이 있도록.. 나중에 property도 읽기전용으로 할까봐 아예
        /// </summary>
        /// <param name="atkPow"> 공격력</param>
        /// <param name="subject"> 공격 주체</param>
        public Damage(float atkPow, GameObject subject)
        {
            this.attackPower = atkPow;
            this.subject = subject;
        }

        // -----Method-----
    }

    public enum Camp
    {
        PLAYER = 0,
        ENEMY,
    }
}