namespace NineChronicles.DataProvider.GraphQL.Types
{
    using global::GraphQL.Types;
    using NineChronicles.DataProvider.Store.Models;

    public class StageRankingRecordType : ObjectGraphType<StageRankingRecord>
    {
        public StageRankingRecordType()
        {
            Field(x => x.Name);
            Field(x => x.ClearedStageId);

            Name = "StageRankingRecord";
        }
    }
}
