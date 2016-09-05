using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLi.NoSql.API.MongoR.Lib.Model
{
    public class Nodes
    {
        private List<string> _report;
        private List<Nodes> _node;

        public string NodeName { get; set; }

        public List<string> Report
        {
            get
            {
                if (_report == null)
                {
                    _report = new List<string>();
                }
                return _report;
            }

        }

        public List<Nodes> Folder
        {
            get
            {
                if (_node == null)
                {
                    _node = new List<Nodes>();
                }
                return _node;
            }

        }

        // public string ReprtList
    }
}
