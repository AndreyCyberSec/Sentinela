using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public record EnvVaultEntity
    {
        public string Text { get; init; }
        public DateOnly Created { get; init; }

        public EnvVaultEntity() { }

        public EnvVaultEntity(string Text)
        {
            this.Text = Text;
            
        }
    }
}
