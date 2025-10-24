using System;
using System.Text;
using UnityEngine;

namespace _Scripts.Data
{
    [Serializable]
    public class CaptionsData
    {
        [SerializeField] private string timestamp;
        public string timestampStr
        {
            get { return timestamp; }
        }

        [TextArea] public string bodyText;

        public TimeSpan TimeStamp
        {
            get
            {
                var aStringBuilder = new StringBuilder(timestamp);
                aStringBuilder.Remove(8, 1);
                aStringBuilder.Insert(8, ".");

                return TimeSpan.Parse(aStringBuilder.ToString());
            }
        }
    }
}
