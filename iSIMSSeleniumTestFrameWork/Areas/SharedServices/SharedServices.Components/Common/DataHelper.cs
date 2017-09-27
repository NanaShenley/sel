using System;
using System.IO;
using SeSugar.Data;

namespace SharedServices.Components.Common
{
    public static class DataHelper
    {
        public static bool BookmarkExists(Guid entity, string docName)
        {
            const string sql = "select count(*) from app.DocumentBookmark where EntityId = @EntityId and RelativeUrl like '%' + @DocName";

            docName = Path.GetFileName(docName);
            var matches = DataAccessHelpers.GetValue<int>(sql, new {EntityId = entity, DocName = docName});
            return matches > 0;
        }

        
    }
}
