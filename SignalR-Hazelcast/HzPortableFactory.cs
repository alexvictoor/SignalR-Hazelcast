using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazelcast.IO.Serialization;

namespace SignalR.Hazelcast
{
    public class HzPortableFactory : IPortableFactory
    {
        public IPortable Create(int classId)
        {

            if (HzConfiguration.ClassId == classId)
            {
                return new HzMessage();
            }
            else
            {
                return null;
            }
                
        }
    }
}
