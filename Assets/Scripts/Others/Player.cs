using System;
using UnityEngine;

namespace Others
{
    public class Player : Body, IShoot
    {
        public float moveSpeed;
        private float hp;
        [SerializeField]
        private float maxHp;

        private Vector3 input;
        private bool dead;
        private void Start()
        {
            hp = maxHp;
        }

        private void Update() // 60fps
        {
            input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (!dead)
            {
                Move();
            }
        }

        private void Move()
        {
            //transform.position = input.normalized * (moveSpeed * Time.deltaTime); // 保持速度相对不变
            
            // 令角色前方与移动方向一致
            if (input.magnitude > 0.1f)
            {
                //transform.forward = input;
            }
            // 以上移动方式没有考虑阻挡，因此使用下面的代码限制移动范围
            Vector3 temp = transform.position;
            const float BORDER = 20;
            if (temp.z > BORDER) { temp.z = BORDER; }
            if (temp.z < -BORDER) { temp.z = -BORDER; }
            if (temp.x > BORDER) { temp.x = BORDER; }
            if (temp.x < -BORDER) { temp.x = -BORDER; }
        }
    }

    public class Enemy : Body, IShoot
    {
        
    }

    public class Body : MonoBehaviour
    {
        
    }

    public interface IShoot
    {
        
    }
}