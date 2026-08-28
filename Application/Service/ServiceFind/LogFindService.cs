using Application.InterfacesService.InterfaceFind;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceFind
{
    public class LogFindService : ILogFind
    {
        public List<LogEntity> TopEndpoint(List<LogEntity> logsLidos)
        {
            var topEndpoints = logsLidos
                   .GroupBy(kvp => kvp.GetEndpoints())
                   .OrderByDescending(x => x.Count())
                   .Take(10)
                   .SelectMany(group => group)
                  .ToList();

            return topEndpoints;
        }

        public Dictionary<string, string?> TopIpaddress(List<LogEntity> logsLidos)
        {
         var topAddresses =  logsLidos
                  .Where(x => x != null && !string.IsNullOrWhiteSpace(x.IpAddress))
                  .GroupBy(kvp => kvp.IpAddress)
                  .OrderByDescending(x => x.Count())
                  .Take(10)
                  .ToDictionary(
                      group => group.Key,
                      group => group.Max(x => x.Date)
                  );
            return  topAddresses;
        }
    }
}
