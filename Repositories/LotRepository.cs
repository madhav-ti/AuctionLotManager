using AuctionLotManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AuctionLotManager.Repositories
{
    public class LotRepository
    {
        private readonly string _connectionString;

        public LotRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // Get all lots
        public IEnumerable<Lot> GetAllLots()
        {
            var lots = new List<Lot>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetLots", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lots.Add(new Lot
                        {
                            //LotID = (int)reader["LotID"],
                            //Title = reader["Title"].ToString(),
                            //StartPrice = Convert.ToDecimal(reader["StartPrice"]),
                            // CurrentBid = Convert.ToDecimal(reader["CurrentBid"]),
                            //StartTime = Convert.ToDateTime(reader["StartTime"]),
                            //EndTime = Convert.ToDateTime(reader["EndTime"])

                            // Checking for DBNull.Value before converting
                            LotID = reader["LotID"] != DBNull.Value ? Convert.ToInt32(reader["LotID"]) : 0,
                            Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : string.Empty,
                            StartPrice = reader["StartPrice"] != DBNull.Value ? Convert.ToDecimal(reader["StartPrice"]) : 0,
                            CurrentBid = reader["CurrentBid"] != DBNull.Value ? Convert.ToDecimal(reader["CurrentBid"]) : 0,
                            StartTime = reader["StartTime"] != DBNull.Value ? Convert.ToDateTime(reader["StartTime"]) : DateTime.MinValue,
                            EndTime = reader["EndTime"] != DBNull.Value ? Convert.ToDateTime(reader["EndTime"]) : DateTime.MinValue
                        });
                    }
                }
            }

            return lots;
        }

        // Add new lot
        public void AddLot(Lot lot)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddLot", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Title", lot.Title);
                cmd.Parameters.AddWithValue("@StartPrice", lot.StartPrice);
                cmd.Parameters.AddWithValue("@CurrentBid", lot.CurrentBid);
                cmd.Parameters.AddWithValue("@StartTime", lot.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", lot.EndTime);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Get Lot by ID
        //public Lot GetLotById(int id)
        //{
        //    Lot lot = null;
        //    using (SqlConnection con = new SqlConnection(_connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM Lots WHERE LotID=@LotID", con);
        //        cmd.Parameters.AddWithValue("@LotID", id);

        //        con.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            lot = new Lot
        //            {
        //                LotID = (int)reader["LotID"],
        //                Title = reader["Title"].ToString(),
        //                StartPrice = (decimal)reader["StartPrice"],
        //                CurrentBid = (decimal)reader["CurrentBid"],
        //                StartTime = (DateTime)reader["StartTime"],
        //                EndTime = (DateTime)reader["EndTime"]
        //            };
        //        }
        //    }
        //    return lot;
        //}

        public Lot GetLotById(int id)
        {
            Lot lot = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetLotById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LotID", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lot = new Lot
                    {
                        LotID = (int)reader["LotID"],
                        Title = reader["Title"].ToString(),
                        StartPrice = (decimal)reader["StartPrice"],
                        CurrentBid = (decimal)reader["CurrentBid"],
                        StartTime = (DateTime)reader["StartTime"],
                        EndTime = (DateTime)reader["EndTime"]
                    };
                }
            }
            return lot;
        }

        // Update Lot
        public void UpdateLot(Lot lot)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateLot", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LotID", lot.LotID);
                cmd.Parameters.AddWithValue("@Title", lot.Title);
                cmd.Parameters.AddWithValue("@StartPrice", lot.StartPrice);
                cmd.Parameters.AddWithValue("@CurrentBid", lot.CurrentBid);
                cmd.Parameters.AddWithValue("@StartTime", lot.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", lot.EndTime);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Delete Lot
        public void DeleteLot(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteLot", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LotID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Lot> GetActiveLots()
        {
            var lots = new List<Lot>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetActiveLots", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lots.Add(new Lot
                        {
                            LotID = (int)reader["LotID"],
                            Title = reader["Title"].ToString(),
                            StartPrice = reader["StartPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["StartPrice"]),
                            CurrentBid = reader["CurrentBid"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["CurrentBid"]),
                            StartTime = reader["StartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["StartTime"]),
                            EndTime = reader["EndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["EndTime"])
                        });
                    }
                }
            }

            return lots;  // ✅ Ensure we always return a value
        }


    }
}