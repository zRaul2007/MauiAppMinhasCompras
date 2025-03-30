using MauiAppMinhasCompras.Models;
using SQLite;
using static SQLite.SQLiteConnection;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();

            // Força a criação da coluna Categoria se ainda não existir
            _conn.CreateTableAsync<Produto>().Wait();

            // Verifica se a tabela possui a coluna Categoria
            var colunas = _conn.QueryAsync<ColumnInfo>("PRAGMA table_info(Produto)").Result;

            // Se a coluna "Categoria" não existir, adiciona
            if (!colunas.Any(c => c.Name == "Categoria"))
            {
                _conn.ExecuteAsync("ALTER TABLE Produto ADD COLUMN Categoria TEXT DEFAULT 'Sem Categoria'").Wait();
            }
        }

        // Classe auxiliar para capturar informações das colunas
        public class ColumnInfo
        {
            public string Name { get; set; }
        }

        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<int> Update(Produto p)
        {
            return _conn.UpdateAsync(p);
        }

        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q, string categoria = null)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE ?";

            if (!string.IsNullOrEmpty(categoria))
            {
                sql += " AND categoria = ?";
                return _conn.QueryAsync<Produto>(sql, $"%{q}%", categoria);
            }

            return _conn.QueryAsync<Produto>(sql, $"%{q}%");
        }

    }
}
