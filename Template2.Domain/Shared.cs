﻿using System.Configuration;

namespace Template2.Domain
{
    /// <summary>
    /// Shared
    /// </summary>
    public static class Shared
    {
        /// <summary>
        /// Fakeの時Trure（1:Fake）
        /// </summary>
        public static bool IsFake { get; } = (ConfigurationManager.AppSettings["IsFake"] == "1");

        /// <summary>
        /// SQLite 接続子
        /// </summary>
        public static string? SQLiteConnectionString { get; } = ConfigurationManager.AppSettings["SQLiteConnectionString"];

    }
}