using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PortScanner
{
    static class PortScanningService
    {
        private static TcpClient _scan = null;



        public enum Status
        {
            Opened,
            Closed
        }

        

        public static Dictionary<int, Status> Scan(IPAddress iPAddress, int lowerThreshold, int higherThreshold)

        {
            Dictionary<int, Status> portMap = new Dictionary<int, Status>();
            using (_scan = new TcpClient())
            {
                while(lowerThreshold <= higherThreshold)
                {
                    try
                    {
                        _scan.Connect(iPAddress, lowerThreshold);
                        portMap.Add(lowerThreshold, Status.Opened);
                        lowerThreshold++;

                    }
                    catch(SocketException)
                    {
                        portMap.Add(lowerThreshold, Status.Closed);
                        lowerThreshold++;
                    }
                }
                
            }
            return portMap;
        }

       
    }

    
}
