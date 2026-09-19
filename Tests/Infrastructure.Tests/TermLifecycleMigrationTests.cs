using Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Xunit;

namespace Infrastructure.Tests;

public sealed class TermLifecycleMigrationTests
{
    [Fact]
    public void Up_KeepsAnAcademicYearIndexWhileReplacingTheSemesterUniqueIndex()
    {
        var operations = new TestableMigration().BuildUpOperations();

        AssertIndexReplacementOrder(
            operations,
            "uq_semesters_year_name",
            "uq_semesters_year_order");
    }

    [Fact]
    public void Down_KeepsAnAcademicYearIndexWhileRestoringTheSemesterUniqueIndex()
    {
        var operations = new TestableMigration().BuildDownOperations();

        AssertIndexReplacementOrder(
            operations,
            "uq_semesters_year_order",
            "uq_semesters_year_name");
    }

    private static void AssertIndexReplacementOrder(
        IReadOnlyList<MigrationOperation> operations,
        string oldIndex,
        string replacementIndex)
    {
        const string temporaryIndex = "ix_semesters_academic_year_id_migration";
        var createTemporary = FindIndex<CreateIndexOperation>(operations, temporaryIndex);
        var dropOld = FindIndex<DropIndexOperation>(operations, oldIndex);
        var createReplacement = FindIndex<CreateIndexOperation>(operations, replacementIndex);
        var dropTemporary = FindIndex<DropIndexOperation>(operations, temporaryIndex);

        Assert.True(createTemporary >= 0);
        Assert.True(createTemporary < dropOld);
        Assert.True(dropOld < createReplacement);
        Assert.True(createReplacement < dropTemporary);
    }

    private static int FindIndex<TOperation>(
        IReadOnlyList<MigrationOperation> operations,
        string name)
        where TOperation : MigrationOperation
    {
        for (var index = 0; index < operations.Count; index++)
        {
            if (operations[index] is TOperation operation &&
                operation switch
                {
                    CreateIndexOperation create => create.Name == name,
                    DropIndexOperation drop => drop.Name == name,
                    _ => false
                })
            {
                return index;
            }
        }

        return -1;
    }

    private sealed class TestableMigration : AddTermLifecycleFields
    {
        public IReadOnlyList<MigrationOperation> BuildUpOperations()
        {
            var builder = new MigrationBuilder("Pomelo.EntityFrameworkCore.MySql");
            Up(builder);
            return builder.Operations;
        }

        public IReadOnlyList<MigrationOperation> BuildDownOperations()
        {
            var builder = new MigrationBuilder("Pomelo.EntityFrameworkCore.MySql");
            Down(builder);
            return builder.Operations;
        }
    }
}
