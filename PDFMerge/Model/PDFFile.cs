using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFMerge.Model
{
    public class PDFFile : GalaSoft.MvvmLight.ObservableObject
    {
        private string _path;

        public string Path
        {
            get { return _path; }
            set { Set<string>(()=>this.Path, ref _path, value); }
        }

    }
}
