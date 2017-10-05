using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaTraverse.Models
{
    public class AppProperties : IAppProperties
    {
        private bool _IsLoginActive = false;
        public bool IsLoginActive
        {
            get { return _IsLoginActive; }
            set { _IsLoginActive = value; }
        }
    }
}
