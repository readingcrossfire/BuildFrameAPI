using CONNECTION.Interface;

namespace DAL.DAL_Logs
{
    public partial class DAL_Logs
    {
        public readonly IDapperConnectionDI _dapperConnectionDI;

        public DAL_Logs(IDapperConnectionDI dapperConnectionDI)
        {
            this._dapperConnectionDI = dapperConnectionDI;
        }
    }
}