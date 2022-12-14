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
    public class OrderQuery : IOrderQuery
    {
        private readonly IDbConnection _connection;

        public OrderQuery(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<ProductViewModel>> Get(Guid userId)
        {
            var query = new Query();

            query.SelectRaw(@"
                            p.ImagePath,
                            p.Name,
                            p.Price,
                            p.PromotionalPrice,
                            i.Quantity,
                            i.TotalValue");
            query.FromRaw(@"[Order] o

                        join OrderItem i
                        on o.Id = i.OrderId

                        join Product p
                        on i.ProductId = p.Id");

            query.WhereFalse("o.IsDeleted");
            query.Where("o.UserId", userId);

            SqlServerCompiler compiler = new SqlServerCompiler() { UseLegacyPagination = false };
            var sqlResult = compiler.Compile(query);

            return (List<ProductViewModel>)await _connection.QueryAsync<ProductViewModel>(sqlResult.Sql, param: sqlResult.NamedBindings);
        }

        public async Task<List<ProductViewModel>> GetById(Guid id)
        {
            var query = new Query();

            query.SelectRaw(@"
                            p.ImagePath,
                            p.Name,
                            p.Price,
                            p.PromotionalPrice,
                            i.Quantity,
                            i.TotalValue");
            query.FromRaw(@"[Order] o

                        join OrderItem i
                        on o.Id = i.OrderId

                        join Product p
                        on i.ProductId = p.Id");

            query.WhereFalse("o.IsDeleted");
            query.Where("o.Id", id);

            SqlServerCompiler compiler = new SqlServerCompiler() { UseLegacyPagination = false };
            var sqlResult = compiler.Compile(query);

            return (List<ProductViewModel>)await _connection.QueryAsync<ProductViewModel>(sqlResult.Sql, param: sqlResult.NamedBindings);
        }
    }
}
