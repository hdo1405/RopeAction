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