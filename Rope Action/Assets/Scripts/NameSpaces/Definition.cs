using UnityEngine;
using System.Numerics;

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

    /// <summary>
    /// float형 스텟
    /// </summary>
    [System.Serializable]
    public class FStat
    {
        [Tooltip("베이스 스텟")]
        public float baseStat;

        [Tooltip("곱한 다음 더해줄 추가스텟")]
        public float add = 0f;

        [Tooltip("베이스 스텟에 곱해줄 스텟")]
        public float multiple = 1f;

        [Tooltip("최종적으로 더해줄 추가스텟")]
        public float lastAdd = 0f;

        [Tooltip("add이후 값에 곱해줄 스텟")]
        public float lastMultiple = 1f;

        //-----Constructor-----
        /// <summary>
        /// float Stat생성자 -- add나 multiple은 기본적으로 0과 1
        /// </summary>
        /// <param name="baseStat">기본 스텟</param>
        public FStat(float baseStat)
        {
            this.baseStat = baseStat;
        }

        //-----function-----
        /// <summary>
        /// 곱하고 더한 최종 스텟 반환
        /// </summary>
        /// <returns>최종 스텟(float)</returns>
        public float FinalStat()
        {
            return ((((baseStat * multiple) + add) * lastMultiple) + lastAdd);
        }
    }

    /// <summary>
    /// int형 스텟
    /// </summary>
    [System.Serializable]
    public class IStat
    {
        [Tooltip("베이스 스텟")]
        public int baseStat;

        [Tooltip("곱한 다음 더해줄 추가스텟")]
        public int add = 0;

        [Tooltip("베이스 스텟에 곱해줄 스텟")]
        public float multiple = 1f;

        [Tooltip("최종적으로 더해줄 추가스텟")]
        public int lastAdd = 0;

        [Tooltip("add이후 값에 곱해줄 스텟")]
        public float lastMultiple = 1f;

        //-----Constructor-----
        /// <summary>
        /// int Stat생성자 -- add나 multiple은 기본적으로 0과 1
        /// </summary>
        /// <param name="baseStat">기본 스텟</param>
        public IStat(int baseStat)
        {
            this.baseStat = baseStat;
        }

        //-----function-----
        /// <summary>
        /// 곱하고 더한 최종 스텟 반환(int 형변환 후 반환)
        /// </summary>
        /// <returns>최종 스텟(int)</returns>
        public int FinalStat()
        {
            return (int)((((baseStat * multiple) + add) * lastMultiple) + lastAdd);
        }
    }

    [System.Serializable]
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