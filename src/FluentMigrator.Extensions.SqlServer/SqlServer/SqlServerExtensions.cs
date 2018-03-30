#region License
// Copyright (c) 2007-2018, FluentMigrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;

using FluentMigrator.Builders;
using FluentMigrator.Builders.Create.Constraint;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Builders.Insert;
using FluentMigrator.Infrastructure;

namespace FluentMigrator.SqlServer
{
    public static partial class SqlServerExtensions
    {
        public const string IdentityInsert = "SqlServerIdentityInsert";
        public const string IdentitySeed = "SqlServerIdentitySeed";
        public const string IdentityIncrement = "SqlServerIdentityIncrement";
        public const string ConstraintType = "SqlServerConstraintType";
        public const string IncludesList = "SqlServerIncludes";
        public const string OnlineIndex = "SqlServerOnlineIndex";
        public const string RowGuidColumn = "SqlServerRowGuidColumn";
        public const string IndexColumnNullsDistinct = "SqlServerIndexColumnNullsDistinct";

        /// <summary>
        /// Inserts data using Sql Server's IDENTITY INSERT feature.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IInsertDataSyntax WithIdentityInsert(this IInsertDataSyntax expression)
        {
            ISupportAdditionalFeatures castExpression = expression as ISupportAdditionalFeatures;
            if (castExpression == null)
            {
                throw new InvalidOperationException("WithIdentityInsert must be called on an object that implements ISupportAdditionalFeatures.");
            }
            castExpression.AdditionalFeatures[IdentityInsert] = true;
            return expression;
        }

        private static void SetConstraintType(ICreateConstraintOptionsSyntax expression, SqlServerConstraintType type)
        {
            if (!(expression is ISupportAdditionalFeatures additionalFeatures)) throw new InvalidOperationException(type + " must be called on an object that implements ISupportAdditionalFeatures.");

            additionalFeatures.AdditionalFeatures[ConstraintType] = type;
        }

        public static void Clustered(this ICreateConstraintOptionsSyntax expression)
        {
            SetConstraintType(expression, SqlServerConstraintType.Clustered);
        }

        public static void NonClustered(this ICreateConstraintOptionsSyntax expression)
        {
            SetConstraintType(expression, SqlServerConstraintType.NonClustered);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax RowGuid(this ICreateTableColumnOptionOrWithColumnSyntax expression)
        {
            var columnExpression = expression as IColumnExpressionBuilder ?? throw new InvalidOperationException("The Include method must be called on an object that implements IColumnExpressionBuilder.");
            columnExpression.Column.AdditionalFeatures[RowGuidColumn] = true;
            return expression;
        }

        private static ISupportAdditionalFeatures GetColumn<TNext, TNextFk>(IColumnOptionSyntax<TNext, TNextFk> expression) where TNext : IFluentSyntax where TNextFk : IFluentSyntax
        {
            if (expression is IColumnExpressionBuilder cast1) return cast1.Column;

            throw new InvalidOperationException("The seeded identity method can only be called on a valid object.");
        }
    }
}
