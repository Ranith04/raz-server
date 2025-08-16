#nullable disable
using System.Data;

namespace Nconnect.Entities;

public class TxConnection
{
    public TxConnection() { }
    public TxConnection(IDbConnection con, IDbTransaction transaction)
    {
        this.con = con;
        this.transaction = transaction;
    }
    public IDbConnection con { get; set; }
    public IDbTransaction transaction { get; set; }
}
