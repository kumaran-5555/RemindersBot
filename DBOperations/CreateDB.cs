using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure;


namespace DBOperations
{
    class UserToAlarms : TableEntity
    {
        public UserToAlarms(string userId, string alarmId)
        {
            this.PartitionKey = userId;
            this.RowKey = alarmId;
            this.CreatedDate = DateTime.Now;
            this.IsActive = true;
        }

        public DateTime CreatedDate { get; }
        public bool IsActive { get; set; }
    }

    

    class AlarmInfo: TableEntity
    {

        public enum AlarmType {  Once, Daily, Weekly, Monthly, Yearly};

        public AlarmInfo(string alarmId, AlarmType alarmType, DateTime timeOfAlarm, DateTime reminderInterval, DateTime reminderStartOffset)
        {

            this.PartitionKey = alarmId;
            this.RowKey = alarmType.ToString();

            this.TimeOfAlarm = timeOfAlarm;
            this.ReminderInterval = reminderInterval;
            this.ReminderStartOffset = reminderStartOffset;

        }

        public DateTime TimeOfAlarm { get; set; }
        public DateTime ReminderInterval { get; set; }
        public DateTime ReminderStartOffset { get; set; }




    }

    
    class CreateDB
    {
        static void Main(string[] args)
        {
            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("UserToAlarms");
            
            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            table = tableClient.GetTableReference("AlarmInfo");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();
        }
    }
}
