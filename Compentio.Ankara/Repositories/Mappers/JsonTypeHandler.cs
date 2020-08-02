namespace Compentio.Ankara.Repositories.Mappers
{
    using Dapper;
    using Newtonsoft.Json;
    using System;
    using System.Data;

    public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = (value == null)
                ? (object)DBNull.Value
                : JsonConvert.SerializeObject(value);
            parameter.DbType = DbType.String;
        }

        public override T Parse(object value)
        {
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }
    }
}
