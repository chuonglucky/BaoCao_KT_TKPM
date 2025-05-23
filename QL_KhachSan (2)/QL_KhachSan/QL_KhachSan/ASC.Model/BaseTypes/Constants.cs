using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Model.BaseTypes
{
    public class Constants
    {

    }

    public enum Roles
    {
        Admin,
        Receptionist,
        Housekeeping,
        Manager,
        Customer // Đã thêm Customer
    }

    public enum MasterKeys
    {
        RoomType, ServiceType
    }

    public enum Status
    {
        New, Cancelled, Pending, Confirmed, CheckedIn, CheckedOut, Maintenance
    }
}
