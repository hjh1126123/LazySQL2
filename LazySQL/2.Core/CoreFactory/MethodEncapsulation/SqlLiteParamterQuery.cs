﻿using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.Tools;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySQL.Core.CoreFactory.MethodEncapsulation
{
    public class SqlLiteParamterQuery : IParamterQuery
    {
        ListBlueprint listBlueprint;
        public SqlLiteParamterQuery(ListBlueprint listBlueprint)
        {
            this.listBlueprint = listBlueprint;
        }

        protected override void CircleBuild(CodeStatementCollection codeStatementCollection)
        {
            codeStatementCollection.Add(stringBuilderBlueprint.Append($"@{fieldName}"));
            codeStatementCollection.Add(stringBuilderBlueprint.AppendField("i"));
            codeStatementCollection.Add(ToolManager.Instance.ConditionTool.CreateConditionCode($"i != ({fieldName}List.Count - 1)", () =>
            {
                CodeStatementCollection codeStatementCollectionTmpIF = new CodeStatementCollection();
                codeStatementCollectionTmpIF.Add(stringBuilderBlueprint.Append(","));
                return codeStatementCollectionTmpIF;
            }));

            SqlLiteParmsBlueprint parameterBlueprint = new SqlLiteParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\" + i", $"{fieldName}List[i]"));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }

        protected override void normalBuild(CodeStatementCollection codeStatementCollection)
        {
            SqlLiteParmsBlueprint parameterBlueprint = new SqlLiteParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\"", $"{fieldName}"));
            codeStatementCollection.Add(listBlueprint.Add(fieldName));
        }
    }
}
