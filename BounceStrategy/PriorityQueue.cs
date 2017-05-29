using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
     public class PriorityQueue
    {
        private List<GameObject> queue;

        public PriorityQueue()
        {
            queue = new List<GameObject>();
        }

        public void enqueue(GameObject elem)
        {
            bool added = false;

            if (queue.Count == 0)
            {
                queue.Add(elem);
            }
            else
            {
                for(int i = 0; i < queue.Count; i++)
                {
                     if(queue[i].GetComponent<NodeOfGraphScript>().minDist > elem.GetComponent<NodeOfGraphScript>().minDist)
                    {
                        queue.Insert(i, elem);
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    queue.Add(elem);
                }

            }           
        }

        public GameObject dequeue()
        {
            if (queue.Count != 0)
            {
                GameObject outcome = queue[0];
                queue.RemoveAt(0);
                return outcome;
            }
            else
            {
                return null;
            }
        }

        public void remove(GameObject toRemove)
        {
            queue.Remove(toRemove);
        }

        public int count()
        {
            return queue.Count;
        }

    }
}
