using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.DataContext
{
    public class DatabaseConnection
    {
        private readonly IConfiguration _configuration;
        private bool _isConnected;

        public DatabaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _isConnected = false;
        }

        public void Connect()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            SetConnection(connectionString);

            _isConnected = true;
        }
        public void Disconnect()
        {
            SetConnection("");

            _isConnected = false;
        }






        
        
        public bool SetConnection(string a)
        {
            return _isConnected;
        }
    }
}