namespace NineChronicles.DataProvider.GraphQL.Types
{
    using System.Collections.Generic;
    using global::GraphQL;
    using global::GraphQL.Types;
    using NineChronicles.DataProvider.Store;
    using NineChronicles.DataProvider.Store.Models;

    internal class NineChroniclesSummaryQuery : ObjectGraphType
    {
        public NineChroniclesSummaryQuery(MySqlStore store)
        {
            Store = store;
            Field<ListGraphType<HackAndSlashType>>(
                name: "HackAndSlashQuery",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "agent_address" },
                    new QueryArgument<IntGraphType> { Name = "limit" }
                ),
                resolve: context =>
                {
                    string? agentAddress = context.GetArgument<string?>("agent_address", null);
                    int? limit = context.GetArgument<int?>("limit", null);
                    return Store.GetHackAndSlash(agentAddress, limit);
                });
            Field<ListGraphType<StageRankingRecordType>>(
                name: "StageRanking",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "limit" }
                ),
                resolve: context =>
                {
                    return new List<StageRankingRecord>
                    {
                        new StageRankingRecord { Name = "무뼈닭발 #1234", ClearedStageId = 200, },
                        new StageRankingRecord { Name = "국물닭발 #1234", ClearedStageId = 199, },
                        new StageRankingRecord { Name = "계란찜 #1234", ClearedStageId = 198, },
                    };
                }
            );
        }

        private MySqlStore Store { get; }
    }
}
