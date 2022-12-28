using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Others
{
    public class PrefabSpawner : MonoBehaviour
    {
        public GameObject player;
        private List<GameObject> _objects = new();
        private float tempTime = 0f;


        private void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 pos = new(Random.Range(0, 5), 0.0f, Random.Range(0, 5));
                var obj = Instantiate(player, pos, Quaternion.identity);
                obj.GetComponent<Player>().moveSpeed = 2;
                _objects.Add(obj);
            }
            
            // 点击按钮之后0.5s内才会做xxx事情
            // 3种办法
            Invoke(nameof(DoSomething), 0.5f);
            // 点击按钮之后 0.2s xxx 0.3s 
            StartCoroutine(Test());
        }

        private void Update()
        {
            /*tempTime += Time.deltaTime;
            if (tempTime > 0.5f)
            {
                dosomething();
            }*/
            
            foreach (var item in _objects)
            {
                Vector3 pos = new(Random.value * Time.deltaTime, 0.0f, Random.value * Time.deltaTime);
                item.transform.Translate(pos);
            }
        }

        private void DoSomething()
        {
            
        }

        private IEnumerator Test()
        {
            Debug.Log("00000");
            yield return new WaitForSeconds(0.2f);
            Debug.Log("11111");
            yield return new WaitForSeconds(0.3f);
            Debug.Log("22222");
        }
    }
}