using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontenery
{
    public interface IContainer
    {
        string serialNumber { get; }
        double loadMass { get; set; }
        double height { get; }
        double ownWeight { get; }
        double depth { get; }
        double maxLoadCapacity { get; }
        void LoadCargo(double mass);
        void UnloadCargo();
        void PrintContainerInfo();
    }

}
