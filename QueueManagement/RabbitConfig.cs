using System;
using System.Collections.Generic;
using System.Text;

namespace QueueManagement {
    public class RabbitConfig {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; } = 5672;

        public string VHost { get; set; } = "/";
    }
}
//Below section must be present in appsettings.json
//"rabbit": {  
//"UserName": "guest",  
//"Password": "guest",  
//"HostName": "localhost",  
//"VHost": "/",  
//"Port": 5672  
//}  