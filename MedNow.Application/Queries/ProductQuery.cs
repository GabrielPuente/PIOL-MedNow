using Dapper;
using MedNow.Domain.Contracts.Queries;
using MedNow.Domain.ViewModels;
using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MedNow.Application.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly IDbConnection _connection;

        public ProductQuery(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<ProductViewModel>> Get()
        {
            var query = new Query();

            query.SelectRaw(@"
                            Id,
                            Name,
                            Price,
                            PromotionalPrice,
                            ImagePath,
                            Description");
            query.FromRaw("Product");

            query.WhereFalse("IsDeleted");
             
            SqlServerCompiler compiler = new SqlServerCompiler() { UseLegacyPagination = false };
            var sqlResult = compiler.Compile(query);

            return (List<ProductViewModel>)await _connection.QueryAsync<ProductViewModel>(sqlResult.Sql, param: sqlResult.NamedBindings);
        }

        public async Task<ProductViewModel> GetById(Guid id)
        {
            var query = new Query();

            query.SelectRaw(@"
                            Id,
                            Name,
                            Price,
                            PromotionalPrice,
                            ImagePath,
                            Description");
            query.FromRaw("Product");

            query.WhereFalse("IsDeleted");

            query.Where("Id", id);
            query.WhereFalse("IsDeleted");

            SqlServerCompiler compiler = new SqlServerCompiler() { UseLegacyPagination = false };
            var sqlResult = compiler.Compile(query);

            return await _connection.QueryFirstOrDefaultAsync<ProductViewModel>(sqlResult.Sql, param: sqlResult.NamedBindings);
        }
    }
}
