using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class EnvVaultEntity
    {
        public string Text { get; set; }
        public DateOnly Created { get; set; }

        public EnvVaultEntity() { }

        public EnvVaultEntity(string Text)
        {
            this.Text = Text;
            
        }
    }
}
