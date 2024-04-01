using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontenery
{
    public interface IHazardNotifier
    {
        void SendHazardNotification(string message);
    }

}
