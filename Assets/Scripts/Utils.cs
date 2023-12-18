using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class Interval
    {
        public float start { get 
            {
                return _start;
            }
            set
            {
                _start = value;
            }
        }

        public float end
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
            }
        }

        [SerializeField]
        private float _start, _end;

        public Interval(float start, float end)
        {
            _start = start;
            _end = end;
        }

        public bool Contains(float value)
        {
            if (value >= _start && value <= _end) 
            {
                return true;
            }
            return false;
        }
    }
}
