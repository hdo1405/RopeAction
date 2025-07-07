using UnityEngine;
using System.Numerics;

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

    /// <summary>
    /// float�� ����
    /// </summary>
    [System.Serializable]
    public class FStat
    {
        [Tooltip("���̽� ����")]
        public float baseStat;

        [Tooltip("���� ���� ������ �߰�����")]
        public float add = 0f;

        [Tooltip("���̽� ���ݿ� ������ ����")]
        public float multiple = 1f;

        [Tooltip("���������� ������ �߰�����")]
        public float lastAdd = 0f;

        [Tooltip("add���� ���� ������ ����")]
        public float lastMultiple = 1f;

        //-----Constructor-----
        /// <summary>
        /// float Stat������ -- add�� multiple�� �⺻������ 0�� 1
        /// </summary>
        /// <param name="baseStat">�⺻ ����</param>
        public FStat(float baseStat)
        {
            this.baseStat = baseStat;
        }

        //-----function-----
        /// <summary>
        /// ���ϰ� ���� ���� ���� ��ȯ
        /// </summary>
        /// <returns>���� ����(float)</returns>
        public float FinalStat()
        {
            return ((((baseStat * multiple) + add) * lastMultiple) + lastAdd);
        }
    }

    /// <summary>
    /// int�� ����
    /// </summary>
    [System.Serializable]
    public class IStat
    {
        [Tooltip("���̽� ����")]
        public int baseStat;

        [Tooltip("���� ���� ������ �߰�����")]
        public int add = 0;

        [Tooltip("���̽� ���ݿ� ������ ����")]
        public float multiple = 1f;

        [Tooltip("���������� ������ �߰�����")]
        public int lastAdd = 0;

        [Tooltip("add���� ���� ������ ����")]
        public float lastMultiple = 1f;

        //-----Constructor-----
        /// <summary>
        /// int Stat������ -- add�� multiple�� �⺻������ 0�� 1
        /// </summary>
        /// <param name="baseStat">�⺻ ����</param>
        public IStat(int baseStat)
        {
            this.baseStat = baseStat;
        }

        //-----function-----
        /// <summary>
        /// ���ϰ� ���� ���� ���� ��ȯ(int ����ȯ �� ��ȯ)
        /// </summary>
        /// <returns>���� ����(int)</returns>
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