using UnityEngine;

namespace Definition
{
    /// <summary>
    /// ��Ģ���� ������ �ڷ����� ����ϼ���
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
        /// �⺻�� ������������ �������� -> new�� ���鶧 ������ ���� �ֵ���.. ���߿� property�� �б��������� �ұ�� �ƿ�
        /// </summary>
        /// <param name="atkPow"> ���ݷ�</param>
        /// <param name="subject"> ���� ��ü</param>
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