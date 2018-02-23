using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globeweigh.Model
{

    public enum eDispatchOrderType
    {
        Opera = 0,
        Adhoc = 1,
    }

    public enum eRefDataType
    {
        HaulierVehicle =1,
        Location = 5,
        Haulier = 9
    }

    public enum eUserRole
    {
        GlobeweighAdmin = 1,
        Accounts = 2,
        Dispatch = 3,
        Sales = 4
    }

    public enum eProductType
    {
        FinishedProduct = 1,
        IntakeProduct = 2,
        Packaging = 3,
        Consumables = 4
    }

    public enum eLocation
    {
        Stock = 1,
        Production = 2,
        Dispatched = 3
    }

    public enum eQaStatus
    {
        Blue = 0,
        Red = 1,
        Amber = 2,
        Green = 3
    }

    public enum eReportType
    {
        PalletLabel = 1
    }

    public enum eQaStatusString
    {
        Blue,
        Red,
        Amber,
        Green
    }

    public enum eEntityType
    {
        Customer = 1,
        Supplier = 2
    }
}
