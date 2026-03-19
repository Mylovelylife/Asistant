using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ReceiveMaterial.Helper
{
    public class IPAddressRange
    {
        public IPAddress Start { get; }
        public IPAddress End { get; }

        public IPAddressRange(IPAddress start, IPAddress end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(IPAddress ipAddress)
        {
            var ip = ipAddress.GetAddressBytes();
            var start = Start.GetAddressBytes();
            var end = End.GetAddressBytes();

            return Compare(ip, start) >= 0 && Compare(ip, end) <= 0;
        }

        private int Compare(byte[] ip1, byte[] ip2)
        {
            for (int i = 0; i < ip1.Length; i++)
            {
                int result = ip1[i].CompareTo(ip2[i]);
                if (result != 0)
                    return result;
            }
            return 0;
        }

        
    }
}
