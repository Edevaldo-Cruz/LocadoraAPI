using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

#nullable disable

namespace LocadoraAPI.Migrations
{
    public partial class PopulandoTabelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Clientes(Nome, CPF, DataNascimento) VALUES('Maya Betina Silvana das Neves', '39907502677', '1954-02-01 01:00:00.000' )");
            migrationBuilder.Sql("INSERT INTO Clientes(Nome, CPF, DataNascimento) VALUES('Vera Camila da Luz', '98166468549', '1980-05-16 01:00:00.000' )");
            migrationBuilder.Sql("INSERT INTO Clientes(Nome, CPF, DataNascimento) VALUES('Tânia Josefa Tereza Duarte', '14935178132', '1995-09-27 01:00:00.000' )");
            migrationBuilder.Sql("INSERT INTO Clientes(Nome, CPF, DataNascimento) VALUES('Tiago Enzo Aparício', '89389548110', '1972-04-06 01:00:00.000' )");

            migrationBuilder.Sql("INSERT INTO Filmes(Titulo, ClassificacaoIndicada, Lancamento) VALUES('Hancock', '12', 0 )");
            migrationBuilder.Sql("INSERT INTO Filmes(Titulo, ClassificacaoIndicada, Lancamento) VALUES('Fantastic Beasts: The Crimes of Grindelwald', '0', 0 )");
            migrationBuilder.Sql("INSERT INTO Filmes(Titulo, ClassificacaoIndicada, Lancamento) VALUES('PANTERA NEGRA: WAKANDA PARA SEMPRE', '12', 1 )");
            migrationBuilder.Sql("INSERT INTO Filmes(Titulo, ClassificacaoIndicada, Lancamento) VALUES('Thor Amor e Trovão', '0', 0 )");
            migrationBuilder.Sql("INSERT INTO Filmes(Titulo, ClassificacaoIndicada, Lancamento) VALUES('Adão Negro', '14', 1 )");
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Clientes");
            migrationBuilder.Sql("DELETE FROM Filmes");


        }
    }
}
