namespace Compentio.Ankara.Repositories.ConnectionFactory
{
    using System.Data.Common;

    public interface IAdoConnectionFactory
    {
        DbConnection CreateConnection();
    }
}
