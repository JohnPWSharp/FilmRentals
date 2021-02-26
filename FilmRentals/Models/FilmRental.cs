using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FilmRentals.Models
{
    public class FilmRental
    {
        internal AppDb Db { get; set; }

        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public int LanguageId { get; set; }
        public int RentalDuration { get; set; }
        public decimal RentalRate { get; set; }
        public int Length { get; set; }
        public decimal ReplacementCost { get; set; }

        public FilmRental()
        { }

        public FilmRental(AppDb db)
        {
            this.Db = db;
        }

        public async Task<FilmRental> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `film_id`, `title`, `description`, `release_year`, `language_id`, `rental_duration`, `rental_rate`, `length`, `replacement_cost`
                                FROM `film` WHERE `film_id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });

            var result = await cmd.ExecuteReaderAsync();

            if (result.HasRows && await result.ReadAsync())
            {
                Populate(this, result);
                return this;
            } else
            {
                return null;
            }
        }

        public async Task<List<FilmRental>> FindAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `film_id`, `title`, `description`, `release_year`, `language_id`, `rental_duration`, `rental_rate`, `length`, `replacement_cost`
                                FROM `film`";

            var result = await cmd.ExecuteReaderAsync();
            var filmRentals = new List<FilmRental>();
            while (await result.ReadAsync())
            {
                var rental = new FilmRental();
                Populate(rental, result);
                filmRentals.Add(rental);
            }
            return filmRentals;
        }

        private void Populate(FilmRental filmRental, MySqlDataReader reader)
        {
            filmRental.FilmId = reader.GetInt32(0);
            filmRental.Title = reader.GetString(1);
            filmRental.Description = reader.GetString(2);
            filmRental.ReleaseYear = reader.GetInt32(3);
            filmRental.LanguageId = reader.GetInt32(4);
            filmRental.RentalDuration = reader.GetInt32(5);
            filmRental.RentalRate = reader.GetDecimal(6);
            filmRental.Length = reader.GetInt32(7);
            filmRental.ReplacementCost = reader.GetDecimal(8);
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `film`(`title`, `description`, `release_year`, `language_id`, `rental_duration`, `rental_rate`, `length`, `replacement_cost`)
                              VALUES (@title, @description, @release_year, @language_id, @rental_duration, @rental_rate, @length, @replacement_cost)";

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@title",
                DbType = DbType.String,
                Value = this.Title
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                DbType = DbType.String,
                Value = this.Description
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@release_year",
                DbType = DbType.Int32,
                Value = this.ReleaseYear
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@language_id",
                DbType = DbType.Int32,
                Value = this.LanguageId
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rental_duration",
                DbType = DbType.Int32,
                Value = this.RentalDuration
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rental_rate",
                DbType = DbType.Decimal,
                Value = this.RentalRate
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@length",
                DbType = DbType.Int32,
                Value = this.Length
            });

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@replacement_cost",
                DbType = DbType.Decimal,
                Value = this.ReplacementCost
            });

            await cmd.ExecuteNonQueryAsync();
            this.FilmId = (int)cmd.LastInsertedId;
        }
    }
}
